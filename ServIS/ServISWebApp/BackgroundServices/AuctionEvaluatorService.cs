using ServISData.Interfaces;
using ServISData.Models;
using ServISWebApp.Shared;

namespace ServISWebApp.BackgroundServices
{
	public class AuctionEvaluatorService : TimerService
	{
		private readonly IServISApi api;
		private readonly EmailManager emailManager;
		private readonly string baseUrl;
		private event Func<Task>? updateEvent;

		public AuctionEvaluatorService(
			IServISApi api,
			EmailManager emailManager,
			string baseUrl
		) : base(TimeSpan.FromMinutes(1))
		{
			this.api = api;
			this.emailManager = emailManager;
			this.baseUrl = baseUrl;

			RegisterEventHandler();
		}

		protected override Func<Task>? GetEventHandlers() => updateEvent;

		private void RegisterEventHandler()
		{
			updateEvent += async () =>
			{
				var dateTimeNow = DateTime.Now;
				var endedAuctionOffers = await GetEndedAuctionOffers(dateTimeNow, includeEvaluated: false);
				if (endedAuctionOffers.Count == 0)
				{
					return;
				}

				await EvaluateAuctionOffersAsync(dateTimeNow, endedAuctionOffers);
			};
		}

		private async Task<List<AuctionOffer>> GetEndedAuctionOffers(DateTime dateTimeNow, bool includeEvaluated = true)
		{
			var auctionOffers = await api.GetAuctionOffersAsync();

			var auctionOffersEnded = auctionOffers
				.Where(ao => (ao.OfferEnd < dateTimeNow) && (includeEvaluated || !ao.IsEvaluated))
				.ToList();

			return auctionOffersEnded;
		}

		private void NotifyAdminAuctionEndedWithWinner(
			DateTime dateTimeNow,
			AuctionBid maxAuctionBid,
			User auctionWinner,
			string auctionWinnerFullname,
			Excavator excavator
		)
		{
			var linkToAuctionOfferDetail = $"{baseUrl}/aukcna-ponuka/{maxAuctionBid.AuctionOffer.Id}";

			var emailForAdmin = new Email
			{
				DateTime = dateTimeNow,
				FromName = auctionWinnerFullname,
				FromAddress = auctionWinner.Email,
				ToName = emailManager.EmailName,
				ToAddress = emailManager.EmailAddress,
				Subject = $"Odpoveď na výhru {excavator.Name} v aukcii",
				Text = $"<injected_url>{linkToAuctionOfferDetail}</injected_url>\n" +
				$"Výherný predmet: {excavator.Name}\n" +
				$"Ponúknutá cena: {maxAuctionBid.Bid} €.\n" +
				$"Meno výhercu: {auctionWinnerFullname}\n" +
				"Kontakt na výhercu:\n" +
				$"\t-telefón: {auctionWinner.PhoneNumber ?? "Neposkytol"}\n" +
				$"\t-email: {auctionWinner.Email}"
			};

			_ = emailManager.SendEmailAsync(emailForAdmin);
		}

		private void NotifyUser(DateTime dateTimeNow, User user, string userFullname, string subject, string text)
		{
			var emailForWinner = new Email
			{
				DateTime = dateTimeNow,
				FromName = emailManager.EmailName,
				FromAddress = emailManager.EmailAddress,
				ToName = userFullname,
				ToAddress = user.Email,
				Subject = subject,
				Text = text
			};

			_ = emailManager.SendEmailAsync(emailForWinner);
		}

		private void NotifyWinner(
			DateTime dateTimeNow,
			AuctionBid maxAuctionBid,
			User auctionWinner,
			string auctionWinnerFullname,
			Excavator excavator
		)
		{
			/* Subject must start with "Vyhrali ste ", 
			 * because we check for this in EmailManagers ShouldSkip method */
			var subject = $"Vyhrali ste {excavator.Name} v aukcii!";
			var text = $"Gratulujeme, vyhrali ste {excavator.Name} v aukcii za {maxAuctionBid.Bid} €.\n" +
				"O ďalšom postupe Vás budeme informovať.\n" +
				"(Tento email bol vygenerovaný automaticky. Prosíme, aby ste naň neodpovedali.)";

			NotifyUser(dateTimeNow, auctionWinner, auctionWinnerFullname, subject, text);
		}

		private void NotifyLoser(
			DateTime dateTimeNow,
			AuctionBid lostBid,
			User auctionLoser,
			string auctionLoserFullname,
			Excavator excavator,
			AuctionBid maxAuctionBid
		)
		{
			/* Subject must start with "Aukcia s predmetom ", 
			 * because we check for this in EmailManagers ShouldSkip method */
			var subject = $"Aukcia s predmetom {excavator.Name} sa skončila";
			var text = $"S ľútosťou Vám oznamujeme, že ste aukciu s predmetom {excavator.Name} nevyhrali.\n" +
				$"Predmet bol vydražený na cenu {maxAuctionBid.Bid} €.\n" +
				$"Vy ste ponúkli {lostBid.Bid}.\n" +
				"Prajeme viacej šťastia nabudúce ;)\n" +
				"(Tento email bol vygenerovaný automaticky. Prosíme, aby ste naň neodpovedali.)";

			NotifyUser(dateTimeNow, auctionLoser, auctionLoserFullname, subject, text);
		}

		private async Task EvaluateAuctionOffersAsync(
			DateTime dateTimeNow,
			List<AuctionOffer> endedAuctionOffers
		)
		{
			foreach (var offer in endedAuctionOffers)
			{
				var maxAuctionBid = await api.GetMaxAuctionBidAsync(offer.Id);
				if (maxAuctionBid is null)
				{// there were no bids (no participants)
					NotifyAdminAuctionEndedWithoutWinner(dateTimeNow, offer);
					offer.OfferEnd += TimeSpan.FromDays(7);
				}
				else
				{
					var auctionWinner = maxAuctionBid.User;
					var auctionWinnerFullname = $"{auctionWinner.Name} {auctionWinner.Surname}";
					var excavator = maxAuctionBid.AuctionOffer.Excavator;

					NotifyWinner(dateTimeNow, maxAuctionBid, auctionWinner, auctionWinnerFullname, excavator);

					NotifyAdminAuctionEndedWithWinner(dateTimeNow, maxAuctionBid, auctionWinner, auctionWinnerFullname, excavator);

					var lostAuctionBids = await api.GetLostAuctionBidsAsync(offer.Id);
					NotifyLosers(dateTimeNow, lostAuctionBids, maxAuctionBid, excavator);
				}

				offer.IsEvaluated = true;
				_ = api.SaveAuctionOfferAsync(offer);
			}
		}

		private void NotifyLosers(
			DateTime dateTimeNow,
			List<AuctionBid> lostAuctionBids,
			AuctionBid maxAuctionBid,
			Excavator excavator
		)
		{
			foreach (var bid in lostAuctionBids)
			{
				var auctionLoser = bid.User;
				var loserFullname = $"{auctionLoser.Name} {auctionLoser.Surname}";
				NotifyLoser(dateTimeNow, bid, auctionLoser, loserFullname, excavator, maxAuctionBid);
			}
		}

		private void NotifyAdminAuctionEndedWithoutWinner(DateTime dateTimeNow, AuctionOffer aoe)
		{
			var excavator = aoe.Excavator;
			var linkToAuctionOfferDetail = $"{baseUrl}/aukcna-ponuka/{aoe.Id}";

			var emailForAdmin = new Email
			{
				DateTime = dateTimeNow,
				FromName = emailManager.EmailName,
				FromAddress = emailManager.EmailAddress,
				ToName = emailManager.EmailName,
				ToAddress = emailManager.EmailAddress,
				Subject = $"Aukcia s {excavator.Name} skončila bez výhercu",
				Text = $"<injected_url>{linkToAuctionOfferDetail}</injected_url>\n" +
				$"Aukčný predmet: {excavator.Name}\n" +
				$"Počiatočná cena: {aoe.StartingBid} €."
			};

			_ = emailManager.SendEmailAsync(emailForAdmin);
		}
	}
}
