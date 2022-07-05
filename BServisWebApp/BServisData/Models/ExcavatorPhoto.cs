using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BServisData.Models
{
	public class ExcavatorPhoto : IItem
	{
		public int Id { get; set; }

		[Required]
		public Excavator Excavator { get; set; } = null!;

		[Required(ErrorMessage = "Nutné vložiť fotku.")]
		[Column(TypeName = "varbinary(50000)")]
		[MaxLength(50000, ErrorMessage = "Fotka je príliš veľká (max 50 kB).")]
		public byte[] Photo { get; set; } = null!;

		[Required]
		public bool IsTitle { get; set; }
	}
}
