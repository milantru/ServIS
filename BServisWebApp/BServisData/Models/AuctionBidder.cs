using BServisData.Interfaces;

namespace BServisData.Models
{
	public class AuctionBidder : IItem
	{
		public int Id { get; set; }

		public User User { get; set; } = null!;

		public AuctionOffer AuctionOffer { get; set; } = null!;

		public int Bid { get; set; }
	}
}
