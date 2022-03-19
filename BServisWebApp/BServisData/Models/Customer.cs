using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class Customer : User
	{
		[MaxLength(35)]
		public string Name { get; set; } = null!;

		[MaxLength(35)]
		public string Surname { get; set; } = null!;

		[MaxLength(17)]
		public string? PhoneNumber { get; set; }

		[MaxLength(254)]
		public string Email { get; set; } = null!;

		[MaxLength(50)]
		public string? Residence { get; set; }

		public bool IsTemporary { get; set; }
	}
}
