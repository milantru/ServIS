using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AdditionalEquipment : IItem
	{
		public int Id { get; set; }

		[Required, ValidateComplexType]
		public ExcavatorCategory ExcavatorCategory { get; set; } = null!;

		[Required, ValidateComplexType]
		public AdditionalEquipmentCategory Category { get; set; } = null!;

		[Required, ValidateComplexType]
		public AdditionalEquipmentBrand Brand { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		[Required(AllowEmptyStrings = true), MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = "";

		// TODO: Custom attr (NotNullNorEmpty)
		public IList<AdditionalEquipmentPhoto> Photos { get; set; } = null!;
	}
}
