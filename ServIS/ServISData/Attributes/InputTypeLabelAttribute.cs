using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
