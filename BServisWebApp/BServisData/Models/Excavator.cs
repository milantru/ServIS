using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class Excavator : IItem
	{
		public int Id { get; set; }

		[MaxLength(40)]
		public string Category { get; set; } = null!;

		[MaxLength(30)]
		public string Brand { get; set; } = null!;

		[MaxLength(30)]
		public string Model { get; set; } = null!;

		[MaxLength(80)]
		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public DateTime? LastInspection { get; set; }

		public bool IsNew { get; set; }

		public List<SparePart> SpareParts { get; set; } = null!;
	}
}
