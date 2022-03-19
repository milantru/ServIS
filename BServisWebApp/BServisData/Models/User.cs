using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class User : IItem
	{
		public int Id { get; set; }

		[MaxLength(30)]
		public string Username { get; set; } = null!;

		[MaxLength(80)]
		public string Password { get; set; } = null!;
	}
}
