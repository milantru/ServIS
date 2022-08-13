using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class ExcavatorPhoto : IItem, IPhoto
	{
		public int Id { get; set; }

		[Required, ValidateComplexType]
		public Excavator Excavator { get; set; } = null!;

		[Required] // TODO: maybe better attributes
		public byte[] Photo { get; set; } = null!;

		public bool IsTitle { get; set; }
	}
}
