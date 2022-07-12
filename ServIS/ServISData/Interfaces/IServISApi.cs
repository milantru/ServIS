using ServISData.Models;

namespace ServISData.Interfaces
{
	public interface IServISApi
	{
		// Create/Update
		public Task<SkidSteerLoader?> SaveSkidSteerLoaderAsync(SkidSteerLoader skidSteerLoader);
		public Task<TrackedExcavator?> SaveTrackedExcavatorAsync(TrackedExcavator trackedExcavator);
		public Task<TrackedLoader?> SaveTrackedLoaderAsync(TrackedLoader trackedLoader);
		public Task<ExcavatorPhoto?> SaveExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto);
		public Task<SparePart?> SaveSparePartAsync(SparePart sparePart);

		public Task<AdditionalEquipment?> SaveAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment);
		public Task<AdditionalEquipmentPhoto?> SaveAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto);

		public Task<Customer?> SaveCustomerAsync(Customer customer);
		public Task<Administrator?> SaveAdministratorAsync(Administrator administrator);

		public Task<AuctionOffer?> SaveAuctionOfferAsync(AuctionOffer auctionOffer);
		public Task<AuctionBid?> SaveAuctionBidAsync(AuctionBid auctionBid);

		// Read
		public Task<List<Excavator>> GetExcavatorsAsync(int? numberOfExcavators = null, int? startIndex = null, string? category = null, string ? brand = null, string? model = null);
		public Task<int> GetExcavatorsCountAsync();
		public Task<int> GetSkidSteerLoadersCountAsync();
		public Task<int> GetTrackedExcavatorsCountAsync();
		public Task<int> GetTrackedLoadersCountAsync();
		public Task<SkidSteerLoader?> GetSkidSteerLoaderAsync(int id);
		public Task<TrackedExcavator?> GetTrackedExcavatorAsync(int id);
		public Task<TrackedLoader?> GetTrackedLoaderAsync(int id);
		public Task<List<ExcavatorPhoto>> GetExcavatorPhotosAsync(int excavatorId, bool shouldIncludeExcavator = true);
		public Task<int> GetExcavatorPhotosCountAsync();
		public Task<ExcavatorPhoto?> GetExcavatorTitlePhotoAsync(int excavatorId);
		public Task<List<SparePart>> GetSparePartsAsync(int? numberOfSpareParts = null, int? startIndex = null);
		public Task<List<SparePart>> GetSparePartsAsync(int excavatorId);
		public Task<int> GetSparePartsCountAsync();
		public Task<SparePart?> GetSparePartAsync(int id);

		public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(int? numberOfAdditionalEquipments = null, int? startIndex = null, string? excavatorCategory = null, string? category = null, string? brand = null);
		public Task<int> GetAdditionalEquipmentsCountAsync();
		public Task<AdditionalEquipment?> GetAdditionalEquipmentAsync(int id);
		public Task<List<AdditionalEquipmentPhoto>> GetAdditionalEquipmentPhotosAsync(int additionalEquipmentId);
		public Task<int> GetAdditionalEquipmentPhotosCountAsync();
		public Task<AdditionalEquipmentPhoto?> GetAdditionalEquipmentTitlePhotoAsync(int additionalEquipmentId);

		public Task<Customer?> GetCustomerAsync(int id);
		public Task<Customer?> GetCustomerAsync(string username, string password);
		public Task<Administrator?> GetAdministratorAsync(int id);
		public Task<Administrator?> GetAdministratorAsync(string username, string password);

		public Task<List<AuctionOffer>> GetAuctionOffersAsync(int? numberOfAuctionOffers = null, int? startIndex = null);
		public Task<int> GetAuctionOffersCountAsync();
		public Task<AuctionOffer?> GetAuctionOfferAsync(int id);
		public Task<List<AuctionBid>> GetAuctionBidsAsync(int auctionOfferId);
		public Task<int> GetAuctionBidsCountAsync();
		public Task<AuctionBid?> GetAuctionBidAsync(int id);

		// delete
		public Task DeleteExcavatorAsync(Excavator excavator);
		public Task DeleteExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto);
		public Task DeleteSparePartAsync(SparePart sparePart);

		public Task DeleteAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment);
		public Task DeleteAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto);

		public Task DeleteCustomerAsync(Customer customer);
		public Task DeleteAdministratorAsync(Administrator administrator);

		public Task DeleteAuctionOfferAsync(AuctionOffer auctionOffer);
		public Task DeleteAuctionBidAsync(AuctionBid auctionBid);

		// more
		public Task AuctionOfferHasEnded();
	}
}
