using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class SparePart : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int CatalogNumber { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		public List<Excavator> Excavators { get; set; } = null!;
	}
}
