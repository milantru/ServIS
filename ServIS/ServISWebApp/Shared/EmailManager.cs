using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using Syncfusion.Blazor.Data;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ServISWebApp.Shared
{
	public class EmailManager
	{
		private readonly string emailPassword;
		private readonly MessageSummaryItems defaultMessageSummaryItems = MessageSummaryItems.UniqueId |
															MessageSummaryItems.GMailThreadId |
															MessageSummaryItems.PreviewText |
															MessageSummaryItems.InternalDate |
															MessageSummaryItems.Flags |
															MessageSummaryItems.Envelope;

		public string EmailAddress { get; private init; } = null!;

		public EmailManager(string emailAddress, string emailPassword)
		{
			EmailAddress = emailAddress;
			this.emailPassword = emailPassword;
		}

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

				threads.Add(thread);
			}
			threads = threads.OrderBy(t => t.Messages.Last().InternalDate).ToList();

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);

			return threads;
		}

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
					FromName = fromName,
					FromAddress = fromAddress,
					ToName = to.Name,
					ToAddress = to.Address,
					Subject = message.Subject,
					Text = message.TextBody,
					Read = emailRead,
					DateTime = message.Date.LocalDateTime
				};

				emails.Add(email);
			}

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);

			return emails;
		}

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
				FromName = fromName,
				FromAddress = fromAddress,
				ToName = to.Name,
				ToAddress = to.Address,
				Subject = message.Subject,
				Text = message.TextBody,
				Read = emailRead,
				DateTime = message.Date.LocalDateTime
			};

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);

			return email;
		}

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

		public async Task<IMessageSummary> GetMessageSummaryAsync(UniqueId uniqueId, MessageSummaryItems? items = null)
		{
			var itemsToPass = items ?? defaultMessageSummaryItems;
			var messageSummary = (await GetMessageSummariesAsync(new List<UniqueId> { uniqueId }, itemsToPass)).First();

			return messageSummary;
		}

		public async Task MarkEmailAsReadAsync(UniqueId uniqueId)
		{
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.AddFlagsAsync(uniqueId, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
		}

		public async Task MarkEmailAsReadAsync(IList<UniqueId> uniqueIds)
		{
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.AddFlagsAsync(uniqueIds, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
		}

		public async Task MarkEmailAsReadAsync(Email email)
		{
			await MarkEmailAsReadAsync(email.Uid);

			email.Read = true;
		}

		public async Task MarkEmailAsReadAsync(List<Email> emails)
		{
			var uids = emails.Select(e => e.Uid).ToList();
			await MarkEmailAsReadAsync(uids);

			emails.ForEach(e => e.Read = true);
		}

		public async Task MarkEmailAsUnreadAsync(UniqueId uniqueId)
		{
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.RemoveFlagsAsync(uniqueId, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
		}

		public async Task MarkEmailAsUnreadAsync(IList<UniqueId> uniqueIds)
		{
			using var imapClient = await GetConnectedImapClientAsync();

			var allMail = imapClient.GetFolder(SpecialFolder.All);
			await allMail.OpenAsync(FolderAccess.ReadWrite);

			await allMail.RemoveFlagsAsync(uniqueIds, MessageFlags.Seen, true);

			await allMail.CloseAsync();
			await imapClient.DisconnectAsync(true);
		}

		public async Task MarkEmailAsUnreadAsync(Email email)
		{
			await MarkEmailAsUnreadAsync(email.Uid);

			email.Read = false;
		}

		public async Task MarkEmailAsUnreadAsync(List<Email> emails)
		{
			var uids = emails.Select(e => e.Uid).ToList();
			await MarkEmailAsUnreadAsync(uids);

			emails.ForEach(e => e.Read = false);
		}

		public async Task Search()
		{
			await Task.CompletedTask;
			throw new NotImplementedException();
		}

		public async Task SendEmailAsync(Email email)
		{
			var message = PrepareMessage(email);

			await SendMessageAsync(message);
		}

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
				Read = true,
				DateTime = reply.Date.DateTime
			};

			return replyEmail;
		}

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
			catch (SslHandshakeException)
			{
				Console.WriteLine($"Recovering from '{nameof(SslHandshakeException)}', trying to connect anyway.");
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
			catch (SslHandshakeException)
			{
				Console.WriteLine($"Recovering from '{nameof(SslHandshakeException)}', trying to connect anyway.");
				smtpClient.CheckCertificateRevocation = false;

				await ConnectSmtpAsync(smtpClient);
			}

			smtpClient.Authenticate(
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

		private async Task SendMessageAsync(MimeMessage message)
		{
			using var smtpClient = await GetConnectedSmtpClientAsync();

			await smtpClient.SendAsync(message);

			await smtpClient.DisconnectAsync(true);
		}
	}
}
