using ServISData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Models
{
	public class MainOffer : IItem
	{
		public int Id { get; set; }

		[Required, ValidateComplexType]
		public ExcavatorType ExcavatorType { get; set; } = null!;

		[Required] // TODO: maybe better attributes
		public byte[] Photo { get; set; } = null!;

		[Required(AllowEmptyStrings = true, ErrorMessage = "Toto pole je povinné.")]
		public string Description { get; set; } = "";
	}
}
