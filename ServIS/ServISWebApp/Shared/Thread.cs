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
		/// Gets or sets a value indicating whether the thread has been read.
		/// </summary>
		public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets the list of emails contained by the thread.
        /// </summary>
        public List<Email> Messages { get; set; } = null!;
	}
}
