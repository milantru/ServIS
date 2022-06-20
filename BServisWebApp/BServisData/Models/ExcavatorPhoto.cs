using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace BServisData.Models
{
	public class ExcavatorPhoto : IItem
	{
		public int Id { get; set; }

		public Excavator Excavator { get; set; } = null!;

		[Column(TypeName = "varbinary(50000)")]
		public byte[] Photo { get; set; } = null!;

		public bool IsTitle { get; set; }
	}
}
