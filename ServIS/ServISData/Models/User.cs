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
	}
}
