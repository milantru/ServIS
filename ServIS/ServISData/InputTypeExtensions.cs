using ServISData.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData
{
	public static class InputTypeExtensions
	{
		public static string GetLabel(this InputType inputType)
		{
			var enumType = inputType.GetType();
			var inputTypeEnumName = Enum.GetName(enumType, inputType);
			if (inputTypeEnumName == null)
			{
				throw new Exception($"Enum '{nameof(InputType)}' does NOT contain constant '{inputType}'");
			}

			var label = enumType
				.GetField(inputTypeEnumName)
				?.GetCustomAttributes(false)
				.OfType<InputTypeLabelAttribute>()
				.Single()
				.Label;
			if (label == null)
			{
				throw new Exception($"'{nameof(InputType)}' does NOT contain field '{inputTypeEnumName}'");
			}

			return label;
		}
	}
}
