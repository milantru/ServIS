using MailKit;
using MimeKit;

namespace ServISWebApp.Shared
{
	public class Email
	{
		public UniqueId Uid { get; set; }
		public string MessageId { get; set; } = null!;
		public MessageIdList References { get; set; } = null!;
		public List<Header>? Headers { get; set; } = null!;
		public string FromName { get; set; } = null!;
		public string FromAddress { get; set; } = null!;
		public string ToName { get; set; } = null!;
		public string ToAddress { get; set; } = null!;
		public string Subject { get; set; } = null!;
		public string Text { get; set; } = null!;
		public bool Read { get; set; }
		public DateTime DateTime { get; set; }
	}
}
