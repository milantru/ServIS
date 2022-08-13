using ServISData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Models
{
	public class ExcavatorProperty : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public string Value { get; set; } = null!;

		[Required, ValidateComplexType]
		public ExcavatorPropertyType PropertyType { get; set; } = null!;

		//[Required]
		//public Excavator Excavator { get; set; } = null!;
	}
}
