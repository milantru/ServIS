using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace BServisData.Models
{
	public class AdditionalEquipmentPhoto : IItem
	{
		public int Id { get; set; }

		public AdditionalEquipment AdditionalEquipment { get; set; } = null!;

		[Column(TypeName = "varbinary(50000)")]
		public byte[] Photo { get; set; } = null!;

		public bool IsTitle { get; set; }
	}
}
