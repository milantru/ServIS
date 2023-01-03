using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace ServISWebApp.Shared
{
	public class EmailSender
	{
		private readonly MailAddress toMailAddress = null!;
		private readonly SmtpClient smtpClient = null!;

		public Action<object, AsyncCompletedEventArgs>? OnSuccessfulSend { get; set; }
		public Action<object, AsyncCompletedEventArgs>? OnError { get; set; }

		public EmailSender(
			string toAddress,
			Action<object, AsyncCompletedEventArgs>? onSuccessfulSend = null,
			Action<object, AsyncCompletedEventArgs>? onError = null
		)
		{
			toMailAddress = new MailAddress(toAddress);
			OnSuccessfulSend = onSuccessfulSend;
			OnError = onError;

			ConfigureSmtpClient(toAddress, out smtpClient);
		}

		private void OnSend(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				if (OnError != null)
				{
					OnError(sender, e);
				}
			}
			else
			{
				if (OnSuccessfulSend != null)
				{
					OnSuccessfulSend(sender, e);
				}
			}
		}

		private void ConfigureSmtpClient(string toAddress, out SmtpClient smtpClient)
		{
			// out in parameters is used to silence compiler
			// (readonly is used and compiler thinks we are reassigning the variable,
			// but this method is called from the constructor so it is fine really)
			IConfiguration config = new ConfigurationBuilder()
				.AddUserSecrets("caf838ad-46c6-4cb3-a8b9-b71aee4b2426")
				.Build();

			var password = config["EmailAppPassword"];

			smtpClient = new SmtpClient()
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential()
				{
					UserName = toAddress,
					Password = password
				}
			};
			smtpClient.SendCompleted += OnSend;
		}

		public async Task SendAsync(string fromAddress, string message, string subject = "Message from ServIS")
		{
			var fromMailAddress = new MailAddress(fromAddress);

			var mailMessage = new MailMessage()
			{
				From = fromMailAddress,
				To = { toMailAddress },
				ReplyToList = { fromMailAddress },
				Subject = subject,
				Body = message
			};

			await smtpClient.SendMailAsync(mailMessage);
		}
	}
}
