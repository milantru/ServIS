using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AdditionalEquipmentPhoto : IItem
	{
		public int Id { get; set; }

		[ValidateComplexType, Required]
		public AdditionalEquipment AdditionalEquipment { get; set; } = null!;

		[Column(TypeName = "varbinary(50000)")]
		public byte[] Photo { get; set; } = null!;

		[Required]
		public bool IsTitle { get; set; }
	}
}
