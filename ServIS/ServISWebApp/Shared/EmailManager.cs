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
        /// Retrieves the sender name and address from the provided <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="senderName">The sender name.</param>
        /// <param name="senderAddress">The sender address.</param>
        public void GetSender(MimeMessage message, out string senderName, out string senderAddress)
		{
			/* If user sent mail using this web app then his/her email address is stored
			 * in `ReplyTo` (because for some reason even though we set it to `From`,
			 * it is overriden). But if mail was sent from some email client (e.g. gmail),
			 * then users email is stored in `From`. */
			senderAddress = message.From.Mailboxes.First().Address;
			senderName = message.From.Mailboxes.First().Name;
			if (senderAddress == EmailAddress && message.ReplyTo.Count > 0)
			{
				/* sent from this web app;
				 * second condition added for covering case when admin 
				 * sends email directly from email client (e.g. gmail) */
				senderAddress = message.ReplyTo.Mailboxes.First().Address;
				senderName = message.ReplyTo.Mailboxes.First().Name;
			}
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

				var thread = new Thread()
				{
					Id = threadId,
					Messages = group.OrderBy(m => m.InternalDate).ToList()
				};

				if (ShouldSkip(thread))
				{
					continue;
				}

				threads.Add(thread);
			}
			threads = threads.OrderBy(t => t.Messages.Last().InternalDate).ToList();

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

				GetSender(message, out string fromName, out string fromAddress);
				var to = message.To.Mailboxes.First();
				
				var email = new Email
				{
					Uid = uid,
					MessageId = message.MessageId,
					References = message.References,
					Headers = message.Headers.ToList(),
					FromName = fromName,
					FromAddress = fromAddress,
					ToName = to.Name,
					ToAddress = to.Address,
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

			GetSender(message, out string fromName, out string fromAddress);
			var to = message.To.Mailboxes.First();

			var email = new Email
			{
				Uid = uniqueId,
				MessageId = message.MessageId,
				References = message.References,
				Headers = message.Headers.ToList(),
				FromName = fromName,
				FromAddress = fromAddress,
				ToName = to.Name,
				ToAddress = to.Address,
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

		//public async Task SearchAsync()
		//{
		//	await Task.CompletedTask;
		//	throw new NotImplementedException();
		//}

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
        /// <param name="email">The email to reply to.</param>
        /// <param name="replyText">The text of the reply.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the replied email.</returns>
        public async Task<Email> ReplyToAsync(Email email, string replyText)
		{
			var reply = PrepareReply(email, replyText);

			await SendMessageAsync(reply);

			var replyEmail = new Email
			{
				MessageId = reply.MessageId,
				References = reply.References,
				FromName = email.ToName,
				FromAddress = email.ToAddress,
				ToName = email.FromName,
				ToAddress = email.FromAddress,
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

        /// <summary>
        /// Determines whether should skip the thread. If the thread contains 
        /// only one autogenerated message (e.g. notifying auction winner about victory (it is not meant for admin), 
        /// then this method deletes the email and decides the thread is skippable.
        /// </summary>
        /// <returns><c>true</c> if thread contains only one autogenerated message 
        /// (i.e. is not meant for admin); <c>false</c> otherwise</returns>
        private bool ShouldSkip(Thread thread)
		{
			var messages = thread.Messages;
			var firstMessage = messages.First();
			var isSentFromApp = firstMessage.Envelope.From.Mailboxes.First().Address == EmailAddress;
			// autogenerated messages are sent to the users e.g. from auction (to notify them if they won or lost)
			var isAutogenerated = firstMessage.Headers.FirstOrDefault(h => h.Field == "X-ServIS-autogenerated")?.Value == "true";

			if (isSentFromApp && isAutogenerated && messages.Count == 1)
			{
				/* We skip this thread because this was message meant just for the user, 
				 * but for some reason it is sent to both user and admin. Because we don't 
				 * want to bother admin with this message (which is meant only for user anyway),
				 * this if was created to prevent showing this thread/message to admin. 
				 * We also delete it so it won't take up space in gmail. 
				 * However, if for some reason user answered this message (the message says 
				 * it doesn't need reply, we might want to be able to see it, that's why 
				 * the condition messages.Count == 1 is present. */
				_ = DeleteEmailAsync(firstMessage.UniqueId);
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
			message.ReplyTo.Add(new MailboxAddress(email.FromName, email.FromAddress));

			email.Headers?.ForEach(header => message.Headers.Add(header));

			message.Subject = email.Subject;
			message.Body = new TextPart() { Text = email.Text };

			return message;
		}

		//private void CalculateFromToEmailIndexes(int? skip, int? take, int maxRange, out int fromIndex, out int toIndex)
		//{
		//	if (!skip.HasValue)
		//	{
		//		skip = 0;
		//	}
		//	if (!take.HasValue)
		//	{
		//		take = maxRange;
		//	}

		//	fromIndex = skip.Value * take.Value;
		//	toIndex = fromIndex + take.Value;
		//}

		private IEnumerable<bool> _GetEmailsReadStatuses(IList<IMessageSummary> emailsSummaries)
		{
			var emailsReadStatuses = emailsSummaries.Select(es => es.Flags!.Value.HasFlag(MessageFlags.Seen)); // read or not

			return emailsReadStatuses;
		}

		private async Task<IEnumerable<bool>> GetEmailsReadStatusesAsync(IMailFolder folder, IList<UniqueId> uniqueIds)
		{
			var emailsSummaries = await folder.FetchAsync(uniqueIds, MessageSummaryItems.Flags);
			var emailsReadStatuses = _GetEmailsReadStatuses(emailsSummaries);

			return emailsReadStatuses;
		}

		private async Task<IEnumerable<bool>> GetEmailsReadStatusesAsync(IMailFolder folder, int fromIndex, int toIndex)
		{
			var emailsSummaries = await folder.FetchAsync(fromIndex, toIndex, MessageSummaryItems.Flags);
			var emailsReadStatuses = _GetEmailsReadStatuses(emailsSummaries);

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

		private MimeMessage PrepareReply(Email email, string replyText)
		{
			var reply = new MimeMessage();

			reply.From.Add(new MailboxAddress(email.ToName, email.ToAddress));
			reply.To.Add(new MailboxAddress(email.FromName, email.FromAddress));
			reply.ReplyTo.Add(new MailboxAddress(email.ToName, email.ToAddress));
			reply.Subject = email.Subject.StartsWith("Re:") ? email.Subject : "Re:" + email.Subject;
			reply.InReplyTo = email.MessageId;
			reply.References.AddRange(email.References);
			reply.References.Add(email.MessageId);

			reply.Body = PreparePreviousMessagesQuotation(email, replyText);

			return reply;
		}

		private string PrepareErrorMessageForFailedEmailSending(MimeMessage message)
		{
			GetSender(message, out _, out var fromEmail);

			/* message.To.Mailboxes should always be nonempty, but FirstOrDefault is called on it
			 * due to defensive programming, if we see in log that this place is empty, 
			 * we know this is the problem. But once again... it should never happen. 
			 * The only reason it is here instead of First is defensive programming; better  
			 * to have some log message with empty spot than no log message at all. */
			var errMsg = $"Failed to send email from {fromEmail} to {message.To.Mailboxes.FirstOrDefault()?.Address}.\n" +
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
