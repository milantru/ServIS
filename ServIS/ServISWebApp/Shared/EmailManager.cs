using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using Syncfusion.Blazor.Data;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Collections.Concurrent;

namespace ServISWebApp.Shared
{
    /// <summary>
    /// Provides functionality for managing and processing emails.
    /// </summary>
    public class EmailManager : IDisposable
	{
		/* We use semaphore to limit IMAP connections to 15 at the same time because Gmail limits it to 15,
		 * see: https://github.com/jstedfast/MailKit/issues/377 
		 * or: https://support.google.com/mail/answer/7126229?hl=en&visit_id=638223334600390557-661682674&rd=1#zippy=%2Ctoo-many-simultaneous-connections-error 
		 * Even though I don't see docs mentioning there SMTP connection limitations, we limit it also using this semaphore
		 * because of defensive programming, it shouldn't make much of a performance difference as we don't expect the user
		 * to be sending a lot of emails very quickly. */
		private readonly SemaphoreSlim semaphore = new(initialCount: 15, maxCount: 15);
		private readonly ILogger<EmailManager> logger;
		private readonly string emailPassword;
		private readonly MessageSummaryItems defaultMessageSummaryItems = MessageSummaryItems.UniqueId
															| MessageSummaryItems.GMailThreadId
															| MessageSummaryItems.PreviewText
															| MessageSummaryItems.InternalDate
															| MessageSummaryItems.Flags
															| MessageSummaryItems.Envelope
															| MessageSummaryItems.Headers;

        /// <summary>
        /// Represents the email name used when sending messages.
        /// </summary>
        public string EmailName { get; private init; } = null!;

        /// <summary>
        /// Represents the email address used for sending messages.
        /// </summary>
        public string EmailAddress { get; private init; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailManager"/> class.
        /// </summary>
        /// <param name="emailName">The email name used when sending messages.</param>
        /// <param name="emailAddress">The email address used for sending messages.</param>
        /// <param name="emailPassword">The password of the email used for sending mesages.</param>
        /// <param name="logger">The object used for logging.</param>
        public EmailManager(string emailName, string emailAddress, string emailPassword, ILogger<EmailManager> logger)
		{
			EmailName = emailName;
			EmailAddress = emailAddress;
			this.emailPassword = emailPassword;
			this.logger = logger;
		}

		/// <summary>
		/// Retrieves a list of threads asynchronously based on the specified page number and the number of items per page.
		/// </summary>
		/// <remarks>
		/// This method should be used for the initial loading of threads on page <paramref name="pageNumber"/>.
		/// If it is required to update the threads object returned by this method, the <see cref="UpdateThreadsAsync(List{Thread}, int, int)"/>
		/// should be used.
		/// </remarks>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageItemsCount">The number of threads that should be displayed per page.</param>
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains a tuple containing the list of retrieved threads 
		/// and the total count of ALL existing threads (not just the ones returned for the page <paramref name="pageNumber"/>).
		/// </returns>
		/// <exception cref="ImapCommandException">
		/// Yet unknown reason for throwing this exception, but it seems it is thrown sometimes when making too many requests.
		/// </exception>
		public async Task<(List<Thread>, int)> GetThreadsAsync(int pageNumber, int pageItemsCount)
		{
			var skip = (pageNumber - 1) * pageItemsCount;
			var take = pageItemsCount;

			var threadGroups = await GetMessageSummariesPerThreadAsync();

			var allThreadsCount = threadGroups.Count();

			threadGroups = threadGroups.OrderByDescending(g => g.Max(msgSumm => msgSumm.Date))
									.Skip(skip)
									.Take(take);

			var threadsBag = new ConcurrentBag<Thread>();
			var createThreadTasks = new List<Task>();

			foreach (var threadGroup in threadGroups)
			{
				var createThreadTask = Task.Run(async () =>
				{
					var threadId = threadGroup.Key!.Value;

					var thread = await CreateThreadAsync(threadId, threadGroup);

					var shouldSkipThread = await ShouldSkipAsync(thread);
					if (!shouldSkipThread)
					{
						threadsBag.Add(thread);
					}
				});

				createThreadTasks.Add(createThreadTask);
			}

			await Task.WhenAll(createThreadTasks);

			var threads = threadsBag.OrderBy(t => t.Messages.Last().DateTime).ToList();

			return (threads, allThreadsCount);
		}

		/// <summary>
		/// Updates the given list of threads asynchronously based on the specified page number and the number of items per page.
		/// </summary>
		/// <param name="threads">The list of threads to update.</param>
		/// <param name="pageNumber">The page number on which the threads were displayed.</param>
		/// <param name="pageItemsCount">The number of threads that should be displayed per page.</param>
		/// <remarks>
		/// This method is supposed to be an optimization. For initial loading of threads on page the method <see cref="GetThreadsAsync(int, int)"/> 
		/// should be used. If the page hasn't changed (didn't move to another page) and the update/reload of threads is required, the threads object 
		/// returned from <see cref="GetThreadsAsync(int, int)"/> should be passed to this method to be updated. This way we limit the async calls 
		/// for information from Gmail (it is too slow).
		/// </remarks>
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains a tuple containing the list of updated threads 
		/// and the total count of ALL existing threads (not just the updated ones).
		/// </returns>
		/// <exception cref="ImapCommandException">
		/// Yet unknown reason for throwing this exception, but it seems it is thrown sometimes when making too many requests.
		/// </exception>
		public async Task<(List<Thread>, int)> UpdateThreadsAsync(List<Thread> threads, int pageNumber, int pageItemsCount)
		{
			var skip = (pageNumber - 1) * pageItemsCount;
			var take = pageItemsCount;

			var threadGroups = await GetMessageSummariesPerThreadAsync();

			var allThreadsCount = threadGroups.Count();

			threadGroups = threadGroups.OrderByDescending(g => g.Max(msgSumm => msgSumm.Date))
									.Skip(skip)
									.Take(take);

			var threadsBag = new ConcurrentBag<Thread>(threads);
			var loadThreadTasks = new List<Task>();

			foreach (var threadGroup in threadGroups)
			{
				var loadThreadTask = Task.Run(async () =>
				{
					var threadId = threadGroup.Key!.Value;

					var thread = threadsBag.FirstOrDefault(t => t.Id == threadId);

					if (thread is not null)
					{// existing thread
						await UpdateThreadAsync(thread, threadGroup);
					}
					else
					{// new thread
						var newThread = await CreateThreadAsync(threadId, threadGroup);

						var shouldSkipThread = await ShouldSkipAsync(newThread);
						if (!shouldSkipThread)
						{
							threadsBag.Add(newThread);
						}
					}
				});

				loadThreadTasks.Add(loadThreadTask);
			}

			await Task.WhenAll(loadThreadTasks);

			threads = threadsBag.TakeLast(take) // in case new threads have appeared we want to return only *take* newest threads
							.OrderBy(t => t.Messages.Last().DateTime)
							.ToList();

			return (threads, allThreadsCount);
		}

		/// <summary>
		/// Asynchronously retrieves the list of emails associated with the <paramref name="uniqueIds"/>.
		/// </summary>
		/// <param name="uniqueIds">The unique identifiers of the emails.</param>
		/// <returns>A task that represents the asynchronous operation.
		/// The task result contains the list of emails associated with the provided unique identifiers.</returns>
		public async Task<List<Email>> GetEmailsAsync(IList<UniqueId> uniqueIds)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var emailsReadStatuses = await GetEmailsReadStatusesAsync(allMail, uniqueIds);

			var emails = new List<Email>();
			for (int i = 0; i < uniqueIds.Count; i++)
			{
				var uid = uniqueIds[i];
				var message = await allMail.GetMessageAsync(uid);
				var emailRead = emailsReadStatuses.ElementAt(i);

				var from = message.From.Mailboxes.First();
				var to = message.To.Mailboxes.First();
				var replyTo = message.ReplyTo.Mailboxes.FirstOrDefault();

				var email = new Email
				{
					Uid = uid,
					MessageId = message.MessageId,
					References = message.References,
					Headers = message.Headers.ToList(),
					FromName = from.Name,
					FromAddress = from.Address,
					ToName = to.Name,
					ToAddress = to.Address,
					ReplyToName = replyTo?.Name!,
					ReplyToAddress = replyTo?.Address!,
					Subject = message.Subject,
					Text = message.TextBody,
					IsRead = emailRead,
					DateTime = message.Date.LocalDateTime
				};

				emails.Add(email);
			}

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();

			return emails;
		}

        /// <summary>
        /// Asynchronously retrieves an email associated with the provided <paramref name="uniqueId"/>.
        /// </summary>
        /// <param name="uniqueId">The unique identifier of the email.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the retrieved email associated with the provided unique identifier.</returns>
        public async Task<Email> GetEmailAsync(UniqueId uniqueId)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var emailRead = (await GetEmailsReadStatusesAsync(allMail, new List<UniqueId> { uniqueId })).First();

			var message = await allMail.GetMessageAsync(uniqueId);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();

			var from = message.From.Mailboxes.First();
			var to = message.To.Mailboxes.First();
			var replyTo = message.ReplyTo.Mailboxes.FirstOrDefault();

			var email = new Email
			{
				Uid = uniqueId,
				MessageId = message.MessageId,
				References = message.References,
				Headers = message.Headers.ToList(),
				FromName = from.Name,
				FromAddress = from.Address,
				ToName = to.Name,
				ToAddress = to.Address,
				ReplyToName = replyTo?.Name!,
				ReplyToAddress = replyTo?.Address!,
				Subject = message.Subject,
				Text = message.TextBody,
				IsRead = emailRead,
				DateTime = message.Date.LocalDateTime
			};

			return email;
		}

        /// <summary>
        /// Asynchronously retrieves the message summaries associated with the provided <paramref name="uniqueIds"/> 
		/// containing info based on the specified <paramref name="items"/>.
        /// </summary>
        /// <param name="uniqueIds">The unique identifiers of the messages.</param>
        /// <param name="items">The message summary items to retrieve (optional).</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the list of message summaries associated with the provided unique identifiers.</returns>
        public async Task<IList<IMessageSummary>> GetMessageSummariesAsync(IList<UniqueId> uniqueIds, MessageSummaryItems? items = null)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var messageSummaries = await allMail.FetchAsync(uniqueIds, items ?? defaultMessageSummaryItems);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();

			return messageSummaries;
		}

        /// <summary>
        /// Asynchronously retrieves the message summary based on the provided <paramref name="uniqueId"/> 
		/// containing info based on the specified <paramref name="items"/>.
        /// </summary>
        /// <param name="uniqueId">The unique identifier of the message.</param>
        /// <param name="items">The message summary items to retrieve (optional).</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the retrieved message summary associated with the provided unique identifier.</returns>
        public async Task<IMessageSummary> GetMessageSummaryAsync(UniqueId uniqueId, MessageSummaryItems? items = null)
		{
			var itemsToPass = items ?? defaultMessageSummaryItems;
			var messageSummary = (await GetMessageSummariesAsync(new List<UniqueId> { uniqueId }, itemsToPass)).First();

			return messageSummary;
		}

        /// <summary>
        /// Asynchronously marks an email as read.
        /// </summary>
        /// <param name="uniqueId">The unique identifier of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsReadAsync(UniqueId uniqueId)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.AddFlagsAsync(uniqueId, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();
		}

        /// <summary>
        /// Asynchronously marks emails as read.
        /// </summary>
        /// <param name="uniqueIds">The unique identifiers of the emails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsReadAsync(IList<UniqueId> uniqueIds)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.AddFlagsAsync(uniqueIds, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();
		}

        /// <summary>
        /// Asynchronously marks an email as read.
        /// </summary>
        /// <param name="email">The email to be marked as read.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsReadAsync(Email email)
		{
			await MarkEmailAsReadAsync(email.Uid);

			email.IsRead = true;
		}

        /// <summary>
        /// Asynchronously marks emails as read.
        /// </summary>
        /// <param name="emails">The emails to be marked as read.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsReadAsync(List<Email> emails)
		{
			var uids = emails.Select(e => e.Uid).ToList();

			await MarkEmailAsReadAsync(uids);

			emails.ForEach(e => e.IsRead = true);
		}

        /// <summary>
        /// Asynchronously marks an email as unread.
        /// </summary>
        /// <param name="uniqueId">The unique identifier of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsUnreadAsync(UniqueId uniqueId)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.RemoveFlagsAsync(uniqueId, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();
		}

        /// <summary>
        /// Asynchronously marks emails as unread.
        /// </summary>
        /// <param name="uniqueIds">The unique identifiers of the emails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsUnreadAsync(IList<UniqueId> uniqueIds)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.RemoveFlagsAsync(uniqueIds, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();
		}

        /// <summary>
        /// Asynchronously marks an email as unread.
        /// </summary>
        /// <param name="email">The email to be marked as unread.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsUnreadAsync(Email email)
		{
			await MarkEmailAsUnreadAsync(email.Uid);

			email.IsRead = false;
		}

        /// <summary>
        /// Asynchronously marks emails as unread.
        /// </summary>
        /// <param name="emails">The emails to be marked as unread.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsUnreadAsync(List<Email> emails)
		{
			var uids = emails.Select(e => e.Uid).ToList();

			await MarkEmailAsUnreadAsync(uids);

			emails.ForEach(e => e.IsRead = false);
		}

        /// <summary>
        /// Asynchronously sends an email.
        /// </summary>
        /// <param name="email">The email to be sent.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendEmailAsync(Email email)
		{
			var message = PrepareMessage(email);

			await SendMessageAsync(message);
		}

		/// <summary>
		/// Asynchronously sends a reply to the <paramref name="email"/> with the <paramref name="replyText"/>.
		/// </summary>
		/// <remarks>The <see cref="Email.Uid"/> of returned <see cref="Email"/> is not valid.</remarks>
		/// <param name="email">The email to reply to.</param>
		/// <param name="replyText">The text of the reply.</param>
		/// <param name="toName">Name of the receiver. It is optional. 
		/// If not set, the reply name from <paramref name="email"/> will be used.</param>
		/// <param name="toAddress">Email address of the receiver. It is optional. 
		/// If not set, the reply address from <paramref name="email"/> will be used.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the replied email.</returns>
		public async Task<Email> ReplyToAsync(Email email, string replyText, string? toName = null, string? toAddress = null)
		{
			var reply = PrepareReply(email, replyText, toName, toAddress);

			await SendMessageAsync(reply);

			var replyEmail = new Email
			{
				MessageId = reply.MessageId,
				References = reply.References,
				FromName = EmailName,
				FromAddress = EmailAddress,
				ToName = toName ?? email.ReplyToName,
				ToAddress = toAddress ?? email.ReplyToAddress,
				Subject = reply.Subject,
				Text = reply.TextBody,
				IsRead = true,
				DateTime = reply.Date.LocalDateTime
			};

			return replyEmail;
		}

        /// <summary>
        /// Asynchronously deletes an email.
        /// </summary>
        /// <param name="uniqueId">The unique identifier of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteEmailAsync(UniqueId uniqueId)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var trash = imapClient.GetFolder(SpecialFolder.Trash);
			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			var moved = await allMail.MoveToAsync(uniqueId, trash);
			await allMail.CloseAsync();
			if (moved.HasValue)
			{
				trash.Open(FolderAccess.ReadWrite);
				trash.AddFlags(moved.Value, MessageFlags.Deleted, true);
				trash.Expunge(new[] { moved.Value });
				await trash.CloseAsync();
			}

			await imapClient.DisconnectAsync(true);
			semaphore.Release();
		}

        /// <summary>
        /// Asynchronously deletes emails.
        /// </summary>
        /// <param name="uniqueIds">The unique identifier of the emails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteEmailAsync(IList<UniqueId> uniqueIds)
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var trash = imapClient.GetFolder(SpecialFolder.Trash);
			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			var moved = (await allMail.MoveToAsync(uniqueIds, trash)).Destination;
			await allMail.CloseAsync();
			if (moved.Count > 0)
			{
				trash.Open(FolderAccess.ReadWrite);
				trash.AddFlags(moved, MessageFlags.Deleted, true);
				trash.Expunge(moved);
				await trash.CloseAsync();
			}

			await imapClient.DisconnectAsync(true);
			semaphore.Release();
		}

		public void Dispose()
		{
			semaphore.Dispose();
		}

		/// <summary>
		/// Determines whether should skip the thread. If the thread contains 
		/// only one autogenerated message (e.g. notifying auction winner about victory (it is not meant for admin), 
		/// then this method deletes the email and decides the thread is skippable.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation. The task result contains 
		/// <c>true</c> if the thread contains only one autogenerated message 
		/// (i.e. is not meant for the admin); <c>false</c> otherwise</returns>
		private async Task<bool> ShouldSkipAsync(Thread thread)
		{
			var messages = thread.Messages;
			var firstMessage = messages.First();
			var isSentFromApp = firstMessage.FromAddress == EmailAddress;
			// autogenerated messages are sent to the users e.g. from auction (to notify them if they won or lost)
			var isAutogenerated = firstMessage.Headers?.FirstOrDefault(h => h.Field == "X-ServIS-autogenerated")?.Value == "true";

			if (isSentFromApp && isAutogenerated && messages.Count == 1)
			{
				/* We skip this thread because this was message meant just for the user, 
				 * but for some reason it is sent to both user and admin. Because we don't 
				 * want to bother admin with this message (which is meant only for user anyway),
				 * this if was created to prevent showing this thread/message to admin. 
				 * We also delete it so it won't take up space in gmail. 
				 * However, if for some reason user answered this message (the message says 
				 * it doesn't need reply) we might want to be able to see it, that's why 
				 * the condition messages.Count == 1 is present (it allows to display message to admin
				 * if user replied even though it said not to). */
				await DeleteEmailAsync(firstMessage.Uid);

				return true;
			}

			return false;
		}

		private async Task ConnectImapAsync(ImapClient imapClient)
		{
			await imapClient.ConnectAsync(
					host: "imap.gmail.com",
					port: 993,
					useSsl: true
				);
		}

		private async Task ConnectSmtpAsync(SmtpClient smtpClient)
		{
			await smtpClient.ConnectAsync(
				host: "smtp.gmail.com",
				port: 587,
				options: SecureSocketOptions.StartTls
			);
		}

		private async Task<ImapClient> GetConnectedImapClientAsync()
		{
			var imapClient = new ImapClient();

			try
			{
				await ConnectImapAsync(imapClient);
			}
			catch (SslHandshakeException ex)
			{
				logger.LogWarning(ex, $"Recovering from '{nameof(SslHandshakeException)}', trying to connect anyway.");
				// 3. in -> https://stackoverflow.com/questions/59026301/sslhandshakeexception-an-error-occurred-while-attempting-to-establish-an-ssl-or/#answer-59039909
				imapClient.CheckCertificateRevocation = false;

				await ConnectImapAsync(imapClient);
			}

			await imapClient.AuthenticateAsync(
				userName: EmailAddress,
				password: emailPassword
			);

			return imapClient;
		}

		private async Task<SmtpClient> GetConnectedSmtpClientAsync()
		{
			var smtpClient = new SmtpClient();

			try
			{
				await ConnectSmtpAsync(smtpClient);
			}
			catch (SslHandshakeException ex)
			{
				logger.LogWarning(ex, $"Recovering from '{nameof(SslHandshakeException)}', trying to connect anyway.");
				smtpClient.CheckCertificateRevocation = false;

				await ConnectSmtpAsync(smtpClient);
			}

			await smtpClient.AuthenticateAsync(
				userName: EmailAddress,
				password: emailPassword
			);

			return smtpClient;
		}

		private MimeMessage PrepareMessage(Email email)
		{
			var message = new MimeMessage();

			message.From.Add(new MailboxAddress(email.FromName, email.FromAddress));
			message.To.Add(new MailboxAddress(email.ToName, email.ToAddress));
			message.ReplyTo.Add(new MailboxAddress(email.ReplyToName, email.ReplyToAddress));

			email.Headers?.ForEach(header => message.Headers.Add(header));

			message.Subject = email.Subject;
			message.Body = new TextPart() { Text = email.Text };

			return message;
		}

		private IEnumerable<bool> GetEmailsReadStatuses(IList<IMessageSummary> emailsSummaries)
		{
			var emailsReadStatuses = emailsSummaries.Select(es => es.Flags.HasValue
																	? es.Flags.Value.HasFlag(MessageFlags.Seen)
																	: false);

			return emailsReadStatuses;
		}

		private async Task<IEnumerable<bool>> GetEmailsReadStatusesAsync(IMailFolder folder, IList<UniqueId> uniqueIds)
		{
			var emailsSummaries = await folder.FetchAsync(uniqueIds, MessageSummaryItems.Flags);
			var emailsReadStatuses = GetEmailsReadStatuses(emailsSummaries);

			return emailsReadStatuses;
		}

		private async Task<IEnumerable<bool>> GetEmailsReadStatusesAsync(IMailFolder folder, int fromIndex, int toIndex)
		{
			var emailsSummaries = await folder.FetchAsync(fromIndex, toIndex, MessageSummaryItems.Flags);
			var emailsReadStatuses = GetEmailsReadStatuses(emailsSummaries);

			return emailsReadStatuses;
		}

		private TextPart PreparePreviousMessagesQuotation(Email email, string replyText)
		{
			using var quoted = new StringWriter();

			var sender = string.IsNullOrEmpty(email.FromName) ? email.FromAddress : email.FromName;

			quoted.WriteLine($"Dňa {email.DateTime.ToString("f")}, {sender} napísal:");
			using (var reader = new StringReader(email.Text))
			{
				string? line;

				while ((line = reader.ReadLine()) != null)
				{
					quoted.WriteLine("> " + line);
				}
			}

			var quotation = new TextPart("plain")
			{
				/* It seems like \n\n is sufficient and everything works, but
				 * if I'm not mistaken gmail uses \r\n\r\n so just to make sure everything's ok
				 * I'll use \r\n\r\n as well. */
				Text = replyText + "\r\n\r\n" + quoted.ToString()
			};

			return quotation;
		}

		/// <summary>
		/// Creates a reply message.
		/// </summary>
		/// <remarks>
		/// <paramref name="toName"/> and <paramref name="toAddress"/> can be used e.g. when we  have already replied in a thread to a user message
		/// but then we want to reply again, but this time our message is the last one, if we  replied to this message the user wouldn't get it
		/// but using these parameters we can explicitly specify that even though it is a reply (in a thread) we want user to get the message.
		/// </remarks>
		/// <param name="email">Provides info like subject, thread messages references, message id etc.</param>
		/// <param name="replyText">Text of the reply.</param>
		/// <param name="toName">Name of the receiver. It is optional. 
		/// If not set, the reply name from <paramref name="email"/> will be used.</param>
		/// <param name="toAddress">Email address of the receiver. It is optional. 
		/// If not set, the reply address from <paramref name="email"/> will be used.</param>
		/// <returns>Reply which is an instance of type <see cref="MimeMessage"/>.</returns>
		private MimeMessage PrepareReply(Email email, string replyText, string? toName = null, string? toAddress = null)
		{
			var reply = new MimeMessage();

			reply.From.Add(new MailboxAddress(EmailName, EmailAddress));
			reply.To.Add(new MailboxAddress(toName ?? email.ReplyToName, toAddress ?? email.ReplyToAddress));
			reply.ReplyTo.Add(new MailboxAddress(EmailName, EmailAddress));
			reply.Subject = email.Subject.StartsWith("Re:") ? email.Subject : "Re:" + email.Subject;
			reply.InReplyTo = email.MessageId;
			reply.References.AddRange(email.References);
			reply.References.Add(email.MessageId);

			reply.Body = PreparePreviousMessagesQuotation(email, replyText);

			return reply;
		}

		private string PrepareErrorMessageForFailedEmailSending(MimeMessage message)
		{
			/* message.X.Mailboxes should always be nonempty, but FirstOrDefault is called on it
			 * due to defensive programming, if we see in log that the place is empty, 
			 * we know this is the problem. But once again... it should never happen. 
			 * The only reason it is here instead of First is defensive programming; better  
			 * to have some log message with an empty spot than no log message at all. */
			var from = message.From.Mailboxes.FirstOrDefault()?.Address;
			var to = message.To.Mailboxes.FirstOrDefault()?.Address;
			var replyTo = message.ReplyTo.Mailboxes.FirstOrDefault()?.Address;

			var errMsg = $"Failed to send email from {from} (with reply-to set to {replyTo}) to {to}.\n" +
				"The subject was:\n" +
				$"{message.Subject}\n" +
				"The text was:\n" +
				$"{message.TextBody}\n";

			if (message.Headers is not null && message.Headers.Count > 0)
			{
				errMsg += "Headers:\n";

				foreach (var header in message.Headers)
				{
					errMsg += $"{header.Field}: {header.Value}\n";
				}
			}

			return errMsg;
		}

		private async Task SendMessageAsync(MimeMessage message)
		{
			await semaphore.WaitAsync();
			using var smtpClient = await GetConnectedSmtpClientAsync();

			try
			{
				await smtpClient.SendAsync(message);
			}
			catch (Exception ex)
			{
				var errMsg = PrepareErrorMessageForFailedEmailSending(message);

				logger.LogError(ex, errMsg);
			}

			await smtpClient.DisconnectAsync(true);
			semaphore.Release();
		}

		private async Task<Email> CreateEmailFromAsync(IMessageSummary messageSummary)
		{
			var uid = messageSummary.UniqueId;

			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var message = await allMail.GetMessageAsync(uid);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();

			var emailRead = messageSummary.Flags.HasValue
								? messageSummary.Flags.Value.HasFlag(MessageFlags.Seen)
								: false;


			var from = message.From.Mailboxes.First();
			var to = message.To.Mailboxes.First();
			var replyto = message.ReplyTo.Mailboxes.FirstOrDefault();

			var email = new Email
			{
				Uid = uid,
				MessageId = message.MessageId,
				References = message.References,
				Headers = message.Headers.ToList(),
				FromName = from.Name,
				FromAddress = from.Address,
				ToName = to.Name,
				ToAddress = to.Address,
				ReplyToName = replyto?.Name!,
				ReplyToAddress = replyto?.Address!,
				Subject = message.Subject,
				Text = message.TextBody,
				IsRead = emailRead,
				DateTime = message.Date.LocalDateTime
			};

			return email;
		}

		private async Task<IEnumerable<IGrouping<ulong?, IMessageSummary>>> GetMessageSummariesPerThreadAsync()
		{
			await semaphore.WaitAsync();
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var gmailMessages = await allMail.FetchAsync(0, -1, defaultMessageSummaryItems);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
			semaphore.Release();

			var threadGroups = gmailMessages.GroupBy(g => g.GMailThreadId);

			return threadGroups;
		}

		private void StartUpdatingExistingThreadEmails(
			Thread thread,
			int minCount,
			IOrderedEnumerable<IMessageSummary> messageSummaries,
			List<Task> updateThreadTasks
		)
		{
			for (int i = 0; i < minCount; i++)
			{
				var iCopy = i;

				var updateThreadMessageTask = Task.Run(async () =>
				{
					var email = thread.Messages.ElementAt(iCopy);
					var msgSumm = messageSummaries.ElementAt(iCopy);

					// take newer read/seen value
					email.IsRead = msgSumm.Flags.HasValue
										? msgSumm.Flags.Value.HasFlag(MessageFlags.Seen)
										: false;

					if (email.Uid.IsValid)
					{
						return;
					}

					// now will have thread message valid uid
					thread.Messages[iCopy] = await CreateEmailFromAsync(msgSumm);
				});

				updateThreadTasks.Add(updateThreadMessageTask);
			}
		}

		private ConcurrentBag<Email> StartGettingNewThreadEmails(
			int minCount,
			int maxCount,
			IOrderedEnumerable<IMessageSummary> messageSummaries,
			List<Task> updateThreadTasks
		)
		{
			var newEmails = new ConcurrentBag<Email>();

			for (int i = minCount; i < maxCount; i++)
			{
				var iCopy = i;

				var addNewMessageToThreadTask = Task.Run(async () =>
				{
					var messageSummary = messageSummaries.ElementAt(iCopy);

					var newEmail = await CreateEmailFromAsync(messageSummary);

					newEmails.Add(newEmail);
				});

				updateThreadTasks.Add(addNewMessageToThreadTask);
			}

			return newEmails;
		}

		private async Task UpdateThreadMessagesAsync(Thread thread, IGrouping<ulong?, IMessageSummary> threadGroup)
		{
			var oldMessagesCount = thread.Messages.Count;
			var newMessagesCount = threadGroup.Count();

			var minCount = Math.Min(oldMessagesCount, newMessagesCount);
			var maxCount = Math.Max(oldMessagesCount, newMessagesCount);

			var messageSummaries = threadGroup.OrderBy(msgSumm => msgSumm.Date);

			var updateThreadTasks = new List<Task>(maxCount);

			StartUpdatingExistingThreadEmails(thread, minCount, messageSummaries, updateThreadTasks);

			// when getting new thread emails finishes the new emails will be available in the variable
			var newEmails = StartGettingNewThreadEmails(minCount, maxCount, messageSummaries, updateThreadTasks);

			await Task.WhenAll(updateThreadTasks);

			var orderedNewEmails = newEmails.OrderBy(m => m.DateTime);

			thread.Messages.AddRange(orderedNewEmails);
		}

		private async Task UpdateThreadAsync(Thread thread, IGrouping<ulong?, IMessageSummary> threadGroup)
		{
			await UpdateThreadMessagesAsync(thread, threadGroup);

			thread.IsRead = thread.Messages.Last().IsRead;
		}

		private async Task<Thread> CreateThreadAsync(ulong threadId, IGrouping<ulong?, IMessageSummary> threadGroup)
		{
			var createEmailTasks = new List<Task<Email>>();
			foreach (var msgSumm in threadGroup)
			{
				var createEmailTask = CreateEmailFromAsync(msgSumm);

				createEmailTasks.Add(createEmailTask);
			}
			var newThreadEmails = (await Task.WhenAll(createEmailTasks)).OrderBy(e => e.DateTime).ToList();

			var newThread = new Thread
			{
				Id = threadId,
				IsRead = newThreadEmails.Last().IsRead,
				Messages = newThreadEmails
			};

			return newThread;
		}
	}
}
