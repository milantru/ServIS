using ServISData.Attributes;

namespace ServISData
{
    /// <summary>
    /// Provides extension methods for the <see cref="InputType"/> enum.
    /// </summary>
    public static class InputTypeExtensions
	{
        /// <summary>
        /// Retrieves the label associated with the specified <see cref="InputType"/> value.
        /// </summary>
        /// <param name="inputType">The <see cref="InputType"/> value.</param>
        /// <returns>The label of the <see cref="InputType"/> value.</returns>
        public static string GetLabel(this InputType inputType)
		{
			var enumType = inputType.GetType();
			var inputTypeEnumName = Enum.GetName(enumType, inputType)!;

			var label = enumType
				.GetField(inputTypeEnumName)!
				.GetCustomAttributes(inherit: false)
				.OfType<InputTypeLabelAttribute>()
				.Single()
				.Label;

			return label;
		}
	}
}
