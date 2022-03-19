using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class SparePart : IItem
	{
		public int Id { get; set; }

		public int CatalogNumber { get; set; }

		[MaxLength(80)]
		public string Name { get; set; } = null!;

		public List<Excavator> Excavators { get; set; } = null!;
	}
}
