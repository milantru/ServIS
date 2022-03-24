using BServisData.Interfaces;
using BServisData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BServisData
{
	public class BServisApi : IBServisApi
	{
		public Task AuctionOfferHasEnded()
		{
			throw new NotImplementedException();
		}

		public Task DeleteAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAdministratorAsync(Administrator administrator)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAuctionBidAsync(AuctionBid auctionBid)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAuctionOfferAsync(AuctionOffer auctionOffer)
		{
			throw new NotImplementedException();
		}

		public Task DeleteCustomerAsync(Customer customer)
		{
			throw new NotImplementedException();
		}

		public Task DeleteExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto)
		{
			throw new NotImplementedException();
		}

		public Task DeleteSparePartAsync(SparePart sparePart)
		{
			throw new NotImplementedException();
		}

		public Task DeleteTrackedExcavatorAsync(TrackedExcavator trackedExcavator)
		{
			throw new NotImplementedException();
		}

		public Task DeleteTrackedLoaderAsync(TrackedLoader trackedLoader)
		{
			throw new NotImplementedException();
		}

		public Task DeleteTrackedSkidSteerLoaderAsync(SkidSteerLoader skidSteerLoader)
		{
			throw new NotImplementedException();
		}

		public Task<AdditionalEquipment> GetAdditionalEquipmentAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<AdditionalEquipmentPhoto>> GetAdditionalEquipmentPhotosAsync(int additionalEquipmentId)
		{
			throw new NotImplementedException();
		}

		public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(int numberOfAdditionalEquipments, int startIndex, string excavatorCategory, string? category = null, string? brand = null)
		{
			throw new NotImplementedException();
		}

		public Task<AdditionalEquipmentPhoto> GetAdditionalEquipmentTitlePhotoAsync(int additionalEquipmentId)
		{
			throw new NotImplementedException();
		}

		public Task<Administrator> GetAdministratorAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Administrator?> GetAdministratorAsync(string username, string password)
		{
			throw new NotImplementedException();
		}

		public Task<AuctionBid> GetAuctionBidAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<AuctionBid>> GetAuctionBidsAsync(int auctionOfferId)
		{
			throw new NotImplementedException();
		}

		public Task<AuctionOffer> GetAuctionOfferAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<AuctionOffer>> GetAuctionOffersAsync(int numberOfAuctionOffers, int startIndex)
		{
			throw new NotImplementedException();
		}

		public Task<Customer> GetCustomerAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Customer?> GetCustomerAsync(string username, string password)
		{
			throw new NotImplementedException();
		}

		public Task<List<ExcavatorPhoto>> GetExcavatorPhotosAsync(int excavatorId)
		{
			throw new NotImplementedException();
		}

		public Task<List<Excavator>> GetExcavatorsAsync(int numberOfExcavators, int startIndex, string? category = null, string? brand = null, string? model = null)
		{
			throw new NotImplementedException();
		}

		public Task<ExcavatorPhoto> GetExcavatorTitlePhotoAsync(int excavatorId)
		{
			throw new NotImplementedException();
		}

		public Task<SkidSteerLoader> GetSkidSteerLoaderAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<SparePart> GetSparePartAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<SparePart>> GetSparePartsAsync(int excavatorId)
		{
			throw new NotImplementedException();
		}

		public Task<TrackedExcavator> GetTrackedExcavatorAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<TrackedLoader> GetTrackedLoaderAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<AdditionalEquipment> SaveAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment)
		{
			throw new NotImplementedException();
		}

		public Task<AdditionalEquipmentPhoto> SaveAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto)
		{
			throw new NotImplementedException();
		}

		public Task<Administrator> SaveAdministratorAsync(Administrator administrator)
		{
			throw new NotImplementedException();
		}

		public Task<AuctionBid> SaveAuctionBidAsync(AuctionBid auctionBid)
		{
			throw new NotImplementedException();
		}

		public Task<AuctionOffer> SaveAuctionOfferAsync(AuctionOffer auctionOffer)
		{
			throw new NotImplementedException();
		}

		public Task<Customer> SaveCustomerAsync(Customer customer)
		{
			throw new NotImplementedException();
		}

		public Task<ExcavatorPhoto> SaveExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto)
		{
			throw new NotImplementedException();
		}

		public Task<SkidSteerLoader> SaveSkidSteerLoaderAsync(SkidSteerLoader skidSteerLoader)
		{
			throw new NotImplementedException();
		}

		public Task<SparePart> SaveSparePartAsync(SparePart sparePart)
		{
			throw new NotImplementedException();
		}

		public Task<TrackedExcavator> SaveTrackedExcavatorAsync(TrackedExcavator trackedExcavator)
		{
			throw new NotImplementedException();
		}

		public Task<TrackedLoader> SaveTrackedLoaderAsync(TrackedLoader trackedLoader)
		{
			throw new NotImplementedException();
		}
	}
}
