using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ServISData.Models
{
	public class ExcavatorProperty : IItem
	{
		public int Id { get; set; }

		[Required(AllowEmptyStrings = true, ErrorMessage = "Toto pole je povinné.")]
		public string Value { get; set; } = "";

		[Required, ValidateComplexType]
		public ExcavatorPropertyType PropertyType { get; set; } = null!;
	}
}
