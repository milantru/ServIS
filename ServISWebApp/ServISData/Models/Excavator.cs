using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ServISData.Models
{
	public class Excavator : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(40, ErrorMessage = "Max {1} znakov.")]
		public string Category { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Brand { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Model { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		[MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = null!;

		[Required]
		public bool IsNew { get; set; }

		public DateTime? LastInspection { get; set; }

		public List<SparePart> SpareParts { get; set; } = null!;
	}
}
