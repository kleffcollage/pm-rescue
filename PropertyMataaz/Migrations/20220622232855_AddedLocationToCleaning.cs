using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyMataaz.Migrations
{
    public partial class AddedLocationToCleaning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionTimes_InspectionDates_InspectionDateId",
                table: "InspectionTimes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 23, 0, 28, 54, 962, DateTimeKind.Local).AddTicks(8890),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 12, 9, 21, 49, 21, 278, DateTimeKind.Local).AddTicks(2680));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 23, 0, 28, 54, 955, DateTimeKind.Local).AddTicks(4640),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 12, 9, 21, 49, 21, 268, DateTimeKind.Local).AddTicks(6970));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Cleanings",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionTimes_InspectionDates_InspectionDateId",
                table: "InspectionTimes",
                column: "InspectionDateId",
                principalTable: "InspectionDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionTimes_InspectionDates_InspectionDateId",
                table: "InspectionTimes");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Cleanings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 9, 21, 49, 21, 278, DateTimeKind.Local).AddTicks(2680),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 23, 0, 28, 54, 962, DateTimeKind.Local).AddTicks(8890));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 9, 21, 49, 21, 268, DateTimeKind.Local).AddTicks(6970),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 23, 0, 28, 54, 955, DateTimeKind.Local).AddTicks(4640));

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionTimes_InspectionDates_InspectionDateId",
                table: "InspectionTimes",
                column: "InspectionDateId",
                principalTable: "InspectionDates",
                principalColumn: "Id");
        }
    }
}
