using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace BServisData.Migrations
{
    public partial class InitialDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ForWhichExcavatorCategory = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    Category = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalEquipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Excavators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Model = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LastInspection = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsNew = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    HeightMm = table.Column<int>(type: "int", nullable: true),
                    LengthWithBucketMm = table.Column<int>(type: "int", nullable: true),
                    WidthWithBucketMm = table.Column<int>(type: "int", nullable: true),
                    WeightKg = table.Column<int>(type: "int", nullable: true),
                    NominalLoadCapacityKg = table.Column<int>(type: "int", nullable: true),
                    OverloadPointKg = table.Column<int>(type: "int", nullable: true),
                    TopSpeedKmh = table.Column<float>(type: "float", nullable: true),
                    TopSpeedKmhSpeedVersionMin = table.Column<float>(type: "float", nullable: true),
                    TopSpeedKmhSpeedVersionMax = table.Column<float>(type: "float", nullable: true),
                    IncreasedBucketVolumeM3 = table.Column<float>(type: "float", nullable: true),
                    TearingStrengthKn = table.Column<float>(type: "float", nullable: true),
                    TractionForceKn = table.Column<float>(type: "float", nullable: true),
                    TractionForceKnSpeedVersion = table.Column<float>(type: "float", nullable: true),
                    LiftingForceKn = table.Column<float>(type: "float", nullable: true),
                    ReachMm = table.Column<int>(type: "int", nullable: true),
                    MaximumDischargeHeightMm = table.Column<int>(type: "int", nullable: true),
                    EngineType = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true),
                    RatedPowerKw = table.Column<float>(type: "float", nullable: true),
                    DriveType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DriveControlHydrogenerator = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    VehicleHydraulicMotor = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    VehicleHydraulicMotorOperatingPressureMpa = table.Column<float>(type: "float", nullable: true),
                    ControlType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    OperatingControlPressureMpa = table.Column<float>(type: "float", nullable: true),
                    Control = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    WorkEquipmentHydrogenerator = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    WorkEquipmentSwitchboard = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    OperatingPressureMpa = table.Column<float>(type: "float", nullable: true),
                    OperatingHydraulicFlowLpm = table.Column<int>(type: "int", nullable: true),
                    BucketLeveling = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    AcousticNoisePowerDb = table.Column<int>(type: "int", nullable: true),
                    StandardTiresMin = table.Column<float>(type: "float", nullable: true),
                    StandardTiresMax = table.Column<float>(type: "float", nullable: true),
                    ElectricalInstallationV = table.Column<int>(type: "int", nullable: true),
                    OperatingWeightKg = table.Column<int>(type: "int", nullable: true),
                    ExcavationDepthMm = table.Column<int>(type: "int", nullable: true),
                    MaximumWidthMm = table.Column<int>(type: "int", nullable: true),
                    Engine = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true),
                    MaximumPowerKw = table.Column<float>(type: "float", nullable: true),
                    TearingStrengthKg = table.Column<int>(type: "int", nullable: true),
                    PenetrationForceKg = table.Column<int>(type: "int", nullable: true),
                    HydraulicFlowLpm = table.Column<float>(type: "float", nullable: true),
                    OperatingPressureBar = table.Column<int>(type: "int", nullable: true),
                    TrackedLoader_OperatingWeightKg = table.Column<int>(type: "int", nullable: true),
                    TiltingLoadKg = table.Column<int>(type: "int", nullable: true),
                    OperatingLoadCapacityIso14397Kg = table.Column<int>(type: "int", nullable: true),
                    StandardBucketVolumeM3 = table.Column<float>(type: "float", nullable: true),
                    TrackedLoader_Engine = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true),
                    TrackedLoader_MaximumPowerKw = table.Column<float>(type: "float", nullable: true),
                    TrackWidthMm = table.Column<int>(type: "int", nullable: true),
                    TrackedLoader_TractionForceKn = table.Column<float>(type: "float", nullable: true),
                    TrackedLoader_HydraulicFlowLpm = table.Column<float>(type: "float", nullable: true),
                    HydraulicFlowHiFlowLpm = table.Column<float>(type: "float", nullable: true),
                    MaximumOperatingPressureBar = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Excavators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CatalogNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true),
                    Surname = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: true),
                    Email = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: true),
                    Residence = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsTemporary = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalEquipmentPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AdditionalEquipmentId = table.Column<int>(type: "int", nullable: true),
                    Photo = table.Column<byte[]>(type: "varbinary(4000)", nullable: false),
                    IsTitle = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalEquipmentPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalEquipmentPhotos_AdditionalEquipments_AdditionalEqu~",
                        column: x => x.AdditionalEquipmentId,
                        principalTable: "AdditionalEquipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuctionOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ExcavatorId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    OfferEnd = table.Column<DateTime>(type: "datetime", nullable: false),
                    StartingBid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionOffers_Excavators_ExcavatorId",
                        column: x => x.ExcavatorId,
                        principalTable: "Excavators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcavatorPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ExcavatorId = table.Column<int>(type: "int", nullable: true),
                    Photo = table.Column<byte[]>(type: "varbinary(4000)", nullable: false),
                    IsTitle = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcavatorPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcavatorPhotos_Excavators_ExcavatorId",
                        column: x => x.ExcavatorId,
                        principalTable: "Excavators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcavatorSparePart",
                columns: table => new
                {
                    ExcavatorsId = table.Column<int>(type: "int", nullable: false),
                    SparePartsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcavatorSparePart", x => new { x.ExcavatorsId, x.SparePartsId });
                    table.ForeignKey(
                        name: "FK_ExcavatorSparePart_Excavators_ExcavatorsId",
                        column: x => x.ExcavatorsId,
                        principalTable: "Excavators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcavatorSparePart_SpareParts_SparePartsId",
                        column: x => x.SparePartsId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctionBidders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    AuctionOfferId = table.Column<int>(type: "int", nullable: true),
                    Bid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionBidders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionBidders_AuctionOffers_AuctionOfferId",
                        column: x => x.AuctionOfferId,
                        principalTable: "AuctionOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuctionBidders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalEquipmentPhotos_AdditionalEquipmentId",
                table: "AdditionalEquipmentPhotos",
                column: "AdditionalEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBidders_AuctionOfferId",
                table: "AuctionBidders",
                column: "AuctionOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBidders_UserId",
                table: "AuctionBidders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionOffers_ExcavatorId",
                table: "AuctionOffers",
                column: "ExcavatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcavatorPhotos_ExcavatorId",
                table: "ExcavatorPhotos",
                column: "ExcavatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcavatorSparePart_SparePartsId",
                table: "ExcavatorSparePart",
                column: "SparePartsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalEquipmentPhotos");

            migrationBuilder.DropTable(
                name: "AuctionBidders");

            migrationBuilder.DropTable(
                name: "ExcavatorPhotos");

            migrationBuilder.DropTable(
                name: "ExcavatorSparePart");

            migrationBuilder.DropTable(
                name: "AdditionalEquipments");

            migrationBuilder.DropTable(
                name: "AuctionOffers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SpareParts");

            migrationBuilder.DropTable(
                name: "Excavators");
        }
    }
}
