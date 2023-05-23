using MailKit;
using MimeKit;

namespace ServISWebApp.Shared
{
    /// <summary>
    /// Represents an email message.
    /// </summary>
    public class Email
	{
        /// <summary>
        /// Gets or sets the unique identifier of the email.
        /// </summary>
        /// <para>
        /// <remarks>
        /// Represents a unique identifier for messages in a <see cref="IMailFolder"/>.
        /// </remarks>
        /// </para>
        public UniqueId Uid { get; set; }

        /// <summary>
        /// Gets or sets the message ID of the email.
        /// </summary>
        /// <para>
        /// <remarks>
        /// It is used for replying to the email.
        /// </remarks>
        /// </para>
        public string MessageId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of message IDs.
        /// </summary>
        /// <para>
        /// <remarks>
        /// It is used for replying to the email.
        /// </remarks>
        /// </para>
        public MessageIdList References { get; set; } = null!;

        /// <summary>
        /// Gets or sets the headers of the email.
        /// </summary>
        public List<Header>? Headers { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the sender of the email.
        /// </summary>
        public string FromName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address of the sender.
        /// </summary>
        public string FromAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the recipient of the email.
        /// </summary>
        public string ToName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address of the recipient.
        /// </summary>
        public string ToAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text body of the email.
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the email has been read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the email.
        /// </summary>
        public DateTime DateTime { get; set; }
	}
}
