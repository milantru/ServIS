using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class ExcavatorPhoto : IItem
	{
		public int Id { get; set; }

		[ValidateComplexType, Required]
		public Excavator Excavator { get; set; } = null!;

		[Column(TypeName = "varbinary(50000)")]
		public byte[] Photo { get; set; } = null!;

		[Required]
		public bool IsTitle { get; set; }
	}
}
