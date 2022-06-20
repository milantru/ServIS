using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class Excavator : IItem
	{
		public int Id { get; set; }

		[Required, MaxLength(40)]
		public string Category { get; set; } = null!;

		[Required, MaxLength(30)]
		public string Brand { get; set; } = null!;

		[Required, MaxLength(30)]
		public string Model { get; set; } = null!;

		[Required, MaxLength(80)]
		public string Name { get; set; } = null!;

		[Required]
		public string Description { get; set; } = null!;

		public DateTime? LastInspection { get; set; }

		[Required]
		public bool IsNew { get; set; }

		public List<SparePart> SpareParts { get; set; } = null!;
	}
}
