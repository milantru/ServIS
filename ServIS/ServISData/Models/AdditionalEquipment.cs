using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AdditionalEquipment : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(40, ErrorMessage ="Max {1} znakov.")]
		public string ExcavatorCategory { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Category { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Brand { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		[Required(AllowEmptyStrings = true), MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = "";

		// TODO: Custom attr (NotNullNorEmpty)
		public IList<AdditionalEquipmentPhoto> Photos { get; set; } = null!;
	}
}
