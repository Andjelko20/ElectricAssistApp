using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class DeviceChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyInKwh",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "StandByKwh",
                table: "Devices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "EnergyInKwh",
                table: "Devices",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "StandByKwh",
                table: "Devices",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
