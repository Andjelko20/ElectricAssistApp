using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class DeviceFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceDefaultSettings");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "TypeBrandModels");

            migrationBuilder.DropTable(
                name: "TypeBrands");

            migrationBuilder.DropColumn(
                name: "DeviceBrandId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DeviceCategoryId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DeviceTypeId",
                table: "Devices");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "ResetPassword",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "DeviceBrandId",
                table: "DeviceModels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeviceTypeId",
                table: "DeviceModels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<float>(
                name: "EnergyKwh",
                table: "DeviceModels",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "StandByKwh",
                table: "DeviceModels",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_CityId",
                table: "Settlements",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTypes_CategoryId",
                table: "DeviceTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceModelId",
                table: "Devices",
                column: "DeviceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_DeviceBrandId",
                table: "DeviceModels",
                column: "DeviceBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_DeviceTypeId",
                table: "DeviceModels",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_UserId",
                table: "Bills",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargingSchedulers_Devices_DeviceId",
                table: "ChargingSchedulers",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceEnergyUsages_Devices_DeviceId",
                table: "DeviceEnergyUsages",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModels_DeviceBrands_DeviceBrandId",
                table: "DeviceModels",
                column: "DeviceBrandId",
                principalTable: "DeviceBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModels_DeviceTypes_DeviceTypeId",
                table: "DeviceModels",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceModels_DeviceModelId",
                table: "Devices",
                column: "DeviceModelId",
                principalTable: "DeviceModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_UserId",
                table: "Devices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceTypes_DeviceCategories_CategoryId",
                table: "DeviceTypes",
                column: "CategoryId",
                principalTable: "DeviceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InclusionSchedulers_Devices_DeviceId",
                table: "InclusionSchedulers",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResetPassword_Users_UserId",
                table: "ResetPassword",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Settlements_Cities_CityId",
                table: "Settlements",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEnergyUsages_Users_UserId",
                table: "UserEnergyUsages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_UserId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_ChargingSchedulers_Devices_DeviceId",
                table: "ChargingSchedulers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceEnergyUsages_Devices_DeviceId",
                table: "DeviceEnergyUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModels_DeviceBrands_DeviceBrandId",
                table: "DeviceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModels_DeviceTypes_DeviceTypeId",
                table: "DeviceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceModels_DeviceModelId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_UserId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceTypes_DeviceCategories_CategoryId",
                table: "DeviceTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_InclusionSchedulers_Devices_DeviceId",
                table: "InclusionSchedulers");

            migrationBuilder.DropForeignKey(
                name: "FK_ResetPassword_Users_UserId",
                table: "ResetPassword");

            migrationBuilder.DropForeignKey(
                name: "FK_Settlements_Cities_CityId",
                table: "Settlements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEnergyUsages_Users_UserId",
                table: "UserEnergyUsages");

            migrationBuilder.DropIndex(
                name: "IX_Settlements_CityId",
                table: "Settlements");

            migrationBuilder.DropIndex(
                name: "IX_DeviceTypes_CategoryId",
                table: "DeviceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceModelId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_UserId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_DeviceModels_DeviceBrandId",
                table: "DeviceModels");

            migrationBuilder.DropIndex(
                name: "IX_DeviceModels_DeviceTypeId",
                table: "DeviceModels");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "DeviceBrandId",
                table: "DeviceModels");

            migrationBuilder.DropColumn(
                name: "DeviceTypeId",
                table: "DeviceModels");

            migrationBuilder.DropColumn(
                name: "EnergyKwh",
                table: "DeviceModels");

            migrationBuilder.DropColumn(
                name: "StandByKwh",
                table: "DeviceModels");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ResetPassword",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "DeviceBrandId",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeviceCategoryId",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeviceTypeId",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "DeviceDefaultSettings",
                columns: table => new
                {
                    DeviceModelId = table.Column<long>(type: "INTEGER", nullable: false),
                    DeviceBrandId = table.Column<long>(type: "INTEGER", nullable: false),
                    DefaultKwh = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceDefaultSettings", x => new { x.DeviceModelId, x.DeviceBrandId });
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    PriceGreenZoneCheapPower = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.PriceGreenZoneCheapPower);
                });

            migrationBuilder.CreateTable(
                name: "TypeBrandModels",
                columns: table => new
                {
                    TypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    BrandId = table.Column<long>(type: "INTEGER", nullable: false),
                    ModelId = table.Column<long>(type: "INTEGER", nullable: false),
                    EnergyKwh = table.Column<float>(type: "REAL", nullable: false),
                    StandByKwh = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeBrandModels", x => new { x.TypeId, x.BrandId, x.ModelId });
                });

            migrationBuilder.CreateTable(
                name: "TypeBrands",
                columns: table => new
                {
                    TypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    BrandId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeBrands", x => new { x.TypeId, x.BrandId });
                });
        }
    }
}
