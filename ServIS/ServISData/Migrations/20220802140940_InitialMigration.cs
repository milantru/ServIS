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
                    ExcavatorCategory = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalEquipments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExcavatorPropertyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InputType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcavatorPropertyTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExcavatorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcavatorTypes", x => x.Id);
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
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Surname = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Residence = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                    Photo = table.Column<byte[]>(type: "longblob", nullable: false),
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
                name: "ExcavatorPropertyTypeExcavatorType",
                columns: table => new
                {
                    ExcavatorTypesWithThisPropertyId = table.Column<int>(type: "int", nullable: false),
                    PropertyTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcavatorPropertyTypeExcavatorType", x => new { x.ExcavatorTypesWithThisPropertyId, x.PropertyTypesId });
                    table.ForeignKey(
                        name: "FK_ExcavatorPropertyTypeExcavatorType_ExcavatorPropertyTypes_Pr~",
                        column: x => x.PropertyTypesId,
                        principalTable: "ExcavatorPropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcavatorPropertyTypeExcavatorType_ExcavatorTypes_ExcavatorT~",
                        column: x => x.ExcavatorTypesWithThisPropertyId,
                        principalTable: "ExcavatorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Excavators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsForAuctionOnly = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Excavators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Excavators_ExcavatorTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ExcavatorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MainOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Photo = table.Column<byte[]>(type: "longblob", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExcavatorTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainOffers_ExcavatorTypes_ExcavatorTypeId",
                        column: x => x.ExcavatorTypeId,
                        principalTable: "ExcavatorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AcquiredExcavators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LastInspection = table.Column<DateOnly>(type: "date", nullable: false),
                    ExcavatorId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcquiredExcavators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcquiredExcavators_Excavators_ExcavatorId",
                        column: x => x.ExcavatorId,
                        principalTable: "Excavators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcquiredExcavators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                    Photo = table.Column<byte[]>(type: "longblob", nullable: false),
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
                name: "ExcavatorProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: false),
                    ExcavatorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcavatorProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcavatorProperties_ExcavatorPropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "ExcavatorPropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcavatorProperties_Excavators_ExcavatorId",
                        column: x => x.ExcavatorId,
                        principalTable: "Excavators",
                        principalColumn: "Id");
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
                name: "IX_AcquiredExcavators_ExcavatorId",
                table: "AcquiredExcavators",
                column: "ExcavatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AcquiredExcavators_UserId",
                table: "AcquiredExcavators",
                column: "UserId");

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
                name: "IX_ExcavatorProperties_ExcavatorId",
                table: "ExcavatorProperties",
                column: "ExcavatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcavatorProperties_PropertyTypeId",
                table: "ExcavatorProperties",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcavatorPropertyTypeExcavatorType_PropertyTypesId",
                table: "ExcavatorPropertyTypeExcavatorType",
                column: "PropertyTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Excavators_TypeId",
                table: "Excavators",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcavatorSparePart_SparePartsId",
                table: "ExcavatorSparePart",
                column: "SparePartsId");

            migrationBuilder.CreateIndex(
                name: "IX_MainOffers_ExcavatorTypeId",
                table: "MainOffers",
                column: "ExcavatorTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcquiredExcavators");

            migrationBuilder.DropTable(
                name: "AdditionalEquipmentPhotos");

            migrationBuilder.DropTable(
                name: "AuctionBids");

            migrationBuilder.DropTable(
                name: "ExcavatorPhotos");

            migrationBuilder.DropTable(
                name: "ExcavatorProperties");

            migrationBuilder.DropTable(
                name: "ExcavatorPropertyTypeExcavatorType");

            migrationBuilder.DropTable(
                name: "ExcavatorSparePart");

            migrationBuilder.DropTable(
                name: "MainOffers");

            migrationBuilder.DropTable(
                name: "AdditionalEquipments");

            migrationBuilder.DropTable(
                name: "AuctionOffers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ExcavatorPropertyTypes");

            migrationBuilder.DropTable(
                name: "SpareParts");

            migrationBuilder.DropTable(
                name: "Excavators");

            migrationBuilder.DropTable(
                name: "ExcavatorTypes");
        }
    }
}
