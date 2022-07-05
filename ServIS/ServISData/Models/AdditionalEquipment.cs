using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AdditionalEquipment : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(40, ErrorMessage ="Max {1} znakov.")]
		public string ForWhichExcavatorCategory { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Category { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Brand { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		[MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), Column(TypeName = "decimal(11,2)")]
		public decimal Price { get; set; }
	}
}
