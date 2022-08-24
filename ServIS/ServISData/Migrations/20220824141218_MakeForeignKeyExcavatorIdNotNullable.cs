using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServISData.Migrations
{
    public partial class MakeForeignKeyExcavatorIdNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcavatorProperties_Excavators_ExcavatorId",
                table: "ExcavatorProperties");

            migrationBuilder.AlterColumn<int>(
                name: "ExcavatorId",
                table: "ExcavatorProperties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExcavatorProperties_Excavators_ExcavatorId",
                table: "ExcavatorProperties",
                column: "ExcavatorId",
                principalTable: "Excavators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcavatorProperties_Excavators_ExcavatorId",
                table: "ExcavatorProperties");

            migrationBuilder.AlterColumn<int>(
                name: "ExcavatorId",
                table: "ExcavatorProperties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ExcavatorProperties_Excavators_ExcavatorId",
                table: "ExcavatorProperties",
                column: "ExcavatorId",
                principalTable: "Excavators",
                principalColumn: "Id");
        }
    }
}
