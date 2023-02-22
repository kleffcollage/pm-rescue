using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyMataaz.Migrations
{
    public partial class AddTenancyIdToMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 22, 11, 52, 12, 62, DateTimeKind.Local).AddTicks(4170),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 11, 13, 16, 51, 37, 101, DateTimeKind.Local).AddTicks(8250));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 22, 11, 52, 12, 54, DateTimeKind.Local).AddTicks(6540),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 11, 13, 16, 51, 37, 92, DateTimeKind.Local).AddTicks(2550));

            migrationBuilder.AddColumn<int>(
                name: "TenancyId",
                table: "Media",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_TenancyId",
                table: "Media",
                column: "TenancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Tenancies_TenancyId",
                table: "Media",
                column: "TenancyId",
                principalTable: "Tenancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Tenancies_TenancyId",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_TenancyId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "TenancyId",
                table: "Media");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 13, 16, 51, 37, 101, DateTimeKind.Local).AddTicks(8250),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 11, 22, 11, 52, 12, 62, DateTimeKind.Local).AddTicks(4170));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 13, 16, 51, 37, 92, DateTimeKind.Local).AddTicks(2550),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 11, 22, 11, 52, 12, 54, DateTimeKind.Local).AddTicks(6540));
        }
    }
}
