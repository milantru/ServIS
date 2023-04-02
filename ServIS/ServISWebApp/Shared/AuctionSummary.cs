using ServISData.Models;

namespace ServISWebApp.Shared
{
	public class AuctionSummary
	{
		public AuctionOffer AuctionOffer { get; }
		public AuctionBid? Bid { get; }
        public AuctionBid? WinningBid { get; }

		/// <summary>
		/// This constructor is assumed to be used when notifying admin auction has ended without participants.
		/// </summary>
        public AuctionSummary(AuctionOffer auctionOffer)
        {
			AuctionOffer = auctionOffer;
        }

		/// <summary>
		/// This constructor is assumed to be used when notifying auction winner he/she has won the auction.
		/// Or when notifying admin about the end of the auction (the winner exists).
		/// </summary>
		public AuctionSummary(AuctionOffer auctionOffer, AuctionBid winningBid)
		{
			AuctionOffer = auctionOffer;
			WinningBid = winningBid;
			/* We assign winning bid to bid, because defensive programming... 
			 * This way the behavior makes more sense, e.g. when admin uses tag to dispaly the price the participant has provided,
			 * it would show the same amount as if he asked for the winners provided price. Because in this case the participant is also the winner. */
			Bid = winningBid;
		}

		/// <summary>
		/// This constructor is assumed to be used when notifying the user that has lost the auction.
		/// </summary>
		public AuctionSummary(AuctionOffer auctionOffer, AuctionBid bid, AuctionBid winningBid)
		{
			AuctionOffer = auctionOffer;
			Bid = bid;
			WinningBid = winningBid;
			/* The following code can "censor" the winners info. The admin may use tags that allow access to the winners info 
			 * in the message (e.g.) for the users that have lost the auction. It might be what we want to allow, but it also might not...
			 * Thus this code can help us with the censorship. Uncomment if needed. */
			//WinningBid.User = new()
			//{
			//	Username = "-",
			//	Password = "-",
			//	Name = "-",
			//	Surname = "-",
			//	Email = "-",
			//	PhoneNumber = "-",
			//	Residence = "-"
			//};
		}
	}
}
