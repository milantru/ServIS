using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ServISData.Models
{
	public class SparePart : IItem
	{
		public int Id { get; set; }

		public int CatalogNumber { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		public IList<Excavator> Excavators { get; set; } = null!;
	}
}
