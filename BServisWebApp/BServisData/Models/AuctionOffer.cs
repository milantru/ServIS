using BServisData.Interfaces;

namespace BServisData.Models
{
	public class AuctionOffer : IItem
	{
		public int Id { get; set; }

		public Excavator Excavator { get; set; } = null!;

		public string Description { get; set; } = null!;

		public DateTime OfferEnd { get; set; }

		public int StartingBid { get; set; }
	}
}
