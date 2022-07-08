using ServISData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ServISData
{
	public class ServISDbContext : DbContext
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

		public ServISDbContext(DbContextOptions<ServISDbContext> options) : base(options)
		{

		}
	}

	public class ServISDbContextFactory : IDesignTimeDbContextFactory<ServISDbContext>
	{
		public static string GetConnectionString()
		{
			IConfiguration config = new ConfigurationBuilder()
				.AddUserSecrets("de01772f-834a-40d3-86af-a1dcae8ee4d4")
				.Build();

			return config.GetConnectionString("Default");
		}

		public ServISDbContext CreateDbContext(string[] args)
		{
			var connectionString = GetConnectionString();

			var optionsBuilder = new DbContextOptionsBuilder<ServISDbContext>();

			optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

			return new ServISDbContext(optionsBuilder.Options);
		}
	}
}
