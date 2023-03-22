using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    Month = table.Column<float>(type: "REAL", nullable: false),
                    Year = table.Column<float>(type: "REAL", nullable: false),
                    Consumption = table.Column<float>(type: "REAL", nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => new { x.UserId, x.Month, x.Year });
                });

            migrationBuilder.CreateTable(
                name: "ChargingSchedulers",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "INTEGER", nullable: false),
                    Day = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<string>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingSchedulers", x => new { x.DeviceId, x.Day, x.Time });
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceBrands",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCategories", x => x.Id);
                });

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
                name: "DeviceEnergyUsages",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EnergyInKWh = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceEnergyUsages", x => new { x.DeviceId, x.StartTime, x.EndTime });
                });

            migrationBuilder.CreateTable(
                name: "DeviceModels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mark = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    DeviceCategoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    DeviceBrandId = table.Column<long>(type: "INTEGER", nullable: false),
                    DeviceTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    DeviceModelId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    EnergyInKwh = table.Column<float>(type: "REAL", nullable: false),
                    StandByKwh = table.Column<float>(type: "REAL", nullable: false),
                    Visibility = table.Column<bool>(type: "INTEGER", nullable: false),
                    Controlability = table.Column<bool>(type: "INTEGER", nullable: false),
                    TurnOn = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InclusionSchedulers",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "INTEGER", nullable: false),
                    Day = table.Column<string>(type: "TEXT", nullable: false),
                    TurnOn = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    TurnOff = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InclusionSchedulers", x => new { x.DeviceId, x.Day, x.TurnOn, x.TurnOff });
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
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settlements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlements", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "UserEnergyUsages",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Consumption = table.Column<float>(type: "REAL", nullable: true),
                    Production = table.Column<float>(type: "REAL", nullable: true),
                    EnergyStock = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEnergyUsages", x => new { x.UserId, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Blocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "ChargingSchedulers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "DeviceBrands");

            migrationBuilder.DropTable(
                name: "DeviceCategories");

            migrationBuilder.DropTable(
                name: "DeviceDefaultSettings");

            migrationBuilder.DropTable(
                name: "DeviceEnergyUsages");

            migrationBuilder.DropTable(
                name: "DeviceModels");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropTable(
                name: "InclusionSchedulers");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "Settlements");

            migrationBuilder.DropTable(
                name: "TypeBrandModels");

            migrationBuilder.DropTable(
                name: "TypeBrands");

            migrationBuilder.DropTable(
                name: "UserEnergyUsages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
