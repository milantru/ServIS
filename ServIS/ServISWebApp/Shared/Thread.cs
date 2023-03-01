using MailKit;

namespace ServISWebApp.Shared
{
	public class Thread
	{
		public ulong Id { get; set; }
		public List<IMessageSummary> Messages { get; set; } = null!;
	}
}
