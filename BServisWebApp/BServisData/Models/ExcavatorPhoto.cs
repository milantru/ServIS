using BServisData.Interfaces;

namespace BServisData.Models
{
	public class ExcavatorPhoto : IItem
	{
		public int Id { get; set; }

		public Excavator Excavator { get; set; } = null!;

		public byte[] Photo { get; set; } = null!;

		public bool IsTitle { get; set; }
	}
}
