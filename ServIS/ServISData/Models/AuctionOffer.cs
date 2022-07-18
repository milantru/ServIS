using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AuctionOffer : IItem
	{
		public int Id { get; set; }

		[ValidateComplexType, Required]
		public Excavator Excavator { get; set; } = null!;

		[Required(AllowEmptyStrings = true), MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = "";

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public DateTime OfferEnd { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), Column(TypeName = "decimal(11,2)")]
		public decimal StartingBid { get; set; }
	}
}
