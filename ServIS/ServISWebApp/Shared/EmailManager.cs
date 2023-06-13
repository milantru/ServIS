using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using Syncfusion.Blazor.Data;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ServISWebApp.Shared
{
    /// <summary>
    /// Provides functionality for managing and processing emails.
    /// </summary>
    public class EmailManager
	{
		private readonly ILogger<EmailManager> logger;
		private readonly string emailPassword;
		private readonly MessageSummaryItems defaultMessageSummaryItems = MessageSummaryItems.UniqueId |
															MessageSummaryItems.GMailThreadId |
															MessageSummaryItems.PreviewText |
															MessageSummaryItems.InternalDate |
															MessageSummaryItems.Flags |
															MessageSummaryItems.Envelope |
															MessageSummaryItems.Headers;

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
        /// Asynchronously retrieves the list of threads.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the list of threads.</returns>
        public async Task<List<Thread>> GetThreadsAsync()
		{
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var gmailMessages = await allMail.FetchAsync(0, -1, defaultMessageSummaryItems);

			var groups = gmailMessages.GroupBy(g => g.GMailThreadId);

			var threads = new List<Thread>();
			foreach (var group in groups)
			{
				var threadId = group.Key!.Value;

				var threadEmails = new List<Email>();
				foreach (var msgSumm in group)
				{
					var email = await CreateEmailFrom(msgSumm);

					threadEmails.Add(email);
				}
				threadEmails = threadEmails.OrderBy(e => e.DateTime).ToList();

				var thread = new Thread()
				{
					Id = threadId,
					IsRead = threadEmails.Last().IsRead,
					Messages = threadEmails
				};

				var shouldSkipThread = await ShouldSkip(thread);
				if (!shouldSkipThread)
				{
					threads.Add(thread);
				}
			}

			threads = threads.OrderBy(t => t.Messages.Last().DateTime).ToList();
			
			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);

			return threads;
		}

        /// <summary>
        /// Asynchronously retrieves the list of emails associated with the <paramref name="uniqueIds"/>.
        /// </summary>
        /// <param name="uniqueIds">The unique identifiers of the emails.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the list of emails associated with the provided unique identifiers.</returns>
        public async Task<List<Email>> GetEmailsAsync(IList<UniqueId> uniqueIds)
		{
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
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var emailRead = (await GetEmailsReadStatusesAsync(allMail, new List<UniqueId> { uniqueId })).First();

			var message = await allMail.GetMessageAsync(uniqueId);

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

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);

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
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var messageSummaries = await allMail.FetchAsync(uniqueIds, items ?? defaultMessageSummaryItems);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);

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
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.AddFlagsAsync(uniqueId, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
		}

        /// <summary>
        /// Asynchronously marks emails as read.
        /// </summary>
        /// <param name="uniqueIds">The unique identifiers of the emails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsReadAsync(IList<UniqueId> uniqueIds)
		{
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.AddFlagsAsync(uniqueIds, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
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
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.RemoveFlagsAsync(uniqueId, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
		}

        /// <summary>
        /// Asynchronously marks emails as unread.
        /// </summary>
        /// <param name="uniqueIds">The unique identifiers of the emails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MarkEmailAsUnreadAsync(IList<UniqueId> uniqueIds)
		{
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.RemoveFlagsAsync(uniqueIds, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
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
		}

        /// <summary>
        /// Asynchronously deletes emails.
        /// </summary>
        /// <param name="uniqueIds">The unique identifier of the emails.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteEmailAsync(IList<UniqueId> uniqueIds)
		{
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
		}
		
		private async Task<Email> CreateEmailFrom(IMessageSummary messageSummary)
		{
			var uid = messageSummary.UniqueId;

			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadOnly);

			var message = await allMail.GetMessageAsync(uid);

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

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);

			return email;
		}

		/// <summary>
		/// Determines whether should skip the thread. If the thread contains 
		/// only one autogenerated message (e.g. notifying auction winner about victory (it is not meant for admin), 
		/// then this method deletes the email and decides the thread is skippable.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation. The task result contains 
		/// <c>true</c> if the thread contains only one autogenerated message 
		/// (i.e. is not meant for the admin); <c>false</c> otherwise</returns>
		private async Task<bool> ShouldSkip(Thread thread)
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
		}
	}
}
