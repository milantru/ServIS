using ServISData.Attributes;
using ServISData.Models;

namespace ServISData
{
    /// <summary>
    /// Represents the input types for <see cref="ExcavatorPropertyType"/>.
    /// </summary>
    public enum InputType
	{
        /// <summary>
        /// Indicates that the input type is not set.
        /// </summary>
        [InputTypeLabel("Nenastavené")]
		Unset, // let Unset be the first one (some parts of the code, e.g. foreach in ExcavatorForm, are counting on it)

        /// <summary>
        /// Indicates a numeric input type.
        /// </summary>
        [InputTypeLabel("Číslo")]
		Number,

        /// <summary>
        /// Indicates a text input type.
        /// </summary>
        [InputTypeLabel("Text")]
		Text,

        /// <summary>
        /// Indicates a large text input type.
        /// </summary>
        [InputTypeLabel("Veľký text")]
		TextArea,

        /// <summary>
        /// Indicates a date input type.
        /// </summary>
        [InputTypeLabel("Dátum")]
		Date
	}
}
