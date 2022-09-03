using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServISData.Migrations
{
    public partial class AddTablesforBrandsAndCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "ExcavatorTypes");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "ExcavatorTypes");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "AdditionalEquipments");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "AdditionalEquipments");

            migrationBuilder.DropColumn(
                name: "ExcavatorCategory",
                table: "AdditionalEquipments");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "ExcavatorTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ExcavatorTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "AdditionalEquipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AdditionalEquipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExcavatorCategoryId",
                table: "AdditionalEquipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AdditionalEquipmentBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalEquipmentBrands", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdditionalEquipmentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalEquipmentCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExcavatorBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcavatorBrands", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExcavatorCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcavatorCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ExcavatorTypes_BrandId",
                table: "ExcavatorTypes",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcavatorTypes_CategoryId",
                table: "ExcavatorTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalEquipments_BrandId",
                table: "AdditionalEquipments",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalEquipments_CategoryId",
                table: "AdditionalEquipments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalEquipments_ExcavatorCategoryId",
                table: "AdditionalEquipments",
                column: "ExcavatorCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalEquipments_AdditionalEquipmentBrands_BrandId",
                table: "AdditionalEquipments",
                column: "BrandId",
                principalTable: "AdditionalEquipmentBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalEquipments_AdditionalEquipmentCategories_CategoryId",
                table: "AdditionalEquipments",
                column: "CategoryId",
                principalTable: "AdditionalEquipmentCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalEquipments_ExcavatorCategories_ExcavatorCategoryId",
                table: "AdditionalEquipments",
                column: "ExcavatorCategoryId",
                principalTable: "ExcavatorCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExcavatorTypes_ExcavatorBrands_BrandId",
                table: "ExcavatorTypes",
                column: "BrandId",
                principalTable: "ExcavatorBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExcavatorTypes_ExcavatorCategories_CategoryId",
                table: "ExcavatorTypes",
                column: "CategoryId",
                principalTable: "ExcavatorCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalEquipments_AdditionalEquipmentBrands_BrandId",
                table: "AdditionalEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalEquipments_AdditionalEquipmentCategories_CategoryId",
                table: "AdditionalEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalEquipments_ExcavatorCategories_ExcavatorCategoryId",
                table: "AdditionalEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExcavatorTypes_ExcavatorBrands_BrandId",
                table: "ExcavatorTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ExcavatorTypes_ExcavatorCategories_CategoryId",
                table: "ExcavatorTypes");

            migrationBuilder.DropTable(
                name: "AdditionalEquipmentBrands");

            migrationBuilder.DropTable(
                name: "AdditionalEquipmentCategories");

            migrationBuilder.DropTable(
                name: "ExcavatorBrands");

            migrationBuilder.DropTable(
                name: "ExcavatorCategories");

            migrationBuilder.DropIndex(
                name: "IX_ExcavatorTypes_BrandId",
                table: "ExcavatorTypes");

            migrationBuilder.DropIndex(
                name: "IX_ExcavatorTypes_CategoryId",
                table: "ExcavatorTypes");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalEquipments_BrandId",
                table: "AdditionalEquipments");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalEquipments_CategoryId",
                table: "AdditionalEquipments");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalEquipments_ExcavatorCategoryId",
                table: "AdditionalEquipments");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "ExcavatorTypes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ExcavatorTypes");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "AdditionalEquipments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AdditionalEquipments");

            migrationBuilder.DropColumn(
                name: "ExcavatorCategoryId",
                table: "AdditionalEquipments");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "ExcavatorTypes",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ExcavatorTypes",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "AdditionalEquipments",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "AdditionalEquipments",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExcavatorCategory",
                table: "AdditionalEquipments",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
