using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServISData.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdditionalEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ForWhichExcavatorCategory = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalEquipments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Excavators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Model = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsNew = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastInspection = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    EngineType = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RatedPowerKw = table.Column<float>(type: "float", nullable: true),
                    DriveType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DriveControlHydrogenerator = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VehicleHydraulicMotor = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VehicleHydraulicMotorOperatingPressureMpa = table.Column<float>(type: "float", nullable: true),
                    ControlType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperatingControlPressureMpa = table.Column<float>(type: "float", nullable: true),
                    Control = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkEquipmentHydrogenerator = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkEquipmentSwitchboard = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperatingPressureMpa = table.Column<float>(type: "float", nullable: true),
                    OperatingHydraulicFlowLpm = table.Column<int>(type: "int", nullable: true),
                    BucketLeveling = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AcousticNoisePowerDb = table.Column<int>(type: "int", nullable: true),
                    StandardTiresMin = table.Column<float>(type: "float", nullable: true),
                    StandardTiresMax = table.Column<float>(type: "float", nullable: true),
                    ElectricalInstallationV = table.Column<int>(type: "int", nullable: true),
                    OperatingWeightKg = table.Column<int>(type: "int", nullable: true),
                    ExcavationDepthMm = table.Column<int>(type: "int", nullable: true),
                    MaximumWidthMm = table.Column<int>(type: "int", nullable: true),
                    Engine = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaximumPowerKw = table.Column<float>(type: "float", nullable: true),
                    TearingStrengthKg = table.Column<int>(type: "int", nullable: true),
                    PenetrationForceKg = table.Column<int>(type: "int", nullable: true),
                    HydraulicFlowLpm = table.Column<float>(type: "float", nullable: true),
                    OperatingPressureBar = table.Column<int>(type: "int", nullable: true),
                    TrackedLoader_OperatingWeightKg = table.Column<int>(type: "int", nullable: true),
                    TiltingLoadKg = table.Column<int>(type: "int", nullable: true),
                    OperatingLoadCapacityIso14397Kg = table.Column<int>(type: "int", nullable: true),
                    StandardBucketVolumeM3 = table.Column<float>(type: "float", nullable: true),
                    TrackedLoader_Engine = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CatalogNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareParts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Surname = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Residence = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsTemporary = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdditionalEquipmentPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdditionalEquipmentId = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(50000)", maxLength: 50000, nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuctionOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExcavatorId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OfferEnd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartingBid = table.Column<decimal>(type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionOffers_Excavators_ExcavatorId",
                        column: x => x.ExcavatorId,
                        principalTable: "Excavators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExcavatorPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExcavatorId = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(50000)", maxLength: 50000, nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuctionBids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AuctionOfferId = table.Column<int>(type: "int", nullable: false),
                    Bid = table.Column<decimal>(type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionBids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionBids_AuctionOffers_AuctionOfferId",
                        column: x => x.AuctionOfferId,
                        principalTable: "AuctionOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionBids_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalEquipmentPhotos_AdditionalEquipmentId",
                table: "AdditionalEquipmentPhotos",
                column: "AdditionalEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBids_AuctionOfferId",
                table: "AuctionBids",
                column: "AuctionOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBids_UserId",
                table: "AuctionBids",
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
                name: "AuctionBids");

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
