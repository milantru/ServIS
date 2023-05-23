using MailKit;

namespace ServISWebApp.Shared
{
    /// <summary>
    /// Represents a thread containing a list of message summaries.
    /// </summary>
    public class Thread
	{
        /// <summary>
        /// Gets or sets the unique identifier of the thread.
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// Gets or sets the list of message summaries contained by the thread.
        /// </summary>
        public List<IMessageSummary> Messages { get; set; } = null!;
	}
}
