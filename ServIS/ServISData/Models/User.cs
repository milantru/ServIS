using ServISData.Attributes;
using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ServISData.Models
{
	public class User : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Username { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), MaxLength(ErrorMessage = "Príliš dlhé heslo.")]
		public string Password { get; set; } = null!;

		[Required]
		public string Role { get; set; } = "User";

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(35, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(35, ErrorMessage = "Max {1} znakov.")]
		public string Surname { get; set; } = null!;

		[PhoneOptional(ErrorMessage = "Telefónne číslo nie je validné.")]
		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), EmailAddress(ErrorMessage = "Email nie je validný.")]
		public string Email { get; set; } = null!;

		[StringLength(50, ErrorMessage = "Max {1} znakov.")]
		public string? Residence { get; set; }

		public bool IsTemporary { get; set; }
	}
}
