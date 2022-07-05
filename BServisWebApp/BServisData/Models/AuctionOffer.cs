using BServisData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BServisData.Models
{
	public class AuctionOffer : IItem
	{
		public int Id { get; set; }

		[Required]
		public Excavator Excavator { get; set; } = null!;

		[MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public DateTime OfferEnd { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), Column(TypeName = "decimal(11,2)")]
		public decimal StartingBid { get; set; }
	}
}
