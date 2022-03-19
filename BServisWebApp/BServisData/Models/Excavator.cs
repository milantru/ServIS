using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class Excavator : IItem
	{
		public int Id { get; set; }

		public ExcavatorCategory ExcavatorCategory { get; set; }

		[MaxLength(30)]
		public string Brand { get; set; } = null!;

		[MaxLength(30)]
		public string Model { get; set; } = null!;

		[MaxLength(80)]
		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public DateOnly? LastInspection { get; set; }

		public bool IsNew { get; set; }

		public List<SparePart> SpareParts { get; set; } = null!;
	}
}
