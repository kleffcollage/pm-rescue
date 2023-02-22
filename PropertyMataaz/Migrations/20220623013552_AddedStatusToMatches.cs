using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyMataaz.Migrations
{
    public partial class AddedStatusToMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 23, 2, 35, 51, 835, DateTimeKind.Local).AddTicks(7320),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 23, 0, 28, 54, 962, DateTimeKind.Local).AddTicks(8890));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 23, 2, 35, 51, 828, DateTimeKind.Local).AddTicks(2290),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 23, 0, 28, 54, 955, DateTimeKind.Local).AddTicks(4640));

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "PropertyRequestMatches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRequestMatches_StatusId",
                table: "PropertyRequestMatches",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyRequestMatches_Statuses_StatusId",
                table: "PropertyRequestMatches",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyRequestMatches_Statuses_StatusId",
                table: "PropertyRequestMatches");

            migrationBuilder.DropIndex(
                name: "IX_PropertyRequestMatches_StatusId",
                table: "PropertyRequestMatches");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PropertyRequestMatches");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 23, 0, 28, 54, 962, DateTimeKind.Local).AddTicks(8890),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 23, 2, 35, 51, 835, DateTimeKind.Local).AddTicks(7320));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 23, 0, 28, 54, 955, DateTimeKind.Local).AddTicks(4640),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 23, 2, 35, 51, 828, DateTimeKind.Local).AddTicks(2290));
        }
    }
}
