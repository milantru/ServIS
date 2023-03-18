using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServISData.Migrations
{
    public partial class AddIsEvaluatedColumnToAuctionOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEvaluated",
                table: "AuctionOffers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEvaluated",
                table: "AuctionOffers");
        }
    }
}
