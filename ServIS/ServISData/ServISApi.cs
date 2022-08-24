﻿using ServISData.Interfaces;
using ServISData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData
{
	public class ServISApi : IServISApi
	{
		IDbContextFactory<ServISDbContext> factory;

		public ServISApi(IDbContextFactory<ServISDbContext> factory)
		{
			this.factory = factory;
		}

		// Create/Update
		public async Task<Excavator> SaveExcavatorAsync(Excavator excavator)
		{
			using var context = factory.CreateDbContext();
			Excavator currentExcavator;

			List<IList<Excavator>> excavatorsTmp = null!;
			if (excavator.Id == 0)
			{
				context.Attach(excavator.Type);
				excavatorsTmp = new List<IList<Excavator>>(excavator.SpareParts.Count);
				for (int i = 0; i < excavator.SpareParts.Count; i++)
				{
					var sparePart = excavator.SpareParts[i];
					excavatorsTmp.Add(sparePart.Excavators);
					sparePart.Excavators = null!; // TODO: mere hotfix, but this is not pretty, should be changed!
				}
				context.AttachRange(excavator.SpareParts);

				context.Add(excavator);
			}
			else
			{
				currentExcavator = await context.Excavators
					.Include(e => e.Photos)
					.Include(e => e.Type)
					.Include(e => e.Properties)
					.ThenInclude(ep => ep.PropertyType)
					.Include(e => e.SpareParts)
					.FirstAsync(e => e.Id == excavator.Id);

				await UpdateExcavatorDataAsync(context, currentExcavator, excavator);
			}

			await context.SaveChangesAsync();
			if (excavatorsTmp != null)
			{
				for (int i = 0; i < excavator.SpareParts.Count; i++)
				{
					excavator.SpareParts[i].Excavators = excavatorsTmp[i];
				}
			}

			return excavator;
		}

		public async Task<ExcavatorPhoto?> SaveExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto)
		{
			using var context = factory.CreateDbContext();
			ExcavatorPhoto? currentExcavatorPhoto;

			if (excavatorPhoto.Id == 0)
			{
				context.Excavators.Attach(excavatorPhoto.Excavator);

				context.Add(excavatorPhoto);
			}
			else
			{
				currentExcavatorPhoto = await context.ExcavatorPhotos
					.FirstOrDefaultAsync(ep => ep.Id == excavatorPhoto.Id);
				if (currentExcavatorPhoto == null)
				{
					return null;
				}

				//await SaveExcavatorAsync(excavatorPhoto.Excavator);
				currentExcavatorPhoto.Excavator = excavatorPhoto.Excavator;
				//var excavatorId = excavatorPhoto.Excavator.Id;
				//currentExcavatorPhoto.Excavator = await context.Excavators
				//	.FirstAsync(e => e.Id == excavatorId);

				currentExcavatorPhoto.Photo = excavatorPhoto.Photo;
				currentExcavatorPhoto.IsTitle = excavatorPhoto.IsTitle;
			}

			await context.SaveChangesAsync();

			return excavatorPhoto;
		}

		public async Task<ExcavatorType> SaveExcavatorTypeAsync(ExcavatorType excavatorType)
		{
			using var context = factory.CreateDbContext();
			ExcavatorType currentExcavatorType;

			if (excavatorType.Id == 0)
			{
				context.AttachRange(excavatorType.PropertyTypes);

				context.Add(excavatorType);
			}
			else
			{
				currentExcavatorType = await context.ExcavatorTypes
					.Include(et => et.PropertyTypes)
					.Include(et => et.ExcavatorsOfThisType)
					.FirstAsync(et => et.Id == excavatorType.Id);

				await UpdateExcavatorTypeDataAsync(context, currentExcavatorType, excavatorType);
			}

			await context.SaveChangesAsync();

			return excavatorType;
		}

		public async Task<ExcavatorPropertyType> SaveExcavatorPropertyTypeAsync(ExcavatorPropertyType excavatorPropertyType)
		{
			using var context = factory.CreateDbContext();
			ExcavatorPropertyType currentExcavatorPropertyType;

			if (excavatorPropertyType.InputType == InputType.Unset)
			{// defensive programming... we don't want InputType.Unset in db
				throw new Exception($"Tried to save instance of '{nameof(ExcavatorPropertyType)}' with '{InputType.Unset}'.");
			}

			if (excavatorPropertyType.Id == 0)
			{
				context.Add(excavatorPropertyType);
			}
			else
			{
				currentExcavatorPropertyType = await context.ExcavatorPropertyTypes
					.FirstAsync(ept => ept.Id == excavatorPropertyType.Id);

				UpdateExcavatorPropertyTypeData(currentExcavatorPropertyType, excavatorPropertyType);
			}

			await context.SaveChangesAsync();

			return excavatorPropertyType;
		}

		public async Task<ExcavatorProperty?> SaveExcavatorPropertyAsync(ExcavatorProperty excavatorProperty)
		{
			using var context = factory.CreateDbContext();
			ExcavatorProperty? currentExcavatorProperty;

			if (excavatorProperty.Id == 0)
			{
				context.Add(excavatorProperty);
			}
			else
			{
				currentExcavatorProperty = await context.ExcavatorProperties
					.FirstOrDefaultAsync(ep => ep.Id == excavatorProperty.Id);
				if (currentExcavatorProperty == null)
				{
					return null;
				}

				currentExcavatorProperty.Value = excavatorProperty.Value;
				currentExcavatorProperty.PropertyType = excavatorProperty.PropertyType;
			}

			await context.SaveChangesAsync();

			return excavatorProperty;
		}

		public async Task<SparePart> SaveSparePartAsync(SparePart sparePart)
		{
			using var context = factory.CreateDbContext();
			SparePart currentSparePart;

			if (sparePart.Id == 0)
			{
				context.AttachRange(sparePart.Excavators);

				context.Add(sparePart);
			}
			else
			{
				currentSparePart = await context.SpareParts
					.Include(sp => sp.Excavators)
					.FirstAsync(sp => sp.Id == sparePart.Id);

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


		public async Task<MainOffer?> SaveMainOfferAsync(MainOffer mainOffer)
		{
			using var context = factory.CreateDbContext();
			MainOffer? currentMainOffer;

			if (mainOffer.Id == 0)
			{
				context.ExcavatorTypes.Attach(mainOffer.ExcavatorType);

				context.Add(mainOffer);
			}
			else
			{
				currentMainOffer = await context.MainOffers
					//.Include(mo => mo.ExcavatorType)
					.FirstOrDefaultAsync(mo => mo.Id == mainOffer.Id);
				if (currentMainOffer == null)
				{
					return null;
				}

				var excavatorTypeTmp = await context.ExcavatorTypes
					.FirstOrDefaultAsync(et => et.Id == mainOffer.ExcavatorType.Id);
				if (excavatorTypeTmp == null)
				{
					throw new Exception(
						$"Could not save main offer because excavator type with id '{mainOffer.ExcavatorType.Id}' was not found."
						);
				}
				currentMainOffer.ExcavatorType = excavatorTypeTmp;

				currentMainOffer.Photo = mainOffer.Photo;
				currentMainOffer.Description = mainOffer.Description;
			}

			await context.SaveChangesAsync();

			return mainOffer;
		}


		public async Task<AdditionalEquipment?> SaveAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment)
		{
			using var context = factory.CreateDbContext();
			AdditionalEquipment? currentAdditionalEquipment;

			if (additionalEquipment.Id == 0)
			{
				context.Add(additionalEquipment);
			}
			else
			{
				currentAdditionalEquipment = await context.AdditionalEquipments
					.FirstOrDefaultAsync(ae => ae.Id == additionalEquipment.Id);
				if (currentAdditionalEquipment == null)
				{
					return null;
				}

				UpdateAdditionalEquipmentData(context, currentAdditionalEquipment, additionalEquipment);
			}

			await context.SaveChangesAsync();

			return additionalEquipment;
		}

		public async Task<AdditionalEquipmentPhoto?> SaveAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto)
		{
			using var context = factory.CreateDbContext();
			AdditionalEquipmentPhoto? currentAdditionalEquipmentPhoto;

			if (additionalEquipmentPhoto.Id == 0)
			{
				context.AdditionalEquipments.Attach(additionalEquipmentPhoto.AdditionalEquipment);

				context.Add(additionalEquipmentPhoto);
			}
			else
			{
				currentAdditionalEquipmentPhoto = await context.AdditionalEquipmentPhotos
					.Include(aep => aep.AdditionalEquipment)
					.FirstOrDefaultAsync(aep => aep.Id == additionalEquipmentPhoto.Id);
				if (currentAdditionalEquipmentPhoto == null)
				{
					return null;
				}

				currentAdditionalEquipmentPhoto.AdditionalEquipment = additionalEquipmentPhoto.AdditionalEquipment;
				//int additionalEquipmentId = additionalEquipmentPhoto.AdditionalEquipment.Id;
				//currentAdditionalEquipmentPhoto.AdditionalEquipment = await context.AdditionalEquipments
				//	.FirstAsync(ae => ae.Id == additionalEquipmentId);

				currentAdditionalEquipmentPhoto.Photo = additionalEquipmentPhoto.Photo;
				currentAdditionalEquipmentPhoto.IsTitle = additionalEquipmentPhoto.IsTitle;
			}

			await context.SaveChangesAsync();

			return additionalEquipmentPhoto;
		}


		public async Task<User?> SaveUserAsync(User user)
		{
			using var context = factory.CreateDbContext();
			User? currentUser;

			if (user.Id == 0)
			{
				context.Add(user);
			}
			else
			{
				currentUser = await context.Users
					.FirstOrDefaultAsync(c => c.Id == user.Id);
				if (currentUser == null)
				{
					return null;
				}

				UpdateUserData(currentUser, user);
			}

			await context.SaveChangesAsync();

			return user;
		}


		public async Task<AuctionOffer?> SaveAuctionOfferAsync(AuctionOffer auctionOffer)
		{
			using var context = factory.CreateDbContext();
			AuctionOffer? currentAuctionOffer;

			if (auctionOffer.Id == 0)
			{
				context.Add(auctionOffer);
			}
			else
			{
				currentAuctionOffer = await context.AuctionOffers
					.Include(ao => ao.Excavator)
					.FirstOrDefaultAsync(ao => ao.Id == auctionOffer.Id);
				if (currentAuctionOffer == null)
				{
					return null;
				}

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

		public async Task<AuctionBid?> SaveAuctionBidAsync(AuctionBid auctionBid)
		{
			using var context = factory.CreateDbContext();
			AuctionBid? currentAuctionBid;

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
				if (currentAuctionBid == null)
				{
					return null;
				}

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
			ExcavatorType? type = null
		)
		{
			using var context = factory.CreateDbContext();
			var brand = type?.Brand;
			var category = type?.Category;

			var query = context.Excavators
				.Include(e => e.Photos)
				.Include(e => e.Type)
				.Include(e => e.Properties)
				.ThenInclude(e => e.PropertyType)
				.Include(e => e.SpareParts)
				.Where(e => category != null ? e.Type.Category == category : true)
				.Where(e => brand != null ? e.Type.Brand == brand : true);

			var orderedQuery = query.OrderBy(e => e.Name)
				.Skip(startIndex ?? 0);

			if (numberOfExcavators != null)
			{
				orderedQuery = orderedQuery.Take((int)numberOfExcavators);
			}

			return await orderedQuery.AsNoTracking().ToListAsync();
		}

		public async Task<Excavator?> GetExcavatorAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.Excavators
				.Include(e => e.Photos)
				.Include(e => e.Properties)
				.ThenInclude(ep => ep.PropertyType)
				.Include(e => e.Type)
				.Include(e => e.SpareParts)
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task<int> GetExcavatorsCountAsync(ExcavatorType? type = null)
		{
			using var context = factory.CreateDbContext();

			if (type == null)
			{
				return await context.Excavators
					.CountAsync();
			}
			else
			{
				return await context.Excavators
					.Where(e => e.Type.Id == type.Id)
					.CountAsync();
			}
		}



		public async Task<List<ExcavatorPhoto>> GetExcavatorPhotosAsync(int excavatorId)
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPhotos
				.Where(ep => ep.Excavator.Id == excavatorId)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<int> GetExcavatorPhotosCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPhotos.CountAsync();
		}

		public async Task<List<ExcavatorType>> GetExcavatorTypesAsync(
			int? numberOfExcavatorTypes = null,
			int? startIndex = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.ExcavatorTypes
				.Include(et => et.PropertyTypes)
				.Include(et => et.ExcavatorsOfThisType)
				.ThenInclude(e => e.Properties)
				.Include(et => et.ExcavatorsOfThisType)
				.ThenInclude(e => e.Photos)
				.Skip(startIndex ?? 0);

			if (numberOfExcavatorTypes.HasValue)
			{
				query = query.Take(numberOfExcavatorTypes.Value);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetExcavatorTypesCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorTypes.CountAsync();
		}

		public async Task<List<ExcavatorPropertyType>> GetExcavatorPropertyTypesAsync(
			int? numberOfExcavatorPropertyTypes = null,
			int? startIndex = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.ExcavatorPropertyTypes
				.Include(ept => ept.ExcavatorTypesWithThisProperty)
				.Skip(startIndex ?? 0);

			if (numberOfExcavatorPropertyTypes.HasValue)
			{
				query = query.Take(numberOfExcavatorPropertyTypes.Value);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetExcavatorPropertyTypesCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPropertyTypes.CountAsync();
		}

		public async Task<ExcavatorType?> GetExcavatorTypeAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorTypes
				.AsNoTracking()
				.FirstOrDefaultAsync(et => et.Id == id);
		}

		public async Task<ExcavatorPhoto?> GetExcavatorTitlePhotoAsync(int excavatorId)
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPhotos
				.Include(ep => ep.Excavator)
				.Where(ep => ep.Excavator.Id == excavatorId)
				.AsNoTracking()
				.FirstOrDefaultAsync(ep => ep.IsTitle);
		}

		public async Task<List<SparePart>> GetSparePartsAsync(
			int? numberOfSpareParts = null,
			int? startIndex = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.SpareParts
				.Include(sp => sp.Excavators)
				.Skip(startIndex ?? 0);

			if (numberOfSpareParts.HasValue)
			{
				query = query.Take(numberOfSpareParts.Value);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<List<SparePart>> GetSparePartsAsync(int excavatorId)
		{
			using var context = factory.CreateDbContext();

			return await context.SpareParts
				.Include(sp => sp.Excavators)
				.Where(sp => sp.Excavators.Any(e => e.Id == excavatorId))
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<int> GetSparePartsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.SpareParts.CountAsync();
		}

		public async Task<SparePart?> GetSparePartAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.SpareParts
				.Include(sp => sp.Excavators)
				.AsNoTracking()
				.FirstOrDefaultAsync(sp => sp.Id == id);
		}


		public async Task<List<MainOffer>> GetMainOffersAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.MainOffers
				.Include(mo => mo.ExcavatorType)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<MainOffer?> GetMainOfferAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.MainOffers
				.Include(mo => mo.ExcavatorType)
				.AsNoTracking()
				.FirstOrDefaultAsync(mo => mo.Id == id);
		}


		public async Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(
			int? numberOfAdditionalEquipments = null,
			int? startIndex = null,
			string? excavatorCategory = null,
			string? category = null,
			string? brand = null
			)
		{
			using var context = factory.CreateDbContext();

			var query = context.AdditionalEquipments
				.Where(ae => excavatorCategory != null ? ae.ExcavatorCategory == excavatorCategory : true)
				.Where(ae => category != null ? ae.Category == category : true)
				.Where(ae => brand != null ? ae.Brand == brand : true);

			var orderedQuery = query.OrderBy(ae => ae.Name)
				.Skip(startIndex ?? 0);

			if (numberOfAdditionalEquipments.HasValue)
			{
				orderedQuery = orderedQuery.Take(numberOfAdditionalEquipments.Value);
			}

			return await orderedQuery
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<int> GetAdditionalEquipmentsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipments.CountAsync();
		}

		public async Task<AdditionalEquipment?> GetAdditionalEquipmentAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipments
				.AsNoTracking()
				.FirstOrDefaultAsync(ae => ae.Id == id);
		}

		public async Task<List<AdditionalEquipmentPhoto>> GetAdditionalEquipmentPhotosAsync(int additionalEquipmentId)
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentPhotos
				.Where(aep => aep.AdditionalEquipment.Id == additionalEquipmentId)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<int> GetAdditionalEquipmentPhotosCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentPhotos.CountAsync();
		}

		public async Task<AdditionalEquipmentPhoto?> GetAdditionalEquipmentTitlePhotoAsync(int additionalEquipmentId)
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentPhotos
				.Include(aep => aep.AdditionalEquipment)
				.Where(aep => aep.AdditionalEquipment.Id == additionalEquipmentId)
				.AsNoTracking()
				.FirstOrDefaultAsync(aep => aep.IsTitle);
		}


		public async Task<User?> GetUserAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<User?> GetUserAsync(string username)
		{
			using var context = factory.CreateDbContext();

			return await context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Username == username);
		}


		public async Task<List<AuctionOffer>> GetAuctionOffersAsync(int? numberOfAuctionOffers = null, int? startIndex = null)
		{
			using var context = factory.CreateDbContext();

			var query = context.AuctionOffers
				.Include(ao => ao.Excavator)
				.OrderBy(ao => ao.Excavator.Name)
				.Skip(startIndex ?? 0);

			if (numberOfAuctionOffers.HasValue)
			{
				query = query.Take(numberOfAuctionOffers.Value);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetAuctionOffersCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionOffers.CountAsync();
		}

		public async Task<AuctionOffer?> GetAuctionOfferAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionOffers
				.Include(ao => ao.Excavator)
				.AsNoTracking()
				.FirstOrDefaultAsync(ao => ao.Id == id);
		}

		public async Task<List<AuctionBid>> GetAuctionBidsAsync(int auctionOfferId)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids
				.Include(ab => ab.User)
				.Include(ab => ab.AuctionOffer)
				.Where(ab => ab.AuctionOffer.Id == auctionOfferId)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<int> GetAuctionBidsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids.CountAsync();
		}

		public async Task<AuctionBid?> GetAuctionBidAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids
				.Include(ab => ab.User)
				.Include(ab => ab.AuctionOffer)
				.AsNoTracking()
				.FirstOrDefaultAsync(ab => ab.Id == id);
		}

		// delete
		public async Task DeleteExcavatorAsync(Excavator excavator)
		{
			var properties = excavator.Properties;
			for (int i = properties.Count - 1; i >= 0; i--)
			{
				await DeleteExcavatorPropertyAsync(properties[i]);
			}
			properties.Clear();

			await DeleteItem(excavator);
		}

		public async Task DeleteExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto)
		{
			await DeleteItem(excavatorPhoto);
		}

		public async Task DeleteExcavatorTypeAsync(ExcavatorType excavatorType)
		{
			var excavatorsOfDeletingType = excavatorType.ExcavatorsOfThisType;
			/* For loop- better go in reverse because for some reason the items are removed also from the list
			 * not just from db and as the list is edited, we can easily go out of range...
			 * And because of this behaviour we don't really need to call .Clear(), but I'll leave it there just in case... */
			for (int i = excavatorsOfDeletingType.Count - 1; i >= 0; i--)
			{
				await DeleteExcavatorAsync(excavatorsOfDeletingType[i]);
			}
			excavatorsOfDeletingType.Clear();

			await DeleteItem(excavatorType);
		}

		public async Task DeleteExcavatorPropertyAsync(ExcavatorProperty excavatorProperty)
		{
			await DeleteItem(excavatorProperty);
		}

		public async Task DeleteExcavatorPropertyTypeAsync(ExcavatorPropertyType excavatorPropertyType)
		{
			await DeleteItem(excavatorPropertyType);
		}

		public async Task DeleteSparePartAsync(SparePart sparePart)
		{
			await DeleteItem(sparePart);
		}


		public async Task DeleteMainOfferAsync(MainOffer mainOffer)
		{
			await DeleteItem(mainOffer);
		}


		public async Task DeleteAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment)
		{
			await DeleteItem(additionalEquipment);
		}

		public async Task DeleteAdditionalEquipmentPhotoAsync(AdditionalEquipmentPhoto additionalEquipmentPhoto)
		{
			await DeleteItem(additionalEquipmentPhoto);
		}


		public async Task DeleteUserAsync(User user)
		{
			await DeleteItem(user);
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
		private void RemoveDeletedPhotos(IList<ExcavatorPhoto> currentPhotos, IList<ExcavatorPhoto> newPhotos)
		{
			for (int i = currentPhotos.Count - 1; i >= 0; i--)
			{
				var currentPhoto = currentPhotos[i];
				var isCurrentPhotoInNewPhotos = newPhotos.Select(newPhoto => newPhoto.Id == currentPhoto.Id).Any();
				if (!isCurrentPhotoInNewPhotos)
				{
					currentPhotos.Remove(currentPhoto);
				}
			}
		}

		private void AddNewlyAddedPhotos(IList<ExcavatorPhoto> currentPhotos, IList<ExcavatorPhoto> newPhotos)
		{
			var newPhotosCount = newPhotos.Count;
			for (int i = 0; i < newPhotosCount; i++)
			{
				var photo = newPhotos[i];
				if (photo.Id == 0)
				{
					currentPhotos.Add(photo);
				}
			}
		}

		private void UpdateExcavatorPhotos(Excavator currentExcavator, Excavator newExcavator)
		{
			var currentPhotos = currentExcavator.Photos;
			var newPhotos = newExcavator.Photos;

			RemoveDeletedPhotos(currentPhotos, newPhotos);

			AddNewlyAddedPhotos(currentPhotos, newPhotos);
		}

		private async Task UpdateExcavatorTypeAsync(ServISDbContext context, Excavator currentExcavator, Excavator newExcavator)
		{
			currentExcavator.Type = await context.ExcavatorTypes
				.FirstAsync(et => et.Id == newExcavator.Type.Id);
		}

		private void UpdateExcavatorProperties(Excavator currentExcavator, Excavator newExcavator)
		{
			if (currentExcavator.Type.Id == newExcavator.Type.Id)
			{
				var currentProperties = currentExcavator.Properties;
				foreach (var currentProperty in currentProperties)
				{
					/* we check ids of property types and not properties itselves because 
					 * when excavator has type A, user changes it to type B, then back to type A, 
					 * new properties are created with ids 0 */
					var newPropertyValue = newExcavator.Properties.First(p => p.PropertyType.Id == currentProperty.PropertyType.Id).Value;
					currentProperty.Value = newPropertyValue;
				}
			}
			else
			{
				currentExcavator.Properties.Clear();

				var newProperties = newExcavator.Properties;
				foreach (var newProperty in newProperties)
				{
					newProperty.PropertyType.ExcavatorTypesWithThisProperty = null!;
					currentExcavator.Properties.Add(newProperty);
				}
			}
		}

		private async Task UpdateExcavatorSparePartsAsync(ServISDbContext context, Excavator currentExcavator, Excavator newExcavator)
		{
			var sparePartsIds = newExcavator.SpareParts.Select(sp => sp.Id);
			currentExcavator.SpareParts = await context.SpareParts
				.Where(sp => sparePartsIds.Contains(sp.Id))
				.ToListAsync();
		}

		private async Task UpdateExcavatorDataAsync(ServISDbContext context, Excavator currentExcavator, Excavator newExcavator)
		{
			currentExcavator.Name = newExcavator.Name;
			currentExcavator.Description = newExcavator.Description;
			currentExcavator.IsForAuctionOnly = newExcavator.IsForAuctionOnly;

			UpdateExcavatorPhotos(currentExcavator, newExcavator);

			UpdateExcavatorProperties(currentExcavator, newExcavator);

			await UpdateExcavatorTypeAsync(context, currentExcavator, newExcavator);

			await UpdateExcavatorSparePartsAsync(context, currentExcavator, newExcavator);
		}

		private void UpdateExcavatorsByAddingProperty(List<Excavator> excavators, ExcavatorProperty property)
		{
			excavators.ForEach(excavator => excavator.Properties.Add(property));
		}

		private void AddNewlyCheckedPropertyTypes(
			IList<ExcavatorPropertyType> currentPropertyTypes,
			IList<ExcavatorPropertyType> newPropertyTypes,
			List<Excavator> excavatorsOfThisType
		)
		{
			var newPropertyTypesCount = newPropertyTypes.Count;
			for (int i = 0; i < newPropertyTypesCount; i++)
			{
				var newPropertyType = newPropertyTypes[i];

				var isNewPropertyTypeInCurrentPropertyTypes =
					currentPropertyTypes.Any(currentPropertyType => currentPropertyType.Id == newPropertyType.Id);

				if (!isNewPropertyTypeInCurrentPropertyTypes)
				{
					currentPropertyTypes.Add(newPropertyType);

					var newProperty = new ExcavatorProperty
					{
						PropertyType = newPropertyType,
						Value = ""
					};
					UpdateExcavatorsByAddingProperty(excavatorsOfThisType, newProperty);
				}
			}
		}

		private void UpdateExcavatorsByRemovingPropertyOfPropertyType(List<Excavator> excavators, ExcavatorPropertyType propertyType)
		{
			excavators.ForEach(excavator =>
			{
				var excavatorProperties = excavator.Properties;

				for (int i = excavatorProperties.Count - 1; i >= 0; i--)
				{
					var property = excavatorProperties[i];

					if (property.PropertyType.Id == propertyType.Id)
					{
						excavatorProperties.Remove(property);
						break;
					}
				}
			});
		}

		private void RemoveUncheckedPropertyTypes(
			IList<ExcavatorPropertyType> currentPropertyTypes,
			IList<ExcavatorPropertyType> newPropertyTypes,
			List<Excavator> excavatorsToBeUpdated
		)
		{
			for (int i = currentPropertyTypes.Count - 1; i >= 0; i--)
			{
				var currentPropertyType = currentPropertyTypes[i];

				var isCurrentPropertyTypeInNewPropertyTypes =
					newPropertyTypes.Any(newPropertyType => newPropertyType.Id == currentPropertyType.Id);

				if (!isCurrentPropertyTypeInNewPropertyTypes)
				{
					UpdateExcavatorsByRemovingPropertyOfPropertyType(excavatorsToBeUpdated, currentPropertyType);

					currentPropertyTypes.Remove(currentPropertyType);
				}
			}
		}

		private async Task UpdateExcavatorTypePropertyTypesAsync(ServISDbContext context, ExcavatorType currentExcavatorType, ExcavatorType newExcavatorType)
		{
			var currentPropertyTypes = currentExcavatorType.PropertyTypes;
			var newPropertyTypes = newExcavatorType.PropertyTypes;

			/* excavators of the updating type needs to be updated too...
			 * Here's why:
			 * Let's say we have excavator type T which has property types PT1, PT2. 
			 * Let's also assume we have excavator E of type T. This means E has properties P1, P2 of type PT1, PT2 respectively.
			 * When we change T in a way it has now property types PT1, PT3 (i.e. PT2 was deleted and PT3 was added),
			 * it means E should lost P2 and gain property P3 of type PT3. */
			var excavatorsOfUpdatingType = await context.Excavators
				.Include(e => e.Properties)
				.ThenInclude(ep => ep.PropertyType)
				.Where(e => e.Type.Id == currentExcavatorType.Id) // currentExcavatorType.Id == newExcavatorType.Id so it doesn't matter which one we use
				.ToListAsync();

			RemoveUncheckedPropertyTypes(currentPropertyTypes, newPropertyTypes, excavatorsOfUpdatingType);

			AddNewlyCheckedPropertyTypes(currentPropertyTypes, newPropertyTypes, excavatorsOfUpdatingType);
		}

		private async Task UpdateExcavatorTypeDataAsync(ServISDbContext context, ExcavatorType currentExcavatorType, ExcavatorType newExcavatorType)
		{
			currentExcavatorType.Brand = newExcavatorType.Brand;
			currentExcavatorType.Category = newExcavatorType.Category;

			await UpdateExcavatorTypePropertyTypesAsync(context, currentExcavatorType, newExcavatorType);
		}

		private void UpdateExcavatorPropertyTypeData(ExcavatorPropertyType currentExcavatorPropertyType, ExcavatorPropertyType newExcavatorPropertyType)
		{
			currentExcavatorPropertyType.Name = newExcavatorPropertyType.Name;
			currentExcavatorPropertyType.InputType = newExcavatorPropertyType.InputType;
		}

		private static void UpdateUserData(User currentUser, User newUser)
		{
			currentUser.Username = newUser.Username;
			currentUser.Password = newUser.Password;
			//currentUserData.Role = newUserData.Role;
			currentUser.Name = newUser.Name;
			currentUser.Surname = newUser.Surname;
			currentUser.PhoneNumber = newUser.PhoneNumber;
			currentUser.Email = newUser.Email;
			currentUser.Residence = newUser.Residence;
		}

		private static void UpdateAdditionalEquipmentData(ServISDbContext context, AdditionalEquipment currentAdditionalEquipment, AdditionalEquipment additionalEquipment)
		{
			currentAdditionalEquipment.ExcavatorCategory = additionalEquipment.ExcavatorCategory;
			currentAdditionalEquipment.Category = additionalEquipment.Category;
			currentAdditionalEquipment.Brand = additionalEquipment.Brand;
			currentAdditionalEquipment.Name = additionalEquipment.Name;
			currentAdditionalEquipment.Description = additionalEquipment.Description;

			var ids = additionalEquipment.Photos.Select(p => p.Id);
			currentAdditionalEquipment.Photos = context.AdditionalEquipmentPhotos
				.Where(aep => ids.Contains(aep.Id))
				.ToList();
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
