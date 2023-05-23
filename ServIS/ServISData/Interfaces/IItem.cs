namespace ServISData.Interfaces
{
    /// <summary>
    /// Represents an item with an ID used to represent an entity in a database.
    /// </summary>
    public interface IItem
	{
        /// <summary>
        /// Gets or sets the unique identifier of the item.
        /// </summary>
        public int Id { get; set; }
	}
}
