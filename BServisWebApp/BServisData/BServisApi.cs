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

		// Create/Update
		public async Task<SkidSteerLoader> SaveSkidSteerLoaderAsync(SkidSteerLoader skidSteerLoader)
		{
			using var context = factory.CreateDbContext();
			SkidSteerLoader currentSkidSteerLoader;

			if (skidSteerLoader.Id == 0)
			{
				context.Add(skidSteerLoader);
			}
			else
			{
				currentSkidSteerLoader = await context.SkidSteerLoaders
					.Include(ssl => ssl.SpareParts)
					.FirstOrDefaultAsync(ssl => ssl.Id == skidSteerLoader.Id);

				UpdateSkidSteerLoaderData(context, currentSkidSteerLoader, skidSteerLoader);
			}

			await context.SaveChangesAsync();

			return skidSteerLoader;
		}

		public async Task<TrackedExcavator> SaveTrackedExcavatorAsync(TrackedExcavator trackedExcavator)
		{
			using var context = factory.CreateDbContext();
			TrackedExcavator currentTrackedExcavator;

			if (trackedExcavator.Id == 0)
		{
				context.Add(trackedExcavator);
		}
			else
			{
				currentTrackedExcavator = await context.TrackedExcavators
					.Include(te => te.SpareParts)
					.FirstOrDefaultAsync(te => te.Id == trackedExcavator.Id);

				UpdateTrackedExcavatorData(context, currentTrackedExcavator, trackedExcavator);
			}

			await context.SaveChangesAsync();

			return trackedExcavator;
		}

		public async Task<TrackedLoader> SaveTrackedLoaderAsync(TrackedLoader trackedLoader)
		{
			using var context = factory.CreateDbContext();
			TrackedLoader currentTrackedLoader;

			if (trackedLoader.Id == 0)
			{
				context.Add(trackedLoader);
		}
			else
			{
				currentTrackedLoader = await context.TrackedLoaders
					.Include(tl => tl.SpareParts)
					.FirstOrDefaultAsync(tl => tl.Id == trackedLoader.Id);

				UpdateTrackedLoaderData(context, currentTrackedLoader, trackedLoader);
			}

			await context.SaveChangesAsync();

			return trackedLoader;
		}

		public async Task<ExcavatorPhoto> SaveExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto)
		{
			using var context = factory.CreateDbContext();
			ExcavatorPhoto currentExcavatorPhoto;

			if (excavatorPhoto.Id == 0)
			{
				context.Add(excavatorPhoto);
		}
			else
			{
				currentExcavatorPhoto = await context.ExcavatorPhotos
					.Include(ep => ep.Excavator)
					.FirstOrDefaultAsync(ep => ep.Id == excavatorPhoto.Id);

				currentExcavatorPhoto.Excavator = excavatorPhoto.Excavator;
				//int excavatorId = excavatorPhoto.Excavator.Id;
				//currentExcavatorPhoto.Excavator = await context.Excavators
				//	.FirstOrDefaultAsync(e => e.Id == excavatorId);
				currentExcavatorPhoto.Photo = excavatorPhoto.Photo;
				currentExcavatorPhoto.IsTitle = excavatorPhoto.IsTitle;
			}

			await context.SaveChangesAsync();

			return excavatorPhoto;
		}

		public async Task<SparePart> SaveSparePartAsync(SparePart sparePart)
		{
			using var context = factory.CreateDbContext();
			SparePart currentSparePart;

			if (sparePart.Id == 0)
			{
				context.Add(sparePart);
		}
			else
			{
				currentSparePart = await context.SpareParts
					.Include(sp => sp.Excavators)
					.FirstOrDefaultAsync(sp => sp.Id == sparePart.Id);

				currentSparePart.CatalogNumber = sparePart.CatalogNumber;
				currentSparePart.Name = sparePart.Name;
				//currentSparePart.Excavators = sparePart.Excavators;
				var ids = sparePart.Excavators.Select(e => e.Id);
				currentSparePart.Excavators = context.Excavators
					.Where(e => ids.Contains(e.Id))
					.ToList();
			}

			await context.SaveChangesAsync();

			return sparePart;
		}


		public async Task<AdditionalEquipment> SaveAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment)
		{
			using var context = factory.CreateDbContext();
			AdditionalEquipment currentAdditionalEquipment;

			if (additionalEquipment.Id == 0)
			{
				context.Add(additionalEquipment);
		}
			else
			{
				currentAdditionalEquipment = await context.AdditionalEquipments
					.FirstOrDefaultAsync(ae => ae.Id == additionalEquipment.Id);

				currentAdditionalEquipment.ForWhichExcavatorCategory = additionalEquipment.ForWhichExcavatorCategory;
				currentAdditionalEquipment.Category = additionalEquipment.Category;
				currentAdditionalEquipment.Brand = additionalEquipment.Brand;
				currentAdditionalEquipment.Name = additionalEquipment.Name;
				currentAdditionalEquipment.Description = additionalEquipment.Description;
				currentAdditionalEquipment.Price = additionalEquipment.Price;
			}

			await context.SaveChangesAsync();

			return additionalEquipment;
		}

		public async Task<AdditionalEquipmentPhoto> SaveAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto)
		{
			using var context = factory.CreateDbContext();
			AdditionalEquipmentPhoto currentAdditionalEquipmentPhoto;

			if (additionalEquipmentPhoto.Id == 0)
			{
				context.Add(additionalEquipmentPhoto);
		}
			else
			{
				currentAdditionalEquipmentPhoto = await context.AdditionalEquipmentPhotos
					.Include(aep => aep.AdditionalEquipment)
					.FirstOrDefaultAsync(aep => aep.Id == additionalEquipmentPhoto.Id);

				currentAdditionalEquipmentPhoto.AdditionalEquipment = additionalEquipmentPhoto.AdditionalEquipment;
				//int additionalEquipmentPhotoId = additionalEquipmentPhoto.AdditionalEquipment.Id;
				//currentAdditionalEquipmentPhoto.AdditionalEquipment = await context.AdditionalEquipments
				//	.FirstOrDefaultAsync(ae => ae.Id == additionalEquipmentPhotoId);
				currentAdditionalEquipmentPhoto.Photo = additionalEquipmentPhoto.Photo;
				currentAdditionalEquipmentPhoto.IsTitle = additionalEquipmentPhoto.IsTitle;
			}

			await context.SaveChangesAsync();

			return additionalEquipmentPhoto;
		}


		public async Task<Customer> SaveCustomerAsync(Customer customer)
		{
			using var context = factory.CreateDbContext();
			Customer currentCustomer;

			if (customer.Id == 0)
			{
				context.Add(customer);
		}
			else
			{
				currentCustomer = await context.Customers
					.FirstOrDefaultAsync(c => c.Id == customer.Id);

				UpdateCustomerData(context, currentCustomer, customer);
			}

			await context.SaveChangesAsync();

			return customer;
		}

		public async Task<Administrator> SaveAdministratorAsync(Administrator administrator)
		{
			using var context = factory.CreateDbContext();
			Administrator currentAdministrator;

			if (administrator.Id == 0)
			{
				context.Add(administrator);
		}
			else
			{
				currentAdministrator = await context.Administrators
					.FirstOrDefaultAsync(a => a.Id == administrator.Id);

				UpdateAdministratorData(context, currentAdministrator, administrator);
			}

			await context.SaveChangesAsync();

			return administrator;
		}


		public async Task<AuctionOffer> SaveAuctionOfferAsync(AuctionOffer auctionOffer)
		{
			using var context = factory.CreateDbContext();
			AuctionOffer currentAuctionOffer;

			if (auctionOffer.Id == 0)
			{
				context.Add(auctionOffer);
		}
			else
			{
				currentAuctionOffer = await context.AuctionOffers
					.Include(ao => ao.Excavator)
					.FirstOrDefaultAsync(ao => ao.Id == auctionOffer.Id);

				currentAuctionOffer.Excavator = auctionOffer.Excavator;
				//int excavatorId = auctionOffer.Excavator.Id;
				//currentAuctionOffer.Excavator = await context.Excavators
				//	.FirstOrDefaultAsync(e => e.Id == excavatorId);
				currentAuctionOffer.Description = auctionOffer.Description;
				currentAuctionOffer.OfferEnd = auctionOffer.OfferEnd;
				currentAuctionOffer.StartingBid = auctionOffer.StartingBid;
			}

			await context.SaveChangesAsync();

			return auctionOffer;
		}

		public async Task<AuctionBid> SaveAuctionBidAsync(AuctionBid auctionBid)
		{
			using var context = factory.CreateDbContext();
			AuctionBid currentAuctionBid;

			if (auctionBid.Id == 0)
			{
				context.Add(auctionBid);
		}
			else
			{
				currentAuctionBid = await context.AuctionBids
					.Include(ab => ab.User)
					.Include(ab => ab.AuctionOffer)
					.FirstOrDefaultAsync(ab => ab.Id == auctionBid.Id);
		
				currentAuctionBid.User = auctionBid.User;
				//int userId = auctionBid.User.Id;
				//currentAuctionBid.User = await context.Users
				//	.FirstOrDefaultAsync(u => u.Id == userId);
				currentAuctionBid.AuctionOffer = auctionBid.AuctionOffer;
				//int auctionOfferId = auctionBid.AuctionOffer.Id;
				//currentAuctionBid.AuctionOffer = await context.AuctionOffers
				//	.FirstOrDefaultAsync(ao => ao.Id == auctionOfferId);
				currentAuctionBid.Bid = auctionBid.Bid;
			}

			await context.SaveChangesAsync();

			return auctionBid;
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
