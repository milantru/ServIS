using BServisData.Interfaces;
using BServisData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BServisData
{
	public class BServisApi : IBServisApi
	{
		IDbContextFactory<BServisDbContext> factory;

		public BServisApi(IDbContextFactory<BServisDbContext> factory)
		{
			this.factory = factory;
		}

		private async Task DeleteItem(IItem item)
		{
			using var context = factory.CreateDbContext();

			context.Remove(item);

			await context.SaveChangesAsync();
		}

		// Create/Update
		public Task<SkidSteerLoader> SaveSkidSteerLoaderAsync(SkidSteerLoader skidSteerLoader)
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

		public Task<ExcavatorPhoto> SaveExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto)
		{
			throw new NotImplementedException();
		}

		public Task<SparePart> SaveSparePartAsync(SparePart sparePart)
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


		public Task<Customer> SaveCustomerAsync(Customer customer)
		{
			throw new NotImplementedException();
		}

		public Task<Administrator> SaveAdministratorAsync(Administrator administrator)
		{
			throw new NotImplementedException();
		}


		public Task<AuctionOffer> SaveAuctionOfferAsync(AuctionOffer auctionOffer)
		{
			throw new NotImplementedException();
		}

		public Task<AuctionBid> SaveAuctionBidAsync(AuctionBid auctionBid)
		{
			throw new NotImplementedException();
		}
		
		// Read
		public Task<List<Excavator>> GetExcavatorsAsync(int numberOfExcavators, int startIndex, string? category = null, string? brand = null, string? model = null)
		{
			throw new NotImplementedException();
		}

		public Task<SkidSteerLoader> GetSkidSteerLoaderAsync(int id)
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

		public Task<List<ExcavatorPhoto>> GetExcavatorPhotosAsync(int excavatorId)
		{
			throw new NotImplementedException();
		}

		public Task<ExcavatorPhoto> GetExcavatorTitlePhotoAsync(int excavatorId)
		{
			throw new NotImplementedException();
		}

		public Task<List<SparePart>> GetSparePartsAsync(int excavatorId)
		{
			throw new NotImplementedException();
		}

		public Task<SparePart> GetSparePartAsync(int id)
		{
			throw new NotImplementedException();
		}


		public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(int numberOfAdditionalEquipments, int startIndex, string excavatorCategory, string? category = null, string? brand = null)
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

		public Task<AdditionalEquipmentPhoto> GetAdditionalEquipmentTitlePhotoAsync(int additionalEquipmentId)
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

		public Task<Administrator> GetAdministratorAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Administrator?> GetAdministratorAsync(string username, string password)
		{
			throw new NotImplementedException();
		}


		public Task<List<AuctionOffer>> GetAuctionOffersAsync(int numberOfAuctionOffers, int startIndex)
		{
			throw new NotImplementedException();
		}

		public Task<AuctionOffer> GetAuctionOfferAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<AuctionBid>> GetAuctionBidsAsync(int auctionOfferId)
		{
			throw new NotImplementedException();
		}

		public Task<AuctionBid> GetAuctionBidAsync(int id)
		{
			throw new NotImplementedException();
		}

		// delete
		public async Task DeleteTrackedSkidSteerLoaderAsync(SkidSteerLoader skidSteerLoader)
		{
			await DeleteItem(skidSteerLoader);
		}

		public async Task DeleteTrackedExcavatorAsync(TrackedExcavator trackedExcavator)
		{
			await DeleteItem(trackedExcavator);
		}

		public async Task DeleteTrackedLoaderAsync(TrackedLoader trackedLoader)
		{
			await DeleteItem(trackedLoader);
		}

		public async Task DeleteExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto)
		{
			await DeleteItem(excavatorPhoto);
		}

		public async Task DeleteSparePartAsync(SparePart sparePart)
		{
			await DeleteItem(sparePart);
		}


		public async Task DeleteAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment)
		{
			await DeleteItem(additionalEquipment);
		}

		public async Task DeleteAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto)
		{
			await DeleteItem(additionalEquipmentPhoto);
		}


		public async Task DeleteCustomerAsync(Customer customer)
		{
			await DeleteItem(customer);
		}

		public async Task DeleteAdministratorAsync(Administrator administrator)
		{
			await DeleteItem(administrator);
		}


		public async Task DeleteAuctionOfferAsync(AuctionOffer auctionOffer)
		{
			await DeleteItem(auctionOffer);
		}

		public async Task DeleteAuctionBidAsync(AuctionBid auctionBid)
		{
			await DeleteItem(auctionBid);
		}

		// more
		public Task AuctionOfferHasEnded()
		{
			throw new NotImplementedException();
		}
	}
}
