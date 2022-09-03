using ServISData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Models
{
	public class ExcavatorPropertyType : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public string Name { get; set; } = null!;

		// TODO: Custom attribute NotUnset
		public InputType InputType { get; set; } = InputType.Unset;

		public ICollection<ExcavatorType> ExcavatorTypesWithThisProperty { get; set; } = null!;

		//public IList<ExcavatorProperty> ExcavatorPropertiesOfThisType { get; set; } = null!;
	}
}
