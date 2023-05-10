using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class NewMigrationEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceEnergyUsages",
                table: "DeviceEnergyUsages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "DeviceEnergyUsages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceEnergyUsages",
                table: "DeviceEnergyUsages",
                columns: new[] { "DeviceId", "StartTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceEnergyUsages",
                table: "DeviceEnergyUsages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "DeviceEnergyUsages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceEnergyUsages",
                table: "DeviceEnergyUsages",
                columns: new[] { "DeviceId", "StartTime", "EndTime" });
        }
    }
}
