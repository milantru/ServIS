using ServISData.Attributes;

namespace ServISData
{
	public enum InputType
	{
		[InputTypeLabel("Nenastavené")]
		Unset, // let Unset be the first one (some parts of the code, e.g. foreach in ExcavatorForm, counts on it
		[InputTypeLabel("Číslo")]
		Number,
		[InputTypeLabel("Text")]
		Text,
		[InputTypeLabel("Veľký text")]
		TextArea,
		[InputTypeLabel("Dátum")]
		Date
	}
}
