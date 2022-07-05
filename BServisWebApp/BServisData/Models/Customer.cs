using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class Customer : User
	{
		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(35, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(35, ErrorMessage = "Max {1} znakov.")]
		public string Surname { get; set; } = null!;

		[Phone(ErrorMessage = "Telefónne číslo nie je validné.")]
		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), EmailAddress(ErrorMessage = "Email nie je validný.")]
		public string Email { get; set; } = null!;

		[StringLength(50, ErrorMessage = "Max {1} znakov.")]
		public string? Residence { get; set; }

		[Required]
		public bool IsTemporary { get; set; }
	}
}
