using Microsoft.EntityFrameworkCore.Migrations;

namespace ServISData.Migrations
{
    public partial class UpdateAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(17)",
                oldMaxLength: 17,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(254)",
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "StartingBid",
                table: "AuctionOffers",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Bid",
                table: "AuctionBids",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "AdditionalEquipments",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "varchar(17)",
                maxLength: 17,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(254)",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StartingBid",
                table: "AuctionOffers",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Bid",
                table: "AuctionBids",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "AdditionalEquipments",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");
        }
    }
}
