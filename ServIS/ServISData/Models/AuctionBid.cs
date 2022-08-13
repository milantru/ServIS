using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServISData.Models
{
	public class AuctionBid : IItem
	{
		public int Id { get; set; }

		[Required, ValidateComplexType]
		public User User { get; set; } = null!;

		[Required, ValidateComplexType]
		public AuctionOffer AuctionOffer { get; set; } = null!;

		[Column(TypeName = "decimal(11,2)")]
		public decimal Bid { get; set; }
	}
}
