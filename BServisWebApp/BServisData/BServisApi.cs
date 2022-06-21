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

			foreach (var sparePart in trackedExcavator.SpareParts) {
				if (!context.SpareParts.Local.Any(sp => sp.Id == sparePart.Id))
				{
					context.SpareParts.Attach(sparePart);
				}
			}

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

			if (!context.Excavators.Local.Any(e => e.Id == excavatorPhoto.Excavator.Id))
			{
				context.Excavators.Attach(excavatorPhoto.Excavator);
			}

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

			foreach (var excavator in sparePart.Excavators)
			{
				if (!context.Excavators.Local.Any(e => e.Id == excavator.Id))
				{
					context.Excavators.Attach(excavator);
				}
			}

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
		public async Task<List<Excavator>> GetExcavatorsAsync(
			int? numberOfExcavators = null, 
			int? startIndex = null, 
			string? category = null, 
			string? brand = null, 
			string? model = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.Excavators;
			if (category != null)
			{
				query.Where(e => e.Category == category);
			}
			if (brand != null)
			{
				query.Where(e => e.Brand == brand);
			}
			if (model != null)
			{
				query.Where(e => e.Model == model);
			}
			var orderedQuery = query.OrderBy(e => e.Name);
			if (startIndex != null)
			{
				orderedQuery.Skip((int)startIndex);
			}
			if (numberOfExcavators != null)
			{
				orderedQuery.Take((int)numberOfExcavators);
			}

			return await orderedQuery.ToListAsync();
		}

		public async Task<int> GetExcavatorsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.Excavators.CountAsync();
		}

		public async Task<int> GetSkidSteerLoadersCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.SkidSteerLoaders.CountAsync();
		}

		public async Task<int> GetTrackedExcavatorsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.TrackedExcavators.CountAsync();
		}

		public async Task<int> GetTrackedLoadersCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.TrackedLoaders.CountAsync();
		}

		public async Task<SkidSteerLoader> GetSkidSteerLoaderAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.SkidSteerLoaders
				.Include(ssl => ssl.SpareParts)
				.FirstOrDefaultAsync(ssl => ssl.Id == id);
		}

		public async Task<TrackedExcavator> GetTrackedExcavatorAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.TrackedExcavators
				.Include(te => te.SpareParts)
				.FirstOrDefaultAsync(te => te.Id == id);
		}

		public async Task<TrackedLoader> GetTrackedLoaderAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.TrackedLoaders
				.Include(tl => tl.SpareParts)
				.FirstOrDefaultAsync(tl => tl.Id == id);
		}

		public async Task<List<ExcavatorPhoto>> GetExcavatorPhotosAsync(int excavatorId)
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPhotos
				.Include(ep => ep.Excavator)
				.Where(ep => ep.Excavator.Id == excavatorId)
				.ToListAsync();
		}

		public async Task<int> GetExcavatorPhotosCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPhotos.CountAsync();
		}

		public async Task<ExcavatorPhoto> GetExcavatorTitlePhotoAsync(int excavatorId)
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPhotos
				.Include(ep => ep.Excavator)
				.Where(ep => ep.Excavator.Id == excavatorId)
				.FirstOrDefaultAsync(ep => ep.IsTitle);
		}

		public async Task<List<SparePart>> GetSparePartsAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.SpareParts
				//.Include(sp => sp.Excavators)
				.ToListAsync();
		}

		public async Task<List<SparePart>> GetSparePartsAsync(int excavatorId)
		{
			using var context = factory.CreateDbContext();

			return await context.SpareParts
				.Include(sp => sp.Excavators)
				.Where(sp => sp.Excavators.Any(e => e.Id == excavatorId))
				.ToListAsync();
		}

		public async Task<int> GetSparePartsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.SpareParts.CountAsync();
		}

		public async Task<SparePart> GetSparePartAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.SpareParts
				.Include(sp => sp.Excavators)
				.FirstOrDefaultAsync(sp => sp.Id == id);
		}


		public async Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(
			int? numberOfAdditionalEquipments = null, 
			int? startIndex = null, 
			string? forWhichExcavatorCategory = null, 
			string? category = null, 
			string? brand = null
			)
		{
			using var context = factory.CreateDbContext();

			var query = context.AdditionalEquipments;
			if (forWhichExcavatorCategory != null)
			{
				query.Where(ae => ae.ForWhichExcavatorCategory == forWhichExcavatorCategory);
			}
			if (category != null)
			{
				query.Where(ae => ae.Category == category);
			}
			if (brand != null)
			{
				query.Where(ae => ae.Brand == brand);
			}
			var orderedQuery = query.OrderBy(ae => ae.Name);
			if (startIndex != null)
			{
				orderedQuery.Skip((int)startIndex);
			}
			if (numberOfAdditionalEquipments != null)
			{
				orderedQuery.Take((int)numberOfAdditionalEquipments);
			}

			return await orderedQuery.ToListAsync();
		}

		public async Task<int> GetAdditionalEquipmentsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipments.CountAsync();
		}

		public async Task<AdditionalEquipment> GetAdditionalEquipmentAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipments
				.FirstOrDefaultAsync(ae => ae.Id == id);
		}

		public async Task<List<AdditionalEquipmentPhoto>> GetAdditionalEquipmentPhotosAsync(int additionalEquipmentId)
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentPhotos
				.Include(aep => aep.AdditionalEquipment)
				.Where(aep => aep.AdditionalEquipment.Id == additionalEquipmentId)
				.ToListAsync();
		}

		public async Task<int> GetAdditionalEquipmentPhotosCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentPhotos.CountAsync();
		}

		public async Task<AdditionalEquipmentPhoto> GetAdditionalEquipmentTitlePhotoAsync(int additionalEquipmentId)
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentPhotos
				.Include(aep => aep.AdditionalEquipment)
				.Where(aep => aep.AdditionalEquipment.Id == additionalEquipmentId)
				.FirstOrDefaultAsync(aep => aep.IsTitle);
		}


		public async Task<Customer> GetCustomerAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.Customers
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Customer?> GetCustomerAsync(string username, string password)
		{
			using var context = factory.CreateDbContext();

			return await context.Customers
				.FirstOrDefaultAsync(c => c.Username == username && c.Password == password);
		}

		public async Task<Administrator> GetAdministratorAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.Administrators
				.FirstOrDefaultAsync(a => a.Id == id);
		}

		public async Task<Administrator?> GetAdministratorAsync(string username, string password)
		{
			using var context = factory.CreateDbContext();

			return await context.Administrators
				.FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
		}


		public async Task<List<AuctionOffer>> GetAuctionOffersAsync(int? numberOfAuctionOffers = null, int? startIndex = null)
		{
			using var context = factory.CreateDbContext();

			var query = context.AuctionOffers
				.Include(ao => ao.Excavator)
				.OrderBy(ao => ao.Excavator.Name);
			if (startIndex != null)
			{
				query.Skip((int)startIndex);
			}
			if (numberOfAuctionOffers != null)
			{
				query.Take((int)numberOfAuctionOffers);
			}

			return await query.ToListAsync();
		}

		public async Task<int> GetAuctionOffersCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionOffers.CountAsync();
		}

		public async Task<AuctionOffer> GetAuctionOfferAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionOffers
				.Include(ao => ao.Excavator)
				.FirstOrDefaultAsync(ao => ao.Id == id);
		}

		public async Task<List<AuctionBid>> GetAuctionBidsAsync(int auctionOfferId)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids
				.Include(ab => ab.User)
				.Include(ab => ab.AuctionOffer)
				.Where(ab => ab.AuctionOffer.Id == auctionOfferId)
				.ToListAsync();
		}

		public async Task<int> GetAuctionBidsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids.CountAsync();
		}

		public async Task<AuctionBid> GetAuctionBidAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids
				.Include(ab => ab.User)
				.Include(ab => ab.AuctionOffer)
				.FirstOrDefaultAsync(ab => ab.Id == id);
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

		// ---------- private methods ----------

		private void UpdateExcavatorData(BServisDbContext context, Excavator currentExcavatorData, Excavator newExcavatorData)
		{
			currentExcavatorData.Category = newExcavatorData.Category;
			currentExcavatorData.Brand = newExcavatorData.Brand;
			currentExcavatorData.Model = newExcavatorData.Model;
			currentExcavatorData.Name = newExcavatorData.Name;
			currentExcavatorData.Description = newExcavatorData.Description;
			currentExcavatorData.LastInspection = newExcavatorData.LastInspection;
			currentExcavatorData.IsNew = newExcavatorData.IsNew;
			//to.SpareParts = from.SpareParts;
			var ids = newExcavatorData.SpareParts.Select(sp => sp.Id);
			currentExcavatorData.SpareParts = context.SpareParts
				.Where(sp => ids.Contains(sp.Id))
				.ToList();
		}

		private void UpdateSkidSteerLoaderData(BServisDbContext context, SkidSteerLoader currentSkidSteerLoaderData, SkidSteerLoader newSkidSteerLoaderData)
		{
			UpdateExcavatorData(context, currentSkidSteerLoaderData, newSkidSteerLoaderData);

			currentSkidSteerLoaderData.HeightMm = newSkidSteerLoaderData.HeightMm;
			currentSkidSteerLoaderData.LengthWithBucketMm = newSkidSteerLoaderData.LengthWithBucketMm;
			currentSkidSteerLoaderData.WidthWithBucketMm = newSkidSteerLoaderData.WidthWithBucketMm;
			currentSkidSteerLoaderData.WeightKg = newSkidSteerLoaderData.WeightKg;
			currentSkidSteerLoaderData.NominalLoadCapacityKg = newSkidSteerLoaderData.NominalLoadCapacityKg;
			currentSkidSteerLoaderData.OverloadPointKg = newSkidSteerLoaderData.OverloadPointKg;
			currentSkidSteerLoaderData.TopSpeedKmh = newSkidSteerLoaderData.TopSpeedKmh;
			currentSkidSteerLoaderData.TopSpeedKmhSpeedVersionMin = newSkidSteerLoaderData.TopSpeedKmhSpeedVersionMin;
			currentSkidSteerLoaderData.TopSpeedKmhSpeedVersionMax = newSkidSteerLoaderData.TopSpeedKmhSpeedVersionMax;
			currentSkidSteerLoaderData.IncreasedBucketVolumeM3 = newSkidSteerLoaderData.IncreasedBucketVolumeM3;
			currentSkidSteerLoaderData.TearingStrengthKn = newSkidSteerLoaderData.TearingStrengthKn;
			currentSkidSteerLoaderData.TractionForceKn = newSkidSteerLoaderData.TractionForceKn;
			currentSkidSteerLoaderData.TractionForceKnSpeedVersion = newSkidSteerLoaderData.TractionForceKnSpeedVersion;
			currentSkidSteerLoaderData.LiftingForceKn = newSkidSteerLoaderData.LiftingForceKn;
			currentSkidSteerLoaderData.ReachMm = newSkidSteerLoaderData.ReachMm;
			currentSkidSteerLoaderData.MaximumDischargeHeightMm = newSkidSteerLoaderData.MaximumDischargeHeightMm;
			currentSkidSteerLoaderData.EngineType = newSkidSteerLoaderData.EngineType;
			currentSkidSteerLoaderData.RatedPowerKw = newSkidSteerLoaderData.RatedPowerKw;
			currentSkidSteerLoaderData.DriveType = newSkidSteerLoaderData.DriveType;
			currentSkidSteerLoaderData.DriveControlHydrogenerator = newSkidSteerLoaderData.DriveControlHydrogenerator;
			currentSkidSteerLoaderData.VehicleHydraulicMotor = newSkidSteerLoaderData.VehicleHydraulicMotor;
			currentSkidSteerLoaderData.VehicleHydraulicMotorOperatingPressureMpa = newSkidSteerLoaderData.VehicleHydraulicMotorOperatingPressureMpa;
			currentSkidSteerLoaderData.ControlType = newSkidSteerLoaderData.ControlType;
			currentSkidSteerLoaderData.OperatingControlPressureMpa = newSkidSteerLoaderData.OperatingControlPressureMpa;
			currentSkidSteerLoaderData.Control = newSkidSteerLoaderData.Control;
			currentSkidSteerLoaderData.WorkEquipmentHydrogenerator = newSkidSteerLoaderData.WorkEquipmentHydrogenerator;
			currentSkidSteerLoaderData.WorkEquipmentSwitchboard = newSkidSteerLoaderData.WorkEquipmentSwitchboard;
			currentSkidSteerLoaderData.OperatingPressureMpa = newSkidSteerLoaderData.OperatingPressureMpa;
			currentSkidSteerLoaderData.OperatingHydraulicFlowLpm = newSkidSteerLoaderData.OperatingHydraulicFlowLpm;
			currentSkidSteerLoaderData.BucketLeveling = newSkidSteerLoaderData.BucketLeveling;
			currentSkidSteerLoaderData.AcousticNoisePowerDb = newSkidSteerLoaderData.AcousticNoisePowerDb;
			currentSkidSteerLoaderData.StandardTiresMin = newSkidSteerLoaderData.StandardTiresMin;
			currentSkidSteerLoaderData.StandardTiresMax = newSkidSteerLoaderData.StandardTiresMax;
			currentSkidSteerLoaderData.ElectricalInstallationV = newSkidSteerLoaderData.ElectricalInstallationV;
		}

		private void UpdateTrackedExcavatorData(BServisDbContext context, TrackedExcavator currentTrackedExcavatorData, TrackedExcavator newTrackedExcavatorData)
		{
			UpdateExcavatorData(context, currentTrackedExcavatorData, newTrackedExcavatorData);

			currentTrackedExcavatorData.OperatingWeightKg = newTrackedExcavatorData.OperatingWeightKg;
			currentTrackedExcavatorData.ExcavationDepthMm = newTrackedExcavatorData.ExcavationDepthMm;
			currentTrackedExcavatorData.MaximumWidthMm = newTrackedExcavatorData.MaximumWidthMm;
			currentTrackedExcavatorData.Engine = newTrackedExcavatorData.Engine;
			currentTrackedExcavatorData.MaximumPowerKw = newTrackedExcavatorData.MaximumPowerKw;
			currentTrackedExcavatorData.TearingStrengthKg = newTrackedExcavatorData.TearingStrengthKg;
			currentTrackedExcavatorData.PenetrationForceKg = newTrackedExcavatorData.PenetrationForceKg;
			currentTrackedExcavatorData.HydraulicFlowLpm = newTrackedExcavatorData.HydraulicFlowLpm;
			currentTrackedExcavatorData.OperatingPressureBar = newTrackedExcavatorData.OperatingPressureBar;
		}

		private void UpdateTrackedLoaderData(BServisDbContext context, TrackedLoader currentTrackedLoaderData, TrackedLoader newTrackedLoaderData)
		{
			UpdateExcavatorData(context, currentTrackedLoaderData, newTrackedLoaderData);

			currentTrackedLoaderData.OperatingWeightKg = newTrackedLoaderData.OperatingWeightKg;
			currentTrackedLoaderData.TiltingLoadKg = newTrackedLoaderData.TiltingLoadKg;
			currentTrackedLoaderData.OperatingLoadCapacityIso14397Kg = newTrackedLoaderData.OperatingLoadCapacityIso14397Kg;
			currentTrackedLoaderData.StandardBucketVolumeM3 = newTrackedLoaderData.StandardBucketVolumeM3;
			currentTrackedLoaderData.Engine = newTrackedLoaderData.Engine;
			currentTrackedLoaderData.MaximumPowerKw = newTrackedLoaderData.MaximumPowerKw;
			currentTrackedLoaderData.TrackWidthMm = newTrackedLoaderData.TrackWidthMm;
			currentTrackedLoaderData.TractionForceKn = newTrackedLoaderData.TractionForceKn;
			currentTrackedLoaderData.HydraulicFlowLpm = newTrackedLoaderData.HydraulicFlowLpm;
			currentTrackedLoaderData.HydraulicFlowHiFlowLpm = newTrackedLoaderData.HydraulicFlowHiFlowLpm;
			currentTrackedLoaderData.MaximumOperatingPressureBar = newTrackedLoaderData.MaximumOperatingPressureBar;
		}

		private void UpdateUserData(BServisDbContext context, User currentUserData, User newUserData)
		{
			currentUserData.Username = newUserData.Username;
			currentUserData.Password = newUserData.Password;
		}

		private void UpdateCustomerData(BServisDbContext context, Customer currentCustomerData, Customer newCustomerData)
		{
			UpdateUserData(context, currentCustomerData, newCustomerData);

			currentCustomerData.Name = newCustomerData.Name;
			currentCustomerData.Surname = newCustomerData.Surname;
			currentCustomerData.PhoneNumber = newCustomerData.PhoneNumber;
			currentCustomerData.Email = newCustomerData.Email;
			currentCustomerData.Residence = newCustomerData.Residence;
			currentCustomerData.IsTemporary = newCustomerData.IsTemporary;
		}

		private void UpdateAdministratorData(BServisDbContext context, Administrator currentAdministratorData, User newAdministratorData)
		{
			UpdateUserData(context, currentAdministratorData, newAdministratorData);
		}

		//private async Task<List<Excavator>> GetExcavatorsAsync(
		//	int? numberOfExcavators = null,
		//	int? startIndex = null,
		//	string? category = null,
		//	string? brand = null,
		//	string? model = null
		//)
		//{
		//	using var context = factory.CreateDbContext();

		//	var query = context.Excavators.Include(e => e.SpareParts);
		//	if (category != null)
		//	{
		//		query.Where(e => e.Category == category);
		//	}
		//	if (brand != null)
		//	{
		//		query.Where(e => e.Brand == brand);
		//	}
		//	if (model != null)
		//	{
		//		query.Where(e => e.Model == model);
		//	}
		//	var orderedQuery = query.OrderBy(e => e.Name);
		//	if (startIndex != null)
		//	{
		//		orderedQuery.Skip((int)startIndex);
		//	}
		//	if (numberOfExcavators != null)
		//	{
		//		orderedQuery.Take((int)numberOfExcavators);
		//	}

		//	return await orderedQuery.ToListAsync();
		//}

		private async Task DeleteItem(IItem item)
		{
			using var context = factory.CreateDbContext();

			context.Remove(item);

			await context.SaveChangesAsync();
		}
	}
}
