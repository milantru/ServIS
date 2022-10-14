using ServISData.DataOperations;
using ServISData.Models;

namespace ServISData.Interfaces
{
	public interface IServISApi
	{
		// Create/Update
		public Task<Excavator> SaveExcavatorAsync(Excavator excavator);
		public Task<ExcavatorBrand> SaveExcavatorBrandAsync(ExcavatorBrand excavatorBrand);
		public Task<ExcavatorCategory> SaveExcavatorCategoryAsync(ExcavatorCategory excavatorCategory);
		public Task<ExcavatorType> SaveExcavatorTypeAsync(ExcavatorType excavatorType);
		public Task<ExcavatorPropertyType> SaveExcavatorPropertyTypeAsync(ExcavatorPropertyType excavatorPropertyType);
		public Task<SparePart> SaveSparePartAsync(SparePart sparePart);
		public Task<MainOffer> SaveMainOfferAsync(MainOffer mainOffer);
		public Task<AdditionalEquipment> SaveAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment);
		public Task<AdditionalEquipmentBrand> SaveAdditionalEquipmentBrandAsync(AdditionalEquipmentBrand additionalEquipmentBrand);
		public Task<AdditionalEquipmentCategory> SaveAdditionalEquipmentCategoryAsync(AdditionalEquipmentCategory additionalEquipmentCategory);
		public Task<User> SaveUserAsync(User user);
		public Task<AuctionOffer> SaveAuctionOfferAsync(AuctionOffer auctionOffer);
		public Task<AuctionBid> SaveAuctionBidAsync(AuctionBid auctionBid);

		// Read
		public Task<List<Excavator>> GetExcavatorsAsync(DataOperations<Excavator>? dataOperations = null);
		public Task<Excavator> GetExcavatorAsync(int id);
		public Task<int> GetExcavatorsCountAsync(ExcavatorType? type = null);
		public Task<List<ExcavatorPhoto>> GetExcavatorPhotosAsync(int excavatorId);
		public Task<int> GetExcavatorPhotosCountAsync();
		public Task<List<ExcavatorBrand>> GetExcavatorBrandsAsync(DataOperations<ExcavatorBrand>? dataOperations = null);
		public Task<int> GetExcavatorBrandsCountAsync();
		public Task<List<ExcavatorCategory>> GetExcavatorCategoriesAsync(DataOperations<ExcavatorCategory>? dataOperations = null);
		public Task<int> GetExcavatorCategoriesCountAsync();
		public Task<List<ExcavatorType>> GetExcavatorTypesAsync(DataOperations<ExcavatorType>? dataOperations = null);
		public Task<int> GetExcavatorTypesCountAsync();
		public Task<List<ExcavatorPropertyType>> GetExcavatorPropertyTypesAsync(DataOperations<ExcavatorPropertyType>? dataOperations = null);
		public Task<int> GetExcavatorPropertyTypesCountAsync();
		public Task<ExcavatorType> GetExcavatorTypeAsync(int id);
		public Task<ExcavatorPhoto> GetExcavatorTitlePhotoAsync(int excavatorId);
		public Task<List<SparePart>> GetSparePartsAsync(DataOperations<SparePart>? dataOperations = null);
		public Task<List<SparePart>> GetSparePartsAsync(int excavatorId);
		public Task<int> GetSparePartsCountAsync();
		public Task<SparePart> GetSparePartAsync(int id);
		public Task<List<MainOffer>> GetMainOffersAsync();
		public Task<MainOffer> GetMainOfferAsync(int id);
		public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(DataOperations<AdditionalEquipment>? dataOperations = null);
		public Task<AdditionalEquipment> GetAdditionalEquipmentAsync(int id);
		public Task<int> GetAdditionalEquipmentsCountAsync();
		public Task<List<AdditionalEquipmentPhoto>> GetAdditionalEquipmentPhotosAsync(int additionalEquipmentId);
		public Task<int> GetAdditionalEquipmentPhotosCountAsync();
		public Task<AdditionalEquipmentPhoto> GetAdditionalEquipmentTitlePhotoAsync(int additionalEquipmentId);
		public Task<List<AdditionalEquipmentBrand>> GetAdditionalEquipmentBrandsAsync(DataOperations<AdditionalEquipmentBrand>? dataOperations = null);
		public Task<int> GetAdditionalEquipmentBrandsCountAsync();
		public Task<List<AdditionalEquipmentCategory>> GetAdditionalEquipmentCategoriesAsync(DataOperations<AdditionalEquipmentCategory>? dataOperations = null);
		public Task<int> GetAdditionalEquipmentCategoriesCountAsync();
		public Task<User> GetUserAsync(int id);
		public Task<User> GetUserAsync(string username);
		public Task<List<AuctionOffer>> GetAuctionOffersAsync(DataOperations<AuctionOffer>? dataOperations = null);
		public Task<int> GetAuctionOffersCountAsync();
		public Task<AuctionOffer> GetAuctionOfferAsync(int id);
		public Task<List<AuctionBid>> GetAuctionBidsAsync(int auctionOfferId);
		public Task<AuctionBid> GetAuctionBidAsync(int id);
		public Task<int> GetAuctionBidsCountAsync();

		// delete
		public Task DeleteExcavatorAsync(Excavator excavator);
		public Task DeleteExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto);
		public Task DeleteExcavatorBrandAsync(ExcavatorBrand excavatorBrand);
		public Task DeleteExcavatorCategoryAsync(ExcavatorCategory excavatorCategory);
		public Task DeleteExcavatorTypeAsync(ExcavatorType excavatorType);
		public Task DeleteExcavatorPropertyAsync(ExcavatorProperty excavatorProperty);
		public Task DeleteExcavatorPropertyTypeAsync(ExcavatorPropertyType excavatorPropertyType);
		public Task DeleteSparePartAsync(SparePart sparePart);
		public Task DeleteMainOfferAsync(MainOffer mainOffer);
		public Task DeleteAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment);
		public Task DeleteAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto);
		public Task DeleteAdditionalEquipmentBrandAsync(AdditionalEquipmentBrand additionalEquipmentBrand);
		public Task DeleteAdditionalEquipmentCategoryAsync(AdditionalEquipmentCategory additionalEquipmentCategory);
		public Task DeleteUserAsync(User user);
		public Task DeleteAuctionOfferAsync(AuctionOffer auctionOffer);
		public Task DeleteAuctionBidAsync(AuctionBid auctionBid);

		// more
		public Task AuctionOfferHasEnded();
	}
}
