using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class AdditionalEquipment : IItem
	{
		public int Id { get; set; }

		[MaxLength(40)]
		public string ForWhichExcavatorCategory { get; set; } = null!;

		[MaxLength(30)]
		public string Category { get; set; } = null!;

		[MaxLength(30)]
		public string Brand { get; set; } = null!;

		[MaxLength(80)]
		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public int Price { get; set; }
	}
}
