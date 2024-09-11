using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Attributes
{
	internal class PhoneOptionalAttribute : DataTypeAttribute
	{
        private readonly PhoneAttribute phoneAttribute = new();

        public PhoneOptionalAttribute() : base(DataType.PhoneNumber)
        {

        }

		public override bool IsValid(object? value)
		{
			var phoneNumber = value as string;

			if (string.IsNullOrEmpty(phoneNumber))
			{
				return true;
			}

			return phoneAttribute.IsValid(value);
		}
	}
}
