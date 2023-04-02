namespace ServISData.Attributes
{
	internal class InputTypeLabelAttribute : Attribute
	{
		public string Label { get; private set; }

		public InputTypeLabelAttribute(string label)
		{
			Label = label;
		}
	}
}
