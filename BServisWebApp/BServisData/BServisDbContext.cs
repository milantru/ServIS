using BServisData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace BServisData
{
	public class BServisDbContext : DbContext
	{
		public DbSet<AdditionalEquipment> AdditionalEquipments { get; set; } = null!;
		public DbSet<AdditionalEquipmentPhoto> AdditionalEquipmentPhotos { get; set; } = null!;
		public DbSet<Administrator> Administrators { get; set; } = null!;
		public DbSet<AuctionBid> AuctionBids { get; set; } = null!;
		public DbSet<AuctionOffer> AuctionOffers { get; set; } = null!;
		public DbSet<Customer> Customers { get; set; } = null!;
		public DbSet<Excavator> Excavators { get; set; } = null!;
		public DbSet<ExcavatorPhoto> ExcavatorPhotos { get; set; } = null!;
		public DbSet<SkidSteerLoader> SkidSteerLoaders { get; set; } = null!;
		public DbSet<SparePart> SpareParts { get; set; } = null!;
		public DbSet<TrackedExcavator> TrackedExcavators { get; set; } = null!;
		public DbSet<TrackedLoader> TrackedLoaders { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;

		public BServisDbContext(DbContextOptions<BServisDbContext> options) : base(options)
		{

		}
	}

	public class BServisDbContextFactory : IDesignTimeDbContextFactory<BServisDbContext>
	{
		public BServisDbContext CreateDbContext(string[] args)
		{
			IConfiguration config = new ConfigurationBuilder()
				.AddUserSecrets(Assembly.GetCallingAssembly())
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<BServisDbContext>();

			optionsBuilder.UseMySQL(config.GetConnectionString("Default"));

			return new BServisDbContext(optionsBuilder.Options);
		}
	}
}
