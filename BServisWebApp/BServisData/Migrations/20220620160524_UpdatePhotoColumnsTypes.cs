using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BServisData.Migrations
{
    public partial class UpdatePhotoColumnsTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "ExcavatorPhotos",
                type: "varbinary(50000)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(4000)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "AdditionalEquipmentPhotos",
                type: "varbinary(50000)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(4000)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "ExcavatorPhotos",
                type: "varbinary(4000)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(50000)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "AdditionalEquipmentPhotos",
                type: "varbinary(4000)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(50000)");
        }
    }
}
