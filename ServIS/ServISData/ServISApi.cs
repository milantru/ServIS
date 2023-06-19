using ServISData.Interfaces;
using ServISData.Models;
using Microsoft.EntityFrameworkCore;
using ServISData.DataOperations;
using Microsoft.Extensions.Logging;

namespace ServISData
{
	public class ServISApi : IServISApi
	{
		private readonly IDbContextFactory<ServISDbContext> factory;
		private readonly ILogger<ServISApi> logger;

		public ServISApi(IDbContextFactory<ServISDbContext> factory, ILogger<ServISApi> logger)
		{
			this.factory = factory;
			this.logger = logger;
		}

		// ---------- Create/Update ----------
		public async Task<Excavator> SaveExcavatorAsync(Excavator excavator)
		{
			using var context = factory.CreateDbContext();
			Excavator currentExcavator;

			if (excavator.Id == 0)
			{
				var props = new List<ExcavatorProperty>(excavator.Properties.Count);
				foreach (var prop in excavator.Properties)
				{
					props.Add(new ExcavatorProperty()
					{
						PropertyType = await context.ExcavatorPropertyTypes.FirstAsync(ept => ept.Id == prop.PropertyType.Id),
						Value = prop.Value
					});
				}

				currentExcavator = new Excavator()
				{
					Name = excavator.Name,
					Description = excavator.Description,
					IsForAuctionOnly = excavator.IsForAuctionOnly,
					Photos = excavator.Photos,
					Type = await context.ExcavatorTypes.FirstAsync(et => et.Id == excavator.Type.Id),
					Properties = props,
					SpareParts = await context.SpareParts.Where(sp => excavator.SpareParts.Contains(sp)).ToListAsync()
				};

				context.Add(currentExcavator);
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

			return excavator;
		}

		public async Task<ExcavatorBrand> SaveExcavatorBrandAsync(ExcavatorBrand excavatorBrand)
		{
			using var context = factory.CreateDbContext();
			ExcavatorBrand currentExcavatorBrand;

			if (excavatorBrand.Id == 0)
			{
				currentExcavatorBrand = new ExcavatorBrand()
				{
					Brand = excavatorBrand.Brand
				};

				context.Add(currentExcavatorBrand);
			}
			else
			{
				currentExcavatorBrand = await context.ExcavatorBrands
					.FirstAsync(eb => eb.Id == excavatorBrand.Id);

				currentExcavatorBrand.Brand = excavatorBrand.Brand;
			}

			await context.SaveChangesAsync();

			return excavatorBrand;
		}

		public async Task<ExcavatorCategory> SaveExcavatorCategoryAsync(ExcavatorCategory excavatorCategory)
		{
			using var context = factory.CreateDbContext();
			ExcavatorCategory currentExcavatorCategory;

			if (excavatorCategory.Id == 0)
			{
				currentExcavatorCategory = new ExcavatorCategory()
				{
					Category = excavatorCategory.Category
				};

				context.Add(currentExcavatorCategory);
			}
			else
			{
				currentExcavatorCategory = await context.ExcavatorCategories
					.FirstAsync(ec => ec.Id == excavatorCategory.Id);

				currentExcavatorCategory.Category = excavatorCategory.Category;
			}

			await context.SaveChangesAsync();

			return excavatorCategory;
		}

		public async Task<ExcavatorType> SaveExcavatorTypeAsync(ExcavatorType excavatorType)
		{
			using var context = factory.CreateDbContext();
			ExcavatorType currentExcavatorType;

			if (excavatorType.Id == 0)
			{
				currentExcavatorType = new ExcavatorType()
				{
					Brand = await context.ExcavatorBrands.FirstAsync(eb => eb.Id == excavatorType.Brand.Id),
					Category = await context.ExcavatorCategories.FirstAsync(ec => ec.Id == excavatorType.Category.Id),
					PropertyTypes = await context.ExcavatorPropertyTypes.Where(ept => excavatorType.PropertyTypes.Contains(ept)).ToArrayAsync()
				};

				context.Add(currentExcavatorType);
            }
			else
			{
				currentExcavatorType = await context.ExcavatorTypes
					.Include(et => et.Brand)
					.Include(et => et.Category)
					.Include(et => et.PropertyTypes)
					.Include(et => et.ExcavatorsOfThisType)
					.ThenInclude(e => e.Properties)
					.ThenInclude(ep => ep.PropertyType)
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
				logger.LogError($"Tried to save instance of '{nameof(ExcavatorPropertyType)}' with '{InputType.Unset}' " +
					$"(saving '{nameof(ExcavatorPropertyType)}' with id '{excavatorPropertyType.Id}' cancelled).");
				return excavatorPropertyType;
			}

			if (excavatorPropertyType.Id == 0)
			{
				currentExcavatorPropertyType = new ExcavatorPropertyType()
				{
					Name = excavatorPropertyType.Name,
					InputType = excavatorPropertyType.InputType
				};

				context.Add(currentExcavatorPropertyType);
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

		public async Task<SparePart> SaveSparePartAsync(SparePart sparePart)
		{
			using var context = factory.CreateDbContext();
			SparePart currentSparePart;

			if (sparePart.Id == 0)
			{
				currentSparePart = new SparePart()
				{
					Name = sparePart.Name,
					CatalogNumber = sparePart.CatalogNumber,
					Excavators = await context.Excavators.Where(e => sparePart.Excavators.Contains(e)).ToListAsync()
				};

				context.Add(currentSparePart);
			}
			else
			{
				currentSparePart = await context.SpareParts
					.Include(sp => sp.Excavators)
					.FirstAsync(sp => sp.Id == sparePart.Id);

				await UpdateSparePartDataAsync(context, currentSparePart, sparePart);
			}

			await context.SaveChangesAsync();

			return sparePart;
		}

		public async Task<MainOffer> SaveMainOfferAsync(MainOffer mainOffer)
		{
			using var context = factory.CreateDbContext();
			MainOffer currentMainOffer;

			if (mainOffer.Id == 0)
			{
				currentMainOffer = new MainOffer()
				{
					Photo = mainOffer.Photo,
					Description = mainOffer.Description,
					ExcavatorType = await context.ExcavatorTypes.FirstAsync(et => et.Id == mainOffer.ExcavatorType.Id)
				};

				context.Add(currentMainOffer);
			}
			else
			{
				currentMainOffer = await context.MainOffers
					.Include(mo => mo.ExcavatorType)
					.FirstAsync(mo => mo.Id == mainOffer.Id);

				await UpdateMainOfferDataAsync(context, currentMainOffer, mainOffer);
			}

			await context.SaveChangesAsync();

			return mainOffer;
		}

		public async Task<AdditionalEquipment> SaveAdditionalEquipmentAsync(AdditionalEquipment additionalEquipment)
		{
			using var context = factory.CreateDbContext();
			AdditionalEquipment currentAdditionalEquipment;

			if (additionalEquipment.Id == 0)
			{
				currentAdditionalEquipment = new AdditionalEquipment()
				{
					Name = additionalEquipment.Name,
					Description = additionalEquipment.Description,
					Photos = additionalEquipment.Photos,
					Brand = await context.AdditionalEquipmentBrands.FirstAsync(aeb => aeb.Id == additionalEquipment.Brand.Id),
					Category = await context.AdditionalEquipmentCategories.FirstAsync(aec => aec.Id == additionalEquipment.Category.Id),
					ExcavatorCategory = await context.ExcavatorCategories.FirstAsync(ec => ec.Id == additionalEquipment.ExcavatorCategory.Id)
				};

				context.Add(currentAdditionalEquipment);
			}
			else
			{
				currentAdditionalEquipment = await context.AdditionalEquipments
					.Include(ae => ae.Brand)
					.Include(ae => ae.Category)
					.Include(ae => ae.ExcavatorCategory)
					.Include(ae => ae.Photos)
					.FirstAsync(ae => ae.Id == additionalEquipment.Id);

				await UpdateAdditionalEquipmentDataAsync(context, currentAdditionalEquipment, additionalEquipment);
			}

			await context.SaveChangesAsync();

			return additionalEquipment;
		}

		public async Task<AdditionalEquipmentBrand> SaveAdditionalEquipmentBrandAsync(AdditionalEquipmentBrand additionalEquipmentBrand)
		{
			using var context = factory.CreateDbContext();
			AdditionalEquipmentBrand currentAdditionalEquipmentBrand;

			if (additionalEquipmentBrand.Id == 0)
			{
				context.Add(additionalEquipmentBrand);
			}
			else
			{
				currentAdditionalEquipmentBrand = await context.AdditionalEquipmentBrands
					.FirstAsync(aeb => aeb.Id == additionalEquipmentBrand.Id);

				currentAdditionalEquipmentBrand.Brand = additionalEquipmentBrand.Brand;
			}

			await context.SaveChangesAsync();

			return additionalEquipmentBrand;
		}

		public async Task<AdditionalEquipmentCategory> SaveAdditionalEquipmentCategoryAsync(AdditionalEquipmentCategory additionalEquipmentCategory)
		{
			using var context = factory.CreateDbContext();
			AdditionalEquipmentCategory currentAdditionalEquipmentCategory;

			if (additionalEquipmentCategory.Id == 0)
			{
				context.Add(additionalEquipmentCategory);
			}
			else
			{
				currentAdditionalEquipmentCategory = await context.AdditionalEquipmentCategories
					.FirstAsync(aec => aec.Id == additionalEquipmentCategory.Id);

				currentAdditionalEquipmentCategory.Category = additionalEquipmentCategory.Category;
			}

			await context.SaveChangesAsync();

			return additionalEquipmentCategory;
		}

		public async Task<User> SaveUserAsync(User user)
		{
			using var context = factory.CreateDbContext();
			User currentUser;

			if (user.Id == 0)
			{
				context.Add(user);
			}
			else
			{
				currentUser = await context.Users
					.FirstAsync(c => c.Id == user.Id);

				UpdateUserData(currentUser, user);
			}

			await context.SaveChangesAsync();

			return user;
		}

		public async Task<AuctionOffer> SaveAuctionOfferAsync(AuctionOffer auctionOffer)
		{
			using var context = factory.CreateDbContext();
			AuctionOffer currentAuctionOffer;

			if (auctionOffer.Id == 0)
			{
				currentAuctionOffer = new AuctionOffer()
				{
					Description = auctionOffer.Description,
					StartingBid = auctionOffer.StartingBid,
					OfferEnd = auctionOffer.OfferEnd,
					IsEvaluated = auctionOffer.IsEvaluated,
					Excavator = await context.Excavators.FirstAsync(e => e.Id == auctionOffer.Excavator.Id)
				};

				context.Add(currentAuctionOffer);
			}
			else
			{
				currentAuctionOffer = await context.AuctionOffers
					.Include(ao => ao.Excavator)
					.FirstAsync(ao => ao.Id == auctionOffer.Id);

				await UpdateAuctionOfferDataAsync(context, currentAuctionOffer, auctionOffer);
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
				var user = auctionBid.User.Id == 0 
					? auctionBid.User 
					: await context.Users.FirstAsync(u => u.Id == auctionBid.User.Id);

				currentAuctionBid = new AuctionBid()
				{
					Bid = auctionBid.Bid,
					AuctionOffer = await context.AuctionOffers.FirstAsync(ao => ao.Id == auctionBid.AuctionOffer.Id),
					User = user
				};

				context.Add(currentAuctionBid);
			}
			else
			{
				currentAuctionBid = await context.AuctionBids
					.Include(ab => ab.User)
					.Include(ab => ab.AuctionOffer)
					.FirstAsync(ab => ab.Id == auctionBid.Id);

				await UpdateAuctionBidDataAsync(context, currentAuctionBid, auctionBid);
			}

			await context.SaveChangesAsync();

			return auctionBid;
		}

		public async Task<AutogeneratedMessage> SaveAutogeneratedMessageAsync(AutogeneratedMessage autogeneratedMessage)
		{
			using var context = factory.CreateDbContext();
			AutogeneratedMessage currentAutogeneratedMessage;

			if (autogeneratedMessage.Id == 0)
			{
				context.Add(autogeneratedMessage);
			}
			else
			{
				currentAutogeneratedMessage = await context.AutogeneratedMessages
					.FirstAsync(am => am.Id == autogeneratedMessage.Id);

				UpdateAutogeneratedMessageData(currentAutogeneratedMessage, autogeneratedMessage);
			}

			await context.SaveChangesAsync();

			return autogeneratedMessage;
		}

		// ---------- Read ----------
		public async Task<List<Excavator>> GetExcavatorsAsync(
			IDataOperations<Excavator>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.Excavators
				.Include(e => e.Photos)
				.Include(e => e.Type)
				.ThenInclude(et => et.Brand)
				.Include(e => e.Type)
				.ThenInclude(et => et.Category)
				.Include(e => e.Properties)
				.ThenInclude(e => e.PropertyType)
				.Include(e => e.SpareParts)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);

			}

			// return query.AsNoTracking().ToListAsync(); // TODO
			await Task.CompletedTask;
			return query.AsNoTracking().ToList();
		}

		public async Task<Excavator> GetExcavatorAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.Excavators
				.Include(e => e.Photos)
				.Include(e => e.Type)
				.ThenInclude(et => et.Brand)
				.Include(e => e.Type)
				.ThenInclude(et => et.Category)
				.Include(e => e.Properties)
				.ThenInclude(e => e.PropertyType)
				.Include(e => e.SpareParts)
				.AsNoTracking()
				.FirstAsync(e => e.Id == id);
		}

		public async Task<int> GetExcavatorsCountAsync(ExcavatorType? type = null)
		{
			using var context = factory.CreateDbContext();

			if (type is null)
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

		public async Task<List<ExcavatorBrand>> GetExcavatorBrandsAsync(
			IDataOperations<ExcavatorBrand>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.ExcavatorBrands
				.Include(eb => eb.ExcavatorTypesOfThisBrand)
				.ThenInclude(e => e.ExcavatorsOfThisType)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetExcavatorBrandsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorBrands.CountAsync();
		}

		public async Task<List<ExcavatorCategory>> GetExcavatorCategoriesAsync(
			IDataOperations<ExcavatorCategory>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.ExcavatorCategories
				.Include(ec => ec.ExcavatorTypesOfThisCategory)
				.ThenInclude(e => e.ExcavatorsOfThisType)
				.Include(ec => ec.AdditionalEquipmentsOfThisCategory)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetExcavatorCategoriesCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorCategories.CountAsync();
		}

		public async Task<List<ExcavatorType>> GetExcavatorTypesAsync(
			IDataOperations<ExcavatorType>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.ExcavatorTypes
				.Include(et => et.Brand)
				.Include(et => et.Category)
				.Include(et => et.PropertyTypes)
				.Include(et => et.ExcavatorsOfThisType)
				.ThenInclude(e => e.Properties)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetExcavatorTypesCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorTypes.CountAsync();
		}

		public async Task<List<ExcavatorPropertyType>> GetExcavatorPropertyTypesAsync(
			IDataOperations<ExcavatorPropertyType>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.ExcavatorPropertyTypes
				.Include(ept => ept.ExcavatorTypesWithThisProperty)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetExcavatorPropertyTypesCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPropertyTypes.CountAsync();
		}

		public async Task<ExcavatorType> GetExcavatorTypeAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorTypes
				.Include(et => et.Brand)
				.Include(et => et.Category)
				.Include(et => et.PropertyTypes)
				.AsNoTracking()
				.FirstAsync(et => et.Id == id);
		}

		public async Task<ExcavatorPhoto> GetExcavatorTitlePhotoAsync(int excavatorId)
		{
			using var context = factory.CreateDbContext();

			return await context.ExcavatorPhotos
				.Include(ep => ep.Excavator)
				.Where(ep => ep.Excavator.Id == excavatorId)
				.AsNoTracking()
				.FirstAsync(ep => ep.IsTitle);
		}

		public async Task<List<SparePart>> GetSparePartsAsync(
			IDataOperations<SparePart>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.SpareParts
				.Include(sp => sp.Excavators)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
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

		public async Task<SparePart> GetSparePartAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.SpareParts
				.Include(sp => sp.Excavators)
				.AsNoTracking()
				.FirstAsync(sp => sp.Id == id);
		}

		public async Task<List<MainOffer>> GetMainOffersAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.MainOffers
				.Include(mo => mo.ExcavatorType)
				.ThenInclude(et => et.Brand)
				.Include(mo => mo.ExcavatorType)
				.ThenInclude(et => et.Category)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<MainOffer> GetMainOfferAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.MainOffers
				.Include(mo => mo.ExcavatorType)
				.ThenInclude(et => et.Brand)
				.Include(mo => mo.ExcavatorType)
				.ThenInclude(et => et.Category)
				.AsNoTracking()
				.FirstAsync(mo => mo.Id == id);
		}

		public async Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(
			IDataOperations<AdditionalEquipment>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.AdditionalEquipments
				.Include(ae => ae.Photos)
				.Include(ae => ae.ExcavatorCategory)
				.Include(ae => ae.Category)
				.Include(ae => ae.Brand)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
			}

			await Task.CompletedTask;
			return query.AsNoTracking().ToList();
			//return await query.AsNoTracking().ToListAsync(); // TODO
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
				.Include(ae => ae.Photos)
				.Include(ae => ae.ExcavatorCategory)
				.Include(ae => ae.Category)
				.Include(ae => ae.Brand)
				.AsNoTracking()
				.FirstAsync(ae => ae.Id == id);
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

		public async Task<AdditionalEquipmentPhoto> GetAdditionalEquipmentTitlePhotoAsync(int additionalEquipmentId)
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentPhotos
				.Include(aep => aep.AdditionalEquipment)
				.Where(aep => aep.AdditionalEquipment.Id == additionalEquipmentId)
				.AsNoTracking()
				.FirstAsync(aep => aep.IsTitle);
		}

		public async Task<List<AdditionalEquipmentBrand>> GetAdditionalEquipmentBrandsAsync(
			IDataOperations<AdditionalEquipmentBrand>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.AdditionalEquipmentBrands
				.Include(aeb => aeb.AdditionalEquipmentsOfThisBrand)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetAdditionalEquipmentBrandsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentBrands.CountAsync();
		}

		public async Task<List<AdditionalEquipmentCategory>> GetAdditionalEquipmentCategoriesAsync(
			IDataOperations<AdditionalEquipmentCategory>? dataOperations = null
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.AdditionalEquipmentCategories
				.Include(aec => aec.AdditionalEquipmentsOfThisCategory)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
			}

			return await query.AsNoTracking().ToListAsync();
		}

		public async Task<int> GetAdditionalEquipmentCategoriesCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AdditionalEquipmentCategories.CountAsync();
		}

		public async Task<User> GetUserAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.Users
				.AsNoTracking()
				.FirstAsync(c => c.Id == id);
		}

		public async Task<User?> GetUserAsync(string username)
		{
			using var context = factory.CreateDbContext();

			return await context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Username == username);
		}

		public async Task<List<AuctionOffer>> GetAuctionOffersAsync(
			IDataOperations<AuctionOffer>? dataOperations = null	
		)
		{
			using var context = factory.CreateDbContext();

			var query = context.AuctionOffers
				.Include(ao => ao.Excavator)
				.ThenInclude(e => e.Photos)
				.AsQueryable();

			if (dataOperations != null)
			{
				query = dataOperations.PerformDataOperations(query);
			}

			//return await query.AsNoTracking().ToListAsync(); // TODO
			await Task.CompletedTask;
			return query.AsNoTracking().ToList();
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
				.ThenInclude(e => e.Photos)
				.Include(ao => ao.Excavator)
				.ThenInclude(e => e.Properties)
				.ThenInclude(ep => ep.PropertyType)
				.Include(ao => ao.Excavator)
				.ThenInclude(e => e.Type)
				.ThenInclude(et => et.Category)
				.Include(ao => ao.Excavator)
				.ThenInclude(e => e.Type)
				.ThenInclude(et => et.Brand)
				.Include(ao => ao.Excavator)
				.ThenInclude(e => e.SpareParts)
				.AsNoTracking()
				.FirstAsync(ao => ao.Id == id);
		}

		public async Task<List<AuctionBid>> GetAuctionBidsAsync(int auctionOfferId)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids
				.Include(ab => ab.AuctionOffer)
				.Include(ab => ab.User)
				.Where(ab => ab.AuctionOffer.Id == auctionOfferId)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<AuctionBid> GetAuctionBidAsync(int id)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids
				.Include(ab => ab.AuctionOffer)
				.Include(ab => ab.User)
				.AsNoTracking()
				.FirstAsync(ab => ab.Id == id);
		}

		public async Task<int> GetAuctionBidsCountAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids.CountAsync();
		}

		public async Task<int> GetAuctionBiddersCountAsync(int auctionOfferId)
		{
			using var context = factory.CreateDbContext();

			await Task.CompletedTask;
			return context.AuctionBids
				.Include(ab => ab.AuctionOffer)
				.Include(ab => ab.User)
				.Where(ab => ab.AuctionOffer.Id == auctionOfferId)
				.AsEnumerable()
				.DistinctBy(ab => ab.User.Email)
				.Count();
		}

		public async Task<AuctionBid?> GetMaxAuctionBidAsync(int auctionOfferId)
		{
			using var context = factory.CreateDbContext();

			return await context.AuctionBids
				.Include(ab => ab.User)
				.Include(ab => ab.AuctionOffer)
				.ThenInclude(ao => ao.Excavator)
				.Where(ab => ab.AuctionOffer.Id == auctionOfferId)
				.OrderByDescending(ab => ab.Bid) // OrderByDescending is used because MaxBy didnt work for some reason
                .AsNoTracking()
				.FirstOrDefaultAsync();
		}

		public async Task<List<AuctionBid>> GetLostAuctionBidsAsync(int auctionOfferId)
		{
			var maxAuctionBid = await GetMaxAuctionBidAsync(auctionOfferId);

			using var context = factory.CreateDbContext();

			var queryTmp1 = context.AuctionBids
				.Include(ab => ab.AuctionOffer)
				.Include(ab => ab.User);

			IQueryable<AuctionBid> queryTmp2;
			if (maxAuctionBid is null)
			{
				queryTmp2 = queryTmp1.Where(ab => ab.AuctionOffer.Id == auctionOfferId);
			}
			else
			{
				queryTmp2 = queryTmp1.Where(ab => ab.AuctionOffer.Id == auctionOfferId && ab.Id != maxAuctionBid.Id);
			}

			return queryTmp2
				.OrderByDescending(ab => ab.Bid)
				.AsNoTracking()
				.AsEnumerable() // this has to come before DistinctBy, in the current version of ef core its translation to SQL is not yet supported
				.DistinctBy(ab => ab.User.Id)
				.ToList();
		}

		public async Task<AutogeneratedMessage> GetAutogeneratedMessageAsync(AutogeneratedMessage.For forWhom)
		{
			using var context = factory.CreateDbContext();

			return await context.AutogeneratedMessages
				.AsNoTracking()
				.FirstAsync(am => am.ForWhom == forWhom);
		}

		public async Task<List<AutogeneratedMessage>> GetAutogeneratedMessagesAsync()
		{
			using var context = factory.CreateDbContext();

			return await context.AutogeneratedMessages
				.AsNoTracking()
				.ToListAsync();
		}

		// ---------- Delete ----------
		public async Task DeleteExcavatorAsync(Excavator excavator)
		{
			await DeleteTemporaryUsersOfAuctionWithExcavatorAsync(excavator);

			var properties = excavator.Properties;
			for (int i = properties.Count - 1; i >= 0; i--)
			{
				await DeleteExcavatorPropertyAsync(properties.ElementAt(i));
			}
			excavator.Properties.Clear();

			await DeleteItem(excavator);
		}

		public async Task DeleteExcavatorPhotoAsync(ExcavatorPhoto excavatorPhoto)
		{
			excavatorPhoto.Excavator = null!;

			await DeleteItem(excavatorPhoto);
		}

		public async Task DeleteExcavatorBrandAsync(ExcavatorBrand excavatorBrand)
		{
			await DeleteItem(excavatorBrand);
		}

		public async Task DeleteExcavatorCategoryAsync(ExcavatorCategory excavatorCategory)
		{
			await DeleteItem(excavatorCategory);
		}

		public async Task DeleteExcavatorTypeAsync(ExcavatorType excavatorType)
		{
			var excavatorsOfDeletingType = excavatorType.ExcavatorsOfThisType;
			/* For loop- better go in reverse because for some reason the items are removed also from the list
			 * not just from db and as the list is edited, we can easily go out of range...
			 * And because of this behaviour we don't really need to call .Clear(), but I'll leave it there just in case... */
			for (int i = excavatorsOfDeletingType.Count - 1; i >= 0; i--)
			{
				await DeleteExcavatorAsync(excavatorsOfDeletingType.ElementAt(i));
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
			additionalEquipmentPhoto.AdditionalEquipment = null!;

			await DeleteItem(additionalEquipmentPhoto);
		}

		public async Task DeleteAdditionalEquipmentBrandAsync(AdditionalEquipmentBrand additionalEquipmentBrand)
		{
			await DeleteItem(additionalEquipmentBrand);
		}

		public async Task DeleteAdditionalEquipmentCategoryAsync(AdditionalEquipmentCategory additionalEquipmentCategory)
		{
			await DeleteItem(additionalEquipmentCategory);
		}

		public async Task DeleteUserAsync(User user)
		{
			await DeleteItem(user);
		}

		public async Task DeleteAuctionOfferAsync(AuctionOffer auctionOffer)
		{
			var bids = await GetAuctionBidsAsync(auctionOffer.Id);
			foreach (var bid in bids)
			{
				var user = bid.User;
				if (user.IsTemporary)
				{
					await DeleteUserAsync(user);
				}
			}

			await DeleteItem(auctionOffer);
		}

		public async Task DeleteAuctionBidAsync(AuctionBid auctionBid)
		{
			await DeleteItem(auctionBid);
		}

		public async Task DeleteAutogeneratedMessageAsync(AutogeneratedMessage autogeneratedMessage)
		{
			await DeleteItem(autogeneratedMessage);
		}

		// ---------- Private methods ----------
		private static void RemoveDeletedPhotos<PhotoType>(IList<PhotoType> currentPhotos, IList<PhotoType> newPhotos)
			where PhotoType : IPhoto, IItem
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

		private static void AddNewlyAddedPhotos<PhotoType>(IList<PhotoType> currentPhotos, IList<PhotoType> newPhotos)
			where PhotoType : IPhoto, IItem
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

		private static void UpdatePhotos<PhotoType>(IList<PhotoType> currentPhotos, IList<PhotoType> newPhotos)
			where PhotoType : IPhoto, IItem
		{
			RemoveDeletedPhotos(currentPhotos, newPhotos);

			AddNewlyAddedPhotos(currentPhotos, newPhotos);
		}

		private static async Task UpdateExcavatorTypeAsync(ServISDbContext context, Excavator currentExcavator, Excavator newExcavator)
		{
			currentExcavator.Type = await context.ExcavatorTypes
				.FirstAsync(et => et.Id == newExcavator.Type.Id);
		}

		private static async Task UpdateExcavatorPropertiesAsync(ServISDbContext context, Excavator currentExcavator, Excavator newExcavator)
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
					currentExcavator.Properties.Add(new ExcavatorProperty()
					{
						PropertyType = await context.ExcavatorPropertyTypes.FirstAsync(ept => ept.Id == newProperty.PropertyType.Id),
						Value = newProperty.Value
					});
				}
			}
		}

		private static async Task UpdateExcavatorSparePartsAsync(ServISDbContext context, Excavator currentExcavator, Excavator newExcavator)
		{
			foreach (var newSparePart in newExcavator.SpareParts)
			{
				var currSparePart = currentExcavator.SpareParts.FirstOrDefault(sp => sp.Id == newSparePart.Id);
				if (currSparePart is null)
				{
					currentExcavator.SpareParts.Add(await context.SpareParts.FirstAsync(sp => sp.Id == newSparePart.Id));
				}
			}

			for (int i = currentExcavator.SpareParts.Count - 1; i >= 0; i--)
			{
				var currSparePart = currentExcavator.SpareParts.ElementAt(i);

				var currSparePartIsInNewSpareParts = newExcavator.SpareParts.Any(sp => sp.Id == currSparePart.Id);
				if (!currSparePartIsInNewSpareParts)
				{
					currentExcavator.SpareParts.Remove(currSparePart);
				}
			}
		}

		private static async Task UpdateExcavatorDataAsync(ServISDbContext context, Excavator currentExcavator, Excavator newExcavator)
		{
			currentExcavator.Name = newExcavator.Name;
			currentExcavator.Description = newExcavator.Description;
			currentExcavator.IsForAuctionOnly = newExcavator.IsForAuctionOnly;

			UpdatePhotos(currentExcavator.Photos, newExcavator.Photos);

			await UpdateExcavatorPropertiesAsync(context, currentExcavator, newExcavator);

			await UpdateExcavatorTypeAsync(context, currentExcavator, newExcavator);

			await UpdateExcavatorSparePartsAsync(context, currentExcavator, newExcavator);
		}

		private static void UpdateExcavatorsByAddingProperty(ICollection<Excavator> excavators, ExcavatorProperty property)
		{
			foreach (var excavator in excavators)
			{
				excavator.Properties.Add(property);
			}
		}

		private static async Task AddNewlyCheckedPropertyTypesAsync(
			ServISDbContext context,
			ICollection<ExcavatorPropertyType> currentPropertyTypes,
			ICollection<ExcavatorPropertyType> newPropertyTypes,
			ICollection<Excavator> excavatorsOfThisType
		)
		{
			var propertyTypesForAddition = newPropertyTypes.Where(newPropertyType =>
			{
				var isNewPropertyTypeInCurrentPropertyTypes =
					currentPropertyTypes.Any(currentPropertyType => currentPropertyType.Id == newPropertyType.Id);

				return !isNewPropertyTypeInCurrentPropertyTypes;
			});

			foreach (var propertyType in propertyTypesForAddition)
			{
				var propTypeFromContext = await context.ExcavatorPropertyTypes.FirstAsync(ept => ept.Id == propertyType.Id);

				currentPropertyTypes.Add(propTypeFromContext);

				var newProperty = new ExcavatorProperty
				{
					PropertyType = propTypeFromContext,
					Value = ""
				};
				UpdateExcavatorsByAddingProperty(excavatorsOfThisType, newProperty);
			}
		}

		private static void UpdateExcavatorsByRemovingPropertyOfPropertyType(
			ICollection<Excavator> excavators,
			ExcavatorPropertyType propertyType
		)
		{
			foreach (var excavator in excavators)
			{
				var excavatorProperties = excavator.Properties;

				for (int i = excavatorProperties.Count - 1; i >= 0; i--)
				{
					var property = excavatorProperties.ElementAt(i);

					if (property.PropertyType.Id == propertyType.Id)
					{
						excavatorProperties.Remove(property);
						/* break here is an optimization, we assume that every excavator has properties of the unique type.
						 * In other words, we assume there cant be 2+ properties of the same property type. */
						break;
					}
				}
			}
		}

		private static void RemoveUncheckedPropertyTypes(
			ICollection<ExcavatorPropertyType> currentPropertyTypes,
			ICollection<ExcavatorPropertyType> newPropertyTypes,
			ICollection<Excavator> excavatorsToBeUpdated
		)
		{
			var propertyTypesForRemoval = currentPropertyTypes.Where(currentPropertyType =>
			{
				var isCurrentPropertyTypeInNewPropertyTypes =
					newPropertyTypes.Any(newPropertyType => newPropertyType.Id == currentPropertyType.Id);

				return !isCurrentPropertyTypeInNewPropertyTypes;
			});

			foreach (var propertyType in propertyTypesForRemoval)
			{
				UpdateExcavatorsByRemovingPropertyOfPropertyType(excavatorsToBeUpdated, propertyType);

				currentPropertyTypes.Remove(propertyType);
			}
		}

		private static async Task UpdateExcavatorTypePropertyTypesAsync(
			ServISDbContext context,
			ExcavatorType currentExcavatorType,
			ExcavatorType newExcavatorType
		)
		{
			var currentPropertyTypes = currentExcavatorType.PropertyTypes;
			var newPropertyTypes = newExcavatorType.PropertyTypes;

			/* excavators of the updating type needs to be updated too...
			 * Here's why:
			 * Let's say we have excavator type T which has property types PT1, PT2. 
			 * Let's also assume we have excavator E of type T. This means E has properties P1, P2 of type PT1, PT2 respectively.
			 * When we change T in a way it has now property types PT1, PT3 (i.e. PT2 was deleted and PT3 was added),
			 * it means E should lost P2 and gain property P3 of type PT3. */
			var excavatorsOfUpdatingType = currentExcavatorType.ExcavatorsOfThisType;

			RemoveUncheckedPropertyTypes(currentPropertyTypes, newPropertyTypes, excavatorsOfUpdatingType);

			await AddNewlyCheckedPropertyTypesAsync(context, currentPropertyTypes, newPropertyTypes, excavatorsOfUpdatingType);
		}

		private static async Task UpdateExcavatorTypeDataAsync(
			ServISDbContext context,
			ExcavatorType currentExcavatorType,
			ExcavatorType newExcavatorType
		)
		{
			currentExcavatorType.Brand = await context.ExcavatorBrands
				.FirstAsync(eb => eb.Id == newExcavatorType.Brand.Id);

			currentExcavatorType.Category = await context.ExcavatorCategories
				.FirstAsync(ec => ec.Id == newExcavatorType.Category.Id);

			await UpdateExcavatorTypePropertyTypesAsync(context, currentExcavatorType, newExcavatorType);
		}

		private static void UpdateExcavatorPropertyTypeData(
			ExcavatorPropertyType currentExcavatorPropertyType,
			ExcavatorPropertyType newExcavatorPropertyType
		)
		{
			currentExcavatorPropertyType.Name = newExcavatorPropertyType.Name;
			currentExcavatorPropertyType.InputType = newExcavatorPropertyType.InputType;
		}

		private static void RemoveUncheckedSpareParts(
			ICollection<Excavator> currentExcavators,
			ICollection<Excavator> newExcavators
		)
		{
			for (int i = currentExcavators.Count - 1; i >= 0; i--)
			{
				var currentExcavator = currentExcavators.ElementAt(i);

				var isCurrentExcavatorInNewExcavators =
					newExcavators.Any(newExcavator => newExcavator.Id == currentExcavator.Id);

				if (!isCurrentExcavatorInNewExcavators)
				{
					currentExcavators.Remove(currentExcavator);
				}
			}
		}

		private static async Task AddNewlyCheckedSparePartsAsync(
			ServISDbContext context,
			ICollection<Excavator> currentExcavators,
			ICollection<Excavator> newExcavators
		)
		{
			foreach (var newExcavator in newExcavators)
			{
				var isNewExcavatorInCurrentExcavators =
					currentExcavators.Any(currentExcavator => currentExcavator.Id == newExcavator.Id);

				if (!isNewExcavatorInCurrentExcavators)
				{
					currentExcavators.Add(await context.Excavators.FirstAsync(e => e.Id == newExcavator.Id));
				}
			}
		}

		private static async Task UpdateSparePartExcavatorsAsync(
			ServISDbContext context,
			ICollection<Excavator> currentExcavators,
			ICollection<Excavator> newExcavators
		)
		{
			RemoveUncheckedSpareParts(currentExcavators, newExcavators);

			await AddNewlyCheckedSparePartsAsync(context, currentExcavators, newExcavators);
		}

		private static async Task UpdateSparePartDataAsync(ServISDbContext context, SparePart currentSparePart, SparePart newSparePart)
		{
			currentSparePart.CatalogNumber = newSparePart.CatalogNumber;
			currentSparePart.Name = newSparePart.Name;

			await UpdateSparePartExcavatorsAsync(context, currentSparePart.Excavators, newSparePart.Excavators);
		}

		private static async Task UpdateMainOfferDataAsync(
			ServISDbContext context,
			MainOffer currentMainOffer,
			MainOffer newMainOffer
		)
		{
			currentMainOffer.Photo = newMainOffer.Photo;
			currentMainOffer.Description = newMainOffer.Description;

			currentMainOffer.ExcavatorType = await context.ExcavatorTypes
					.FirstAsync(et => et.Id == newMainOffer.ExcavatorType.Id);
		}

		private static void UpdateUserData(User currentUser, User newUser)
		{
			currentUser.Username = newUser.Username;
			currentUser.Password = newUser.Password;
			currentUser.Name = newUser.Name;
			currentUser.Surname = newUser.Surname;
			currentUser.PhoneNumber = newUser.PhoneNumber;
			currentUser.Email = newUser.Email;
			currentUser.Residence = newUser.Residence;
		}

		private static async Task UpdateAdditionalEquipmentDataAsync(
			ServISDbContext context,
			AdditionalEquipment currentAdditionalEquipment,
			AdditionalEquipment newAdditionalEquipment
		)
		{
			currentAdditionalEquipment.Name = newAdditionalEquipment.Name;
			currentAdditionalEquipment.Description = newAdditionalEquipment.Description;

			currentAdditionalEquipment.ExcavatorCategory = await context.ExcavatorCategories
				.FirstAsync(ec => ec.Id == newAdditionalEquipment.ExcavatorCategory.Id);

			currentAdditionalEquipment.Category = await context.AdditionalEquipmentCategories
				.FirstAsync(aec => aec.Id == newAdditionalEquipment.Category.Id);

			currentAdditionalEquipment.Brand = await context.AdditionalEquipmentBrands
				.FirstAsync(aeb => aeb.Id == newAdditionalEquipment.Brand.Id);

			UpdatePhotos(currentAdditionalEquipment.Photos, newAdditionalEquipment.Photos);
		}

		private static async Task UpdateAuctionOfferDataAsync(
			ServISDbContext context,
			AuctionOffer currentAuctionOffer,
			AuctionOffer newAuctionOffer
		)
		{
			currentAuctionOffer.Description = newAuctionOffer.Description;
			currentAuctionOffer.OfferEnd = newAuctionOffer.OfferEnd;
			currentAuctionOffer.StartingBid = newAuctionOffer.StartingBid;
			currentAuctionOffer.IsEvaluated = newAuctionOffer.IsEvaluated;

			currentAuctionOffer.Excavator = await context.Excavators
				.FirstAsync(e => e.Id == newAuctionOffer.Excavator.Id);
		}

		private static async Task UpdateAuctionBidDataAsync(
			ServISDbContext context,
			AuctionBid currentAuctionBid,
			AuctionBid newAuctionBid
		)
		{
			currentAuctionBid.Bid = newAuctionBid.Bid;

			currentAuctionBid.User = await context.Users
				.FirstAsync(u => u.Id == newAuctionBid.User.Id);

			currentAuctionBid.AuctionOffer = await context.AuctionOffers
				.FirstAsync(ao => ao.Id == newAuctionBid.AuctionOffer.Id);
		}

		private static void UpdateAutogeneratedMessageData(
			AutogeneratedMessage currentAutogeneratedMessage,
			AutogeneratedMessage newAutogeneratedMessage
		)
		{
			currentAutogeneratedMessage.Subject = newAutogeneratedMessage.Subject;
			currentAutogeneratedMessage.Message = newAutogeneratedMessage.Message;
			currentAutogeneratedMessage.ForWhom = newAutogeneratedMessage.ForWhom;
		}

		private async Task DeleteItem(IItem item)
		{
			using var context = factory.CreateDbContext();

			context.Remove(item);

			await context.SaveChangesAsync();
		}

		/// <summary>
		/// Deletes every temporary user who participated in auction in which excavator was auctioned.
		/// </summary>
		/// <param name="excavator"></param>
		private async Task DeleteTemporaryUsersOfAuctionWithExcavatorAsync(Excavator excavator)
		{
			using var context = factory.CreateDbContext();

			var auctionOffers = await context.AuctionOffers
				.Include(ao => ao.Excavator)
				.Where(ao => ao.Excavator.Id == excavator.Id)
				.ToListAsync();

			auctionOffers.ForEach(async ao =>
			{
				var bids = await GetAuctionBidsAsync(ao.Id);
				bids.ForEach(async bid =>
				{
					var user = bid.User;
					if (user.IsTemporary)
					{
						await DeleteUserAsync(user);
					}
				});
			});
		}
	}
}
