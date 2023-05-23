namespace ServISData.Attributes
{
    /// <summary>
    /// Represents an attribute used to provide a label for an <see cref="InputType" />.
    /// </summary>
    internal class InputTypeLabelAttribute : Attribute
	{
        /// <summary>
        /// Gets the label associated with the <see cref="InputType" />.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputTypeLabelAttribute"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label associated with the <see cref="InputType" />.</param>
        public InputTypeLabelAttribute(string label)
		{
			Label = label;
		}
	}
}
