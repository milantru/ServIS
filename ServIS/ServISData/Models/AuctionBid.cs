using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AuctionBid : IItem
	{
		public int Id { get; set; }

		[ValidateComplexType, Required]
		public User User { get; set; } = null!;

		[ValidateComplexType, Required]
		public AuctionOffer AuctionOffer { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), Column(TypeName = "decimal(11,2)")]
		public decimal Bid { get; set; }
	}
}
