using BServisData.Interfaces;

namespace BServisData.Models
{
	public class AdditionalEquipmentPhoto : IItem
	{
		public int Id { get; set; }

		public AdditionalEquipment AdditionalEquipment { get; set; } = null!;

		public byte[] Photo { get; set; } = null!;

		public bool IsTitle { get; set; }
	}
}
