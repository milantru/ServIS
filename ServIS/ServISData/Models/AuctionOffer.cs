using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AuctionOffer : IItem
	{
		public int Id { get; set; }

		[Required, ValidateComplexType]
		public Excavator Excavator { get; set; } = null!;

		[Required(AllowEmptyStrings = true), MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = "";

		public DateTime OfferEnd { get; set; }

		[Column(TypeName = "decimal(11,2)")]
		public decimal StartingBid { get; set; }
	}
}
