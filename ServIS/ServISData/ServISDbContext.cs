﻿using ServISData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ServISData
{
    /// <summary>
    /// Represents the database context for the ServIS application.
    /// </summary>
    public class ServISDbContext : DbContext
	{
		public DbSet<AdditionalEquipment> AdditionalEquipments { get; set; } = null!;
		public DbSet<AdditionalEquipmentBrand> AdditionalEquipmentBrands { get; set; } = null!;
		public DbSet<AdditionalEquipmentCategory> AdditionalEquipmentCategories { get; set; } = null!;
		public DbSet<AdditionalEquipmentPhoto> AdditionalEquipmentPhotos { get; set; } = null!;
		public DbSet<AuctionBid> AuctionBids { get; set; } = null!;
		public DbSet<AuctionOffer> AuctionOffers { get; set; } = null!;
		public DbSet<AutogeneratedMessage> AutogeneratedMessages { get; set; } = null!;
		public DbSet<Excavator> Excavators { get; set; } = null!;
		public DbSet<ExcavatorBrand> ExcavatorBrands { get; set; } = null!;
		public DbSet<ExcavatorCategory> ExcavatorCategories { get; set; } = null!;
		public DbSet<ExcavatorPhoto> ExcavatorPhotos { get; set; } = null!;
		public DbSet<ExcavatorProperty> ExcavatorProperties { get; set; } = null!;
		public DbSet<ExcavatorPropertyType> ExcavatorPropertyTypes { get; set; } = null!;
		public DbSet<ExcavatorType> ExcavatorTypes { get; set; } = null!;
		public DbSet<MainOffer> MainOffers { get; set; } = null!;
		public DbSet<SparePart> SpareParts { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServISDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the database context.</param>
        public ServISDbContext(DbContextOptions<ServISDbContext> options) : base(options)
		{

		}

        /// <summary>
        /// Configures the model for the database context.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the database context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ExcavatorProperty>()
				.HasOne<Excavator>()
				.WithMany(e => e.Properties)
				.IsRequired();
		}
	}

    /// <summary>
    /// Factory for creating instances of the <see cref="ServISDbContext"/> class.
	/// <para>
	/// For more info on using context factory see: 
	/// <seealso href="https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#using-a-dbcontext-factory-eg-for-blazor"/>
	/// </para>
    /// </summary>
    public class ServISDbContextFactory : IDesignTimeDbContextFactory<ServISDbContext>
	{
        /// <summary>
        /// Retrieves the connection string for the ServIS database.
        /// </summary>
        /// <returns>The connection string for the database.</returns>
        public static string GetConnectionString()
		{
			IConfiguration config = new ConfigurationBuilder()
				.AddUserSecrets("de01772f-834a-40d3-86af-a1dcae8ee4d4")
				.Build();

			return config.GetConnectionString("Default");
		}

        /// <summary>
        /// Creates a new instance of the <see cref="ServISDbContext"/> class.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>An instance of the <see cref="ServISDbContext"/> class.</returns>
        public ServISDbContext CreateDbContext(string[] args)
		{
			var connectionString = GetConnectionString();

			var optionsBuilder = new DbContextOptionsBuilder<ServISDbContext>();

			optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

			return new ServISDbContext(optionsBuilder.Options);
		}
	}
}
