using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ServISData.Models
{
	public class MainOffer : IItem
	{
		public int Id { get; set; }

		[Required, ValidateComplexType]
		public ExcavatorType ExcavatorType { get; set; } = null!;

		[Required(ErrorMessage = "Nebola nahraná žiadna fotka.")]
		public byte[] Photo { get; set; } = null!;

		[Required(AllowEmptyStrings = true, ErrorMessage = "Toto pole je povinné.")]
		public string Description { get; set; } = "";
	}
}
