using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyMataaz.Migrations
{
    public partial class AddedAgreeToTenancy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 24, 20, 27, 32, 882, DateTimeKind.Local).AddTicks(7550),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 2, 20, 21, 22, 13, 977, DateTimeKind.Local).AddTicks(2230));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 24, 20, 27, 32, 875, DateTimeKind.Local).AddTicks(7970),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 2, 20, 21, 22, 13, 963, DateTimeKind.Local).AddTicks(230));

            migrationBuilder.AddColumn<bool>(
                name: "Agreed",
                table: "Tenancies",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agreed",
                table: "Tenancies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 20, 21, 22, 13, 977, DateTimeKind.Local).AddTicks(2230),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 2, 24, 20, 27, 32, 882, DateTimeKind.Local).AddTicks(7550));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 20, 21, 22, 13, 963, DateTimeKind.Local).AddTicks(230),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 2, 24, 20, 27, 32, 875, DateTimeKind.Local).AddTicks(7970));
        }
    }
}
