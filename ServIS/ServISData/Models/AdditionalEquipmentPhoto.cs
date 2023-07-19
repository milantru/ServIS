using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AdditionalEquipmentPhoto : IItem, IPhoto
	{
		public int Id { get; set; }

		[Required, ValidateComplexType]
		public AdditionalEquipment AdditionalEquipment { get; set; } = null!;

		[Required]
		public byte[] Photo { get; set; } = null!;

		public bool IsTitle { get; set; }
	}
}
