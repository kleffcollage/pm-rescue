using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyMataaz.Migrations
{
    public partial class AddAlternativePhoneNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 7, 21, 30, 7, 530, DateTimeKind.Local).AddTicks(8320),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 11, 22, 11, 52, 12, 62, DateTimeKind.Local).AddTicks(4170));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 7, 21, 30, 7, 518, DateTimeKind.Local).AddTicks(6320),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 11, 22, 11, 52, 12, 54, DateTimeKind.Local).AddTicks(6540));

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber1",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber1",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 22, 11, 52, 12, 62, DateTimeKind.Local).AddTicks(4170),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 12, 7, 21, 30, 7, 530, DateTimeKind.Local).AddTicks(8320));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 22, 11, 52, 12, 54, DateTimeKind.Local).AddTicks(6540),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 12, 7, 21, 30, 7, 518, DateTimeKind.Local).AddTicks(6320));
        }
    }
}
