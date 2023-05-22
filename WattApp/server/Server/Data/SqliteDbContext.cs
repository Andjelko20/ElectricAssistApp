using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Server.Utilities;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;

namespace Server.Data
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<ResetPasswordModel> ResetPassword { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<DeviceCategory> DeviceCategories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<DeviceBrand> DeviceBrands { get; set; }
        public DbSet<InclusionScheduler> InclusionSchedulers { get; set; }
        public DbSet<UserEnergyUsage> UserEnergyUsages { get; set; }
        public DbSet<DeviceEnergyUsage> DeviceEnergyUsages { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }

        public DbSet<ChangeEmailModel> ChangeEmailModels { get; set; }
        public DbSet<PendingUserModel> PendingUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<UserModel>()
           .HasOne(u => u.Role)
           .WithMany()
           .HasForeignKey(u => u.RoleId);*/
            //modelBuilder.Entity<ChargingScheduler>().HasKey(x => new { x.DeviceId, x.Day, x.Time });
            modelBuilder.Entity<InclusionScheduler>().HasKey(x => new { x.DeviceId, x.Day, x.TurnOn, x.TurnOff });
            modelBuilder.Entity<UserEnergyUsage>().HasKey(x => new { x.UserId, x.Date });
            modelBuilder.Entity<DeviceEnergyUsage>().HasKey(x => new { x.DeviceId, x.StartTime });
            //modelBuilder.Entity<DeviceDefaultSettings>().HasKey(x => new { x.DeviceModelId, x.DeviceBrandId });
            modelBuilder.Entity<Bill>().HasKey(x => new { x.UserId, x.Month, x.Year });
            //modelBuilder.Entity<TypeBrand>().HasKey(x => new { x.TypeId, x.BrandId });
            //modelBuilder.Entity<TypeBrandModel>().HasKey(x => new { x.TypeId, x.BrandId, x.ModelId });
        }

        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SqliteDbContext>();

                if (!context.Roles.Any())
                {
                    context.AddRange(new[]
                    {
                        new RoleModel()
                        {
                            Id=1,
                            Name="admin"
                        },
                        new RoleModel()
                        {
                            Id=2,
                            Name="dispatcher"
                        },
                        new RoleModel()
                        {
                            Id=3,
                            Name="prosumer"
                        },
                        new RoleModel()
                        {
                            Id=4,
                            Name="guest"
                        },
                        new RoleModel()
                        {
                            Id=5,
                            Name="superadmin"
                        },
                        new RoleModel()
                        {
                            Id=6,
                            Name="operater"
                        }

                    });
                    context.SaveChanges();
                }
                if (!context.Countries.Any())
                {
                    context.Countries.AddRange(new[]
                    {
                        new Country
                        {
                            Name = "Serbia"
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Cities.Any())
                {
                    context.Cities.AddRange(new[]
                    {
                        new City
                        {
                            CountryId = 1,
                            Name = "Kragujevac"
                        },
                        new City
                        {
                            CountryId = 1,
                            Name = "Beograd"
                        },
                        new City
                        {
                            CountryId = 1,
                            Name = "Novi Sad"
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Settlements.Any())
                {
                    context.Settlements.AddRange(new[]
                    {
                        new Settlement
                        {
                            CityId = 1,
                            Name = "Bubanj"
                        },
                        new Settlement
                        {
                            CityId = 1,
                            Name = "Aerodrom"
                        },
                        new Settlement
                        {
                            CityId = 1,
                            Name = "Bresnica"
                        },
                        new Settlement
                        {
                            CityId = 2,
                            Name = "Karaburma"
                        },
                        new Settlement
                        {
                            CityId = 3,
                            Name = "Stari grad"
                        },
                        new Settlement
                        {
                            CityId = 3,
                            Name = "Petrovoradin"

                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.AddRange(new[]
                    {
                        new UserModel() // 1
                        {
                            Name="Admin admin",
                            Email="matovicljubomir2002@gmail.com",
                            RoleId=1,
                            Blocked=false,
                            Username="admin",
                            Password=HashGenerator.Hash("admin"),
                            SettlementId=1,
                            Address="Jovanovac bb",
                            Latitude=44.1234567f,
                            Longitude=36.7890f
                        },
                        new UserModel() // 2
                        {
                            Name="User User",
                            Email="58-2020@pmf.kg.ac.rs",
                            RoleId=3,
                            Blocked=true,
                            Username="user",
                            Password=HashGenerator.Hash("user"),
                            SettlementId=2,
                            Address="Adresa",
                            Latitude=44.003966f,
                            Longitude=20.879002f
                        }, 
                        new UserModel() // 3
                        {
                            Name="DSO DSO",
                            Email="48-2020@pmf.kg.ac.rs",
                            RoleId = 2, 
                            Blocked=false,
                            Username="dsodso", 
                            Password=HashGenerator.Hash("dsodso"),
                            SettlementId=2,
                            Address="Adresa",
                            Latitude=44.01721187973962f,
                            Longitude=20.90732574462891f
                        },
                        new UserModel() // 4
                        {
                            Name="Prosumer",
                            Email="38-2020@pmf.kg.ac.rs",
                            RoleId=3,
                            Blocked=false,
                            Username="prosumer",
                            Password=HashGenerator.Hash("prosumer"),
                            SettlementId=2,
                            Address="Adresa",
                            Latitude=44.038430467535186f,
                            Longitude=20.895158806406904f
                        },
                        new UserModel() // 5
                        {
                            Name="Ivan Ivanovic",
                            Email="ivanivanovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="ivanivanovic",
                            Password=HashGenerator.Hash("ivan"),
                            SettlementId=2,
                            Address="Atinska 17",
                            Latitude=44.02913248753142f, 
                            Longitude=20.907281392946715f
                        },
                        new UserModel() // 6
                        {
                            Name="Marko Markovic",
                            Email="markomarkovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="markomarkovic",
                            Password=HashGenerator.Hash("marko"),
                            SettlementId=3,
                            Address="Zelengorska 23",
                            Latitude=44.000469475005666f,
                            Longitude=20.925654282072284f
                        },
                        new UserModel() // 7
                        {
                            Name="Lazar Lazarevic",
                            Email="lazarlazarevic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="lazarlazarevic",
                            Password=HashGenerator.Hash("lazar"),
                            SettlementId=4,
                            Address="Levskoga 35",
                            Latitude=44.81594385968978f,
                            Longitude=20.5029094730813f
                        },
                        new UserModel() // 8
                        {
                            Name="Ignjat Ignjatovic",
                            Email="ignjatignjatovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="ignjatignjatovic",
                            Password=HashGenerator.Hash("ignjat"),
                            SettlementId=5,
                            Address="Nusiceva 11",
                            Latitude=44.81424619608362f,
                            Longitude=20.462617995056533f
                        },
                        new UserModel() // 9
                        {
                            Name="Andjela Andjelkovic",
                            Email="andjelaandjelkovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="andjelaandjelkovic",
                            Password=HashGenerator.Hash("andjela"),
                            SettlementId=6,
                            Address="Preradoviceva 97",
                            Latitude=45.24334974630657f, 
                            Longitude=19.881933924749635f
                        },
                        new UserModel() // 10
                        {
                            Name="Ana Antonijevic",
                            Email="anaantonijevic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="anaantonijevic",
                            Password=HashGenerator.Hash("ana"),
                            SettlementId=6,
                            Address="Cajkovskog 1b",
                            Latitude=45.25361693169549f, 
                            Longitude=19.87324715206012f
                        },
                        new UserModel() // 11
                        {
                            Name="Jovana Jovanovic",
                            Email="jovanajovanovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="jovanajovanovic",
                            Password=HashGenerator.Hash("jovana"),
                            SettlementId=1,
                            Address="Svetozara Markovica 89/1",
                            Latitude=44.01722335983623f, 
                            Longitude=20.919819272342394f
                        },
                        new UserModel() // 12
                        {
                            Name="Mira Mirovic",
                            Email="miramirovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="miramirovic",
                            Password=HashGenerator.Hash("mira"),
                            SettlementId=1,
                            Address="Svetozara Markovica 89/1",
                            Latitude=44.01722335983623f,
                            Longitude=20.919819272342394f
                        },
                        new UserModel() // 13
                        {
                            Name="Jelena Jelenovic",
                            Email="jelenajelenovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="jelenajelenovic",
                            Password=HashGenerator.Hash("jelena"),
                            SettlementId=1,
                            Address="Svetozara Markovica 69",
                            Latitude=44.016616524325016f, 
                            Longitude=20.917608043704707f
                        },
                        new UserModel() // 14
                        {
                            Name="Helena Helenic",
                            Email="helenahelenic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="helenahelenic",
                            Password=HashGenerator.Hash("helena"),
                            SettlementId=2,
                            Address="Milorada Draskovica 29",
                            Latitude=44.03132386741314f, 
                            Longitude=20.900707469745335f
                        },
                        new UserModel() // 15
                        {
                            Name="Aleksa Aleksic",
                            Email="aleksaaleksic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="aleksaaleksic",
                            Password=HashGenerator.Hash("aleksa"),
                            SettlementId=2,
                            Address="Vladimira Rolovica 3",
                            Latitude=44.03012065118563f,
                            Longitude=20.913396772599704f
                        },
                        new UserModel() // 16
                        {
                            Name="Dragan Draganovic",
                            Email="dragandraganovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="dragandraganovic",
                            Password=HashGenerator.Hash("dragan"),
                            SettlementId=2,
                            Address="Svetogorska bb",
                            Latitude=44.02854729913197f,
                            Longitude=20.91172964625904f
                        },
                        new UserModel() // 17
                        {
                            Name="Djordje Djordjevic",
                            Email="djordjedjordjevic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="djordjedjordjevic",
                            Password=HashGenerator.Hash("djordje"),
                            SettlementId=3,
                            Address="Uzicke republike 19",
                            Latitude=43.997398258090335f, 
                            Longitude=20.94019882694245f
                        },
                        new UserModel() // 18
                        {
                            Name="Davic Davidovic",
                            Email="daviddavidovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="daviddavidovic",
                            Password=HashGenerator.Hash("david"),
                            SettlementId=3,
                            Address="Jovana Ristica 160",
                            Latitude=44.002200996713455f,
                            Longitude=20.9318937689748f
                        },
                        new UserModel() // 19
                        {
                            Name="Tamara Tamaric",
                            Email="tamaratamaric@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="tamaratamaric",
                            Password=HashGenerator.Hash("tamara"),
                            SettlementId=4,
                            Address="Mirijevski Bulevar 18b",
                            Latitude=44.81145073153164f,
                            Longitude=20.52185211109023f
                        },
                        new UserModel() // 20
                        {
                            Name="Tijana Tijanic",
                            Email="tijanatijanic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="tijanatijanic",
                            Password=HashGenerator.Hash("tijana"),
                            SettlementId=4,
                            Address="Hoze Martija 2b",
                            Latitude=44.809293624537624f,
                            Longitude=20.52066439095038f
                        },
                        new UserModel() // 21
                        {
                            Name="Marta Martic",
                            Email="martamartic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="martamartic",
                            Password=HashGenerator.Hash("marta"),
                            SettlementId=5,
                            Address="Brace Baruh 24",
                            Latitude=44.82818147708655f,
                            Longitude=20.45872611349181f
                        },
                        new UserModel() // 22
                        {
                            Name="Nikola Nikolic",
                            Email="nikolanikolic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="nikolanikolic",
                            Password=HashGenerator.Hash("nikola"),
                            SettlementId=5,
                            Address="Despota Djurdja 7",
                            Latitude=44.82656831500527f,
                            Longitude=20.459219639950664f
                        },
                        new UserModel() // 23
                        {
                            Name="Goran Goranovic",
                            Email="gorangoranovic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="gorangoranovic",
                            Password=HashGenerator.Hash("goran"),
                            SettlementId=6,
                            Address="Miseluk 92",
                            Latitude=45.23112665659611f,
                            Longitude=19.862177949005275f
                        },
                        new UserModel() // 24
                        {
                            Name="Jagoda Jagodic",
                            Email="jagodajagodic@gmail.com",
                            RoleId=3,
                            Blocked=false,
                            Username="jagodajagodic",
                            Password=HashGenerator.Hash("jagoda"),
                            SettlementId=6,
                            Address="Ribnjak donji put 89",
                            Latitude=45.23619484032214f,
                            Longitude=19.856997759771957f
                        },
                        new UserModel() // 25
                        {
                            Name="DSO NS",
                            Email="dso.ns@gmail.com",
                            RoleId = 2,
                            Blocked=false,
                            Username="dso.ns",
                            Password=HashGenerator.Hash("dso.ns"),
                            SettlementId=6,
                            Address="Bulevar oslobodjenja 96",
                            Latitude=45.24591602861516f,
                            Longitude=19.839801159727955f
                        },
                        new UserModel() // 26
                        {
                            Name="DSO BG",
                            Email="dso.bg@gmail.com",
                            RoleId = 2,
                            Blocked=false,
                            Username="dso.bg",
                            Password=HashGenerator.Hash("dso.bg"),
                            SettlementId=4,
                            Address="Masarikova 1-3",
                            Latitude=44.80751078632107f, 
                            Longitude=20.46303794922657f
                        },
                        new UserModel() // 27
                        {
                            Name="DSO KG",
                            Email="dso.kg@gmail.com",
                            RoleId = 2,
                            Blocked=false,
                            Username="dso.kg",
                            Password=HashGenerator.Hash("dso.kg"),
                            SettlementId=2,
                            Address="Slobode 7",
                            Latitude=44.02907224180809f,
                            Longitude=20.92016052263715f
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.DeviceCategories.Any())
                {
                    context.DeviceCategories.AddRange(new[]
                    {
                        new DeviceCategory()
                        {
                            Name = "Electricity Producer"
                        }, 
                        new DeviceCategory()
                        {
                            Name = "Electricity Consumer"
                        }, 
                        new DeviceCategory(){
                            Name = "Electricity Stock"
                        }
                    }
                    );
                    context.SaveChanges();
                }
                if (!context.DeviceTypes.Any())
                {
                    context.DeviceTypes.AddRange(new[]
                    {
                        new DeviceType() // 1
                        {
                            Name = "TV",
                            CategoryId = 2
                        },
                        new DeviceType() // 2
                        {
                            Name = "Freezer",
                            CategoryId = 2
                        },
                        new DeviceType() // 3
                        {
                            Name = "Boiler", 
                            CategoryId = 2
                        },
                        new DeviceType() // 4
                        {
                            Name = "Electric car", 
                            CategoryId = 2
                        },
                        new DeviceType() // 5
                        {
                            Name = "Solar panel",
                            CategoryId = 1
                        },
                        new DeviceType() // 6
                        {
                            Name = "Bateries",
                            CategoryId = 3
                        },
                        new DeviceType() // 7
                        {
                            Name = "Air conditioning",
                            CategoryId = 2
                        },
                        new DeviceType() // 8
                        {
                            Name = "Electric stove",
                            CategoryId = 2
                        },
                        new DeviceType() // 9
                        {
                            Name = "Electric bulb",
                            CategoryId = 2
                        },
                        new DeviceType() // 10
                        {
                            Name = "Washing machine",
                            CategoryId = 2
                        },
                        new DeviceType() // 11
                        {
                            Name = "Dishwasher",
                            CategoryId = 2
                        },
                        new DeviceType() // 12
                        {
                            Name = "Heater",
                            CategoryId = 2
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.DeviceBrands.Any())
                {
                    context.DeviceBrands.AddRange(new[]
                    {
                        new DeviceBrand() // 1
                        {
                            Name = "VOX"
                        },
                        new DeviceBrand() // 2
                        {
                            Name = "VIVAX"
                        },
                        new DeviceBrand() // 3
                        {
                            Name = "TESLA"
                        },
                        new DeviceBrand() // 4
                        {
                            Name = "SunPower"
                        },
                        new DeviceBrand() // 5
                        {
                            Name = "GREE"
                        },
                        new DeviceBrand() // 6
                        {
                            Name = "HISENSE"
                        },
                        new DeviceBrand() // 7
                        {
                            Name = "BEKO"
                        },
                        new DeviceBrand() // 8
                        {
                            Name = "Bosch"
                        },
                        new DeviceBrand() // 9
                        {
                            Name = "FOX"
                        },
                        new DeviceBrand() // 10
                        {
                            Name = "SAMSUNG"
                        },
                        new DeviceBrand() // 11
                        {
                            Name = "Philips"
                        },
                        new DeviceBrand() // 12
                        {
                            Name = "Union"
                        },
                        new DeviceBrand() // 13
                        {
                            Name = "CORDYS"
                        },
                        new DeviceBrand() // 14
                        {
                            Name = "Electrolux"
                        },
                        new DeviceBrand() // 15
                        {
                            Name = "Tristar"
                        },
                        new DeviceBrand() // 16
                        {
                            Name = "BMW"
                        },
                        new DeviceBrand() // 17
                        {
                            Name = "Volkswagen"
                        },
                        new DeviceBrand() // 18
                        {
                            Name = "Renault"
                        },
                        new DeviceBrand() // 19
                        {
                            Name = "Gorenje"
                        },
                        new DeviceBrand() //
                        {
                            Name = "Other"
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.DeviceModels.Any())
                {
                    context.DeviceModels.AddRange(new[]
                    {
                        new DeviceModel() // 1 - TV FOX
                        {
                            DeviceBrandId = 9,
                            DeviceTypeId = 1,
                            EnergyKwh = 0.05f,
                            StandByKwh = 0,
                            Mark = "43WOS620D"
                        },
                        new DeviceModel() // 2 - TV FOX
                        {
                            DeviceBrandId = 9,
                            DeviceTypeId = 1,
                            EnergyKwh = 0.064f,
                            StandByKwh = 0,
                            Mark = "50WOS620D"
                        },
                        new DeviceModel() // 3 - TV FOX
                        {
                            DeviceBrandId = 9,
                            DeviceTypeId = 1,
                            EnergyKwh =  0.026f,
                            StandByKwh = 0,
                            Mark = "32DTV220C"
                        },
                        new DeviceModel() // 4 - klima VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 7,
                            EnergyKwh = 1.1f,
                            StandByKwh = 0,
                            Mark = "VSA4-12BE"
                        },
                        new DeviceModel() // 5 - klima GREE
                        {
                            DeviceBrandId = 5,
                            DeviceTypeId = 7,
                            EnergyKwh = 1.8f,
                            StandByKwh = 0,
                            Mark = "PULAR 18K"
                        },
                        new DeviceModel() // 6 - klima GREE
                        {
                            DeviceBrandId = 5,
                            DeviceTypeId = 7,
                            EnergyKwh = 1.2f,
                            StandByKwh = 0,
                            Mark = "PULAR 12K"
                        },
                        new DeviceModel() // 7 - klima HISENSE
                        {
                            DeviceBrandId = 6,
                            DeviceTypeId = 7,
                            EnergyKwh = 1.1f,
                            StandByKwh = 0,
                            Mark = "Expert Smart 12K"
                        },
                        new DeviceModel() // 8 - klima HISENSE
                        {
                            DeviceBrandId = 6,
                            DeviceTypeId = 7,
                            EnergyKwh = 1.14f,
                            StandByKwh = 0,
                            Mark = "WINGS HINANO 12K"
                        },
                        new DeviceModel() // 9 - BEKO elektricni sporet
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 8,
                            EnergyKwh = 1.45f,
                            StandByKwh = 0,
                            Mark = "FSM 57300 GX"
                        },
                        new DeviceModel() // 10 - BEKO elektricni sporet
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 8,
                            EnergyKwh = 1.5f,
                            StandByKwh = 0,
                            Mark = "FSS56000W"
                        },
                        new DeviceModel() // 11 - BEKO elektricni sporet
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 8,
                            EnergyKwh = 1.45f,
                            StandByKwh = 0,
                            Mark = "FSE67310GX"
                        },
                        new DeviceModel() // 12 - VOX elektricni sporet
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 8,
                            EnergyKwh = 1.37f,
                            StandByKwh = 0,
                            Mark = "CHT6000W"
                        },
                        new DeviceModel() // 13 - VOX elektricni sporet
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 8,
                            EnergyKwh = 1.185f,
                            StandByKwh = 0,
                            Mark = "EHB 604 XL"
                        },
                        new DeviceModel() // 14 - Bosch elektricni sporet
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 8,
                            EnergyKwh = 1.705f,
                            StandByKwh = 0,
                            Mark = "HKR39A150"
                        },
                        new DeviceModel() // 15 - Bosch elektricni sporet
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 8,
                            EnergyKwh = 1.55f,
                            StandByKwh = 0,
                            Mark = "HKR39A120"
                        },
                        new DeviceModel() // 16 - TV VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 1,
                            EnergyKwh = 0.03f,
                            StandByKwh = 0,
                            Mark = "32A11H672B"
                        },
                        new DeviceModel() // 17 - TV VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 1,
                            EnergyKwh = 0.031f,
                            StandByKwh = 0,
                            Mark = "32A11H315FL"
                        },
                        new DeviceModel() // 18 - TV HISENSE
                        {
                            DeviceBrandId = 6,
                            DeviceTypeId = 1,
                            EnergyKwh =  0.13f,
                            StandByKwh = 0,
                            Mark = "50A6BG"
                        },
                        new DeviceModel() // 19 - TV HISENSE
                        {
                            DeviceBrandId = 6,
                            DeviceTypeId = 1,
                            EnergyKwh =  0.075f,
                            StandByKwh = 0,
                            Mark = "43A6BG"
                        },
                        new DeviceModel() // 20 - TV SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 1,
                            EnergyKwh =  0.07f,
                            StandByKwh = 0,
                            Mark = "UE43AU7172UXXH"
                        },
                        new DeviceModel() // 21 - TV SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 1,
                            EnergyKwh =  0.101f,
                            StandByKwh = 0,
                            Mark = "UE55AU7172UXXH"
                        },
                        new DeviceModel() // 22 - TV SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 1,
                            EnergyKwh =  0.071f,
                            StandByKwh = 0,
                            Mark = "UE50AU7022KXXH"
                        },
                        new DeviceModel() // 23 - Frizider VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 2,
                            EnergyKwh =  0.024f,
                            StandByKwh = 0,
                            Mark = "KG2500F"
                        },
                        new DeviceModel() // 24 - Frizider VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 2,
                            EnergyKwh =  0.022f,
                            StandByKwh = 0,
                            Mark = "KG2630F"
                        },
                        new DeviceModel() // 25 - Frizider SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 2,
                            EnergyKwh =  0.0256f,
                            StandByKwh = 0,
                            Mark = "RB34T652ESA/EK"
                        },
                        new DeviceModel() // 26 - Frizider SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 2,
                            EnergyKwh =  0.0256f,
                            StandByKwh = 0,
                            Mark = "RB34T652EB1/EK"
                        },
                        new DeviceModel() // 27 - Frizider SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 2,
                            EnergyKwh =  0.026f,
                            StandByKwh = 0,
                            Mark = "RB34T672FWW/EK"
                        },
                        new DeviceModel() // 28 - Frizider BEKO
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 2,
                            EnergyKwh =  0.026f,
                            StandByKwh = 0,
                            Mark = "RDSA240K30WN"
                        },
                        new DeviceModel() // 29 - Frizider BEKO
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 2,
                            EnergyKwh =  0.026f,
                            StandByKwh = 0,
                            Mark = "RCSA300K30SN"
                        },
                        new DeviceModel() // 30 - Bojler VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 3,
                            EnergyKwh =  0.162f,
                            StandByKwh = 0,
                            Mark = "WHD 802 CLEAN"
                        },
                        new DeviceModel() // 31 - Bojler VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 3,
                            EnergyKwh =  0.162f,
                            StandByKwh = 0,
                            Mark = "WHM 802 ECO"
                        },
                        new DeviceModel() // 32 - Bojler VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 3,
                            EnergyKwh =  0.162f,
                            StandByKwh = 0,
                            Mark = "WHF 8021"
                        },
                        new DeviceModel() // 33 - Bojler VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 3,
                            EnergyKwh =  0.2768f,
                            StandByKwh = 0,
                            Mark = "WHF 8021"
                        },
                        new DeviceModel() // 34 - Bojler BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 3,
                            EnergyKwh =  0.28f,
                            StandByKwh = 0,
                            Mark = "TR2000T 80 B"
                        },
                        new DeviceModel() // 35 - Bojler BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 3,
                            EnergyKwh =  0.28f,
                            StandByKwh = 0,
                            Mark = "TR4000 6 ET"
                        },
                        new DeviceModel() // 36 - Bojler BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 3,
                            EnergyKwh =  0.28f,
                            StandByKwh = 0,
                            Mark = "TR8500 15/18 DESOB"
                        },
                        new DeviceModel() // 37 - auto TESLA
                        {
                            DeviceBrandId = 3,
                            DeviceTypeId = 4,
                            EnergyKwh =  120f, // tesla super punjac
                            StandByKwh = 0,
                            Mark = "S 100D"
                        },
                        new DeviceModel() // 38 - auto TESLA
                        {
                            DeviceBrandId = 3,
                            DeviceTypeId = 4,
                            EnergyKwh =  50f, // brzi DC punjac
                            StandByKwh = 0,
                            Mark = "S 100D"
                        },
                        new DeviceModel() // 39 - auto TESLA
                        {
                            DeviceBrandId = 3,
                            DeviceTypeId = 4,
                            EnergyKwh =  120f, // tesla super punjac
                            StandByKwh = 0,
                            Mark = "Model 3"
                        },
                        new DeviceModel() // 40 - auto TESLA
                        {
                            DeviceBrandId = 3,
                            DeviceTypeId = 4,
                            EnergyKwh =  50f, // brzi DC punjac
                            StandByKwh = 0,
                            Mark = "Model 3"
                        },
                        new DeviceModel() // 41 - solarni panel
                        {
                            DeviceBrandId = 4,
                            DeviceTypeId = 5,
                            EnergyKwh = 0.4f,
                            StandByKwh = 0,
                            Mark = "Maxeon"
                        },
                        new DeviceModel() // 42 - solarni panel
                        {
                            DeviceBrandId = 4,
                            DeviceTypeId = 5,
                            EnergyKwh = 0.36f,
                            StandByKwh = 0,
                            Mark = "Performance"
                        },
                        new DeviceModel() // 43 - solarni panel
                        {
                            DeviceBrandId = 4,
                            DeviceTypeId = 5,
                            EnergyKwh = 0.33f,
                            StandByKwh = 0,
                            Mark = "AC"
                        },
                        new DeviceModel() // 44 - smart sijalica
                        {
                            DeviceBrandId = 11,
                            DeviceTypeId = 9,
                            EnergyKwh = 0.009f,
                            StandByKwh = 0,
                            Mark = "Hue Led Bulb WCA"
                        },
                        new DeviceModel() // 45 - smart sijalica
                        {
                            DeviceBrandId = 11,
                            DeviceTypeId = 9,
                            EnergyKwh = 0.01f,
                            StandByKwh = 0,
                            Mark = "Hue Starter kit E27"
                        },
                        new DeviceModel() // 46 - smart sijalica
                        {
                            DeviceBrandId = 11,
                            DeviceTypeId = 9,
                            EnergyKwh = 0.01f,
                            StandByKwh = 0,
                            Mark = "Hue White and Colour Ambiance 3/1"
                        },
                        new DeviceModel() // 47 - grejalica UNION
                        {
                            DeviceBrandId = 12,
                            DeviceTypeId = 12,
                            EnergyKwh = 0.75f,
                            StandByKwh = 0,
                            Mark = "CH-01WFT"
                        },
                        new DeviceModel() // 48 - grejalica UNION
                        {
                            DeviceBrandId = 12,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.0f,
                            StandByKwh = 0,
                            Mark = "PH-01WX"
                        },
                        new DeviceModel() // 49 - grejalica CORDYS
                        {
                            DeviceBrandId = 13,
                            DeviceTypeId = 12,
                            EnergyKwh = 0.8f,
                            StandByKwh = 0,
                            Mark = "CH-2001"
                        },
                        new DeviceModel() // 50 - grejalica CORDYS
                        {
                            DeviceBrandId = 13,
                            DeviceTypeId = 12,
                            EnergyKwh = 0.75f,
                            StandByKwh = 0,
                            Mark = "CH-2002"
                        },
                        new DeviceModel() // 51 - grejalica Electrolux
                        {
                            DeviceBrandId = 14,
                            DeviceTypeId = 12,
                            EnergyKwh = 0.5f,
                            StandByKwh = 0,
                            Mark = "ECH/T-1500 M EEC"
                        },
                        new DeviceModel() // 52 - grejalica Electrolux
                        {
                            DeviceBrandId = 14,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.0f,
                            StandByKwh = 0,
                            Mark = "ECH/T-2000 M EEC"
                        },
                        new DeviceModel() // 53 - grejalica Electrolux
                        {
                            DeviceBrandId = 14,
                            DeviceTypeId = 12,
                            EnergyKwh = 0.5f,
                            StandByKwh = 0,
                            Mark = "ECH/T-1500 E EEC"
                        },
                        new DeviceModel() // 54 - grejalica Electrolux
                        {
                            DeviceBrandId = 14,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.0f,
                            StandByKwh = 0,
                            Mark = "ECH/AG2-2000 3BE EEC"
                        },
                        new DeviceModel() // 55 - grejalica Electrolux
                        {
                            DeviceBrandId = 14,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.5f,
                            StandByKwh = 0,
                            Mark = "ECH/AG2-2500 3BE EEC"
                        },
                        new DeviceModel() // 56 - grejalica Tristar
                        {
                            DeviceBrandId = 15,
                            DeviceTypeId = 12,
                            EnergyKwh = 0.85f,
                            StandByKwh = 0,
                            Mark = "KA-5037"
                        },
                        new DeviceModel() // 57 - grejalica Tristar
                        {
                            DeviceBrandId = 15,
                            DeviceTypeId = 12,
                            EnergyKwh = 0.85f,
                            StandByKwh = 0,
                            Mark = "KA-5039"
                        },
                        new DeviceModel() // 58 - grejalica Bosch
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.5f,
                            StandByKwh = 0,
                            Mark = "HC 4000-25"
                        },
                        new DeviceModel() // 59 - grejalica Bosch
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.0f,
                            StandByKwh = 0,
                            Mark = "HC 4000-20"
                        },
                        new DeviceModel() // 60 - grejalica FOX
                        {
                            DeviceBrandId = 9,
                            DeviceTypeId = 12,
                            EnergyKwh = 0.9f,
                            StandByKwh = 0,
                            Mark = "GR-100"
                        },
                        new DeviceModel() // 61 - grejalica FOX
                        {
                            DeviceBrandId = 9,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.0f,
                            StandByKwh = 0,
                            Mark = "GR-110"
                        },
                        new DeviceModel() // 62 - grejalica VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.0f,
                            StandByKwh = 0,
                            Mark = "FH 59"
                        },
                        new DeviceModel() // 63 - grejalica VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 12,
                            EnergyKwh = 1.0f,
                            StandByKwh = 0,
                            Mark = "FH 75"
                        },
                        new DeviceModel() // 64 - ves masina SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.7f,
                            StandByKwh = 0,
                            Mark = "WW70T4040EE1/LE"
                        },
                        new DeviceModel() // 65 - ves masina SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.8f,
                            StandByKwh = 0,
                            Mark = "WW80T4020EE1/LE"
                        },
                        new DeviceModel() // 66 - ves masina SAMSUNG
                        {
                            DeviceBrandId = 10,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.9f,
                            StandByKwh = 0,
                            Mark = "WW90T4040CE1LE"
                        },
                        new DeviceModel() // 67 - ves masina VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.6f,
                            StandByKwh = 0,
                            Mark = "WM1070SYTD"
                        },
                        new DeviceModel() // 68 - ves masina VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.7f,
                            StandByKwh = 0,
                            Mark = "WM1060SYTD"
                        },
                        new DeviceModel() // 69 - ves masina VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.8f,
                            StandByKwh = 0,
                            Mark = "WM1285YTQD"
                        },
                        new DeviceModel() // 70 - ves masina BEKO
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.75f,
                            StandByKwh = 0,
                            Mark = "WUE 8622 XCW"
                        },
                        new DeviceModel() // 71 - ves masina BEKO
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.85f,
                            StandByKwh = 0,
                            Mark = "WUE 7511D XWW"
                        },
                        new DeviceModel() // 72 - ves masina BEKO
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.95f,
                            StandByKwh = 0,
                            Mark = "WTE 8511 X0"
                        },
                        new DeviceModel() // 73 - ves masina BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.95f,
                            StandByKwh = 0,
                            Mark = "WGG14403BY"
                        },
                        new DeviceModel() // 74 - ves masina BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.95f,
                            StandByKwh = 0,
                            Mark = "WGG14202BY"
                        },
                        new DeviceModel() // 75 - ves masina BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 10,
                            EnergyKwh = 0.95f,
                            StandByKwh = 0,
                            Mark = "WAN24265BY"
                        },
                        new DeviceModel() // 76 - masina za sudove BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.8f,
                            StandByKwh = 0,
                            Mark = "SMS4HVI33E"
                        },
                        new DeviceModel() // 77 - masina za sudove BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.85f,
                            StandByKwh = 0,
                            Mark = "SMS4HVW33E"
                        },
                        new DeviceModel() // 78 - masina za sudove BOSCH
                        {
                            DeviceBrandId = 8,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.9f,
                            StandByKwh = 0,
                            Mark = "SMS24AW02E"
                        },
                        new DeviceModel() // 79 - masina za sudove VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.9f,
                            StandByKwh = 0,
                            Mark = "LC10Y15CE"
                        },
                        new DeviceModel() // 80 - masina za sudove VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.85f,
                            StandByKwh = 0,
                            Mark = "LC12A1EDBE"
                        },
                        new DeviceModel() // 81 - masina za sudove VOX
                        {
                            DeviceBrandId = 1,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.9f,
                            StandByKwh = 0,
                            Mark = "LC12A15E"
                        },
                        new DeviceModel() // 82 - masina za sudove BEKO
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.9f,
                            StandByKwh = 0,
                            Mark = "DVS 05024 S"
                        },
                        new DeviceModel() // 83 - masina za sudove BEKO
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.8f,
                            StandByKwh = 0,
                            Mark = "DVN 05320 S"
                        },
                        new DeviceModel() // 84 - masina za sudove BEKO
                        {
                            DeviceBrandId = 7,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.85f,
                            StandByKwh = 0,
                            Mark = "BDFN 26430 X"
                        },
                        new DeviceModel() // 85 - masina za sudove GORENJE
                        {
                            DeviceBrandId = 19,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.9f,
                            StandByKwh = 0,
                            Mark = "GS 520E15 S"
                        },
                        new DeviceModel() // 86 - masina za sudove GORENJE
                        {
                            DeviceBrandId = 19,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.78f,
                            StandByKwh = 0,
                            Mark = "GS 520E15 W"
                        },
                        new DeviceModel() // 87 - masina za sudove GORENJE
                        {
                            DeviceBrandId = 19,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.82f,
                            StandByKwh = 0,
                            Mark = "GS 62040 S"
                        },
                        new DeviceModel() // 88 - masina za sudove GORENJE
                        {
                            DeviceBrandId = 19,
                            DeviceTypeId = 11,
                            EnergyKwh = 0.82f,
                            StandByKwh = 0,
                            Mark = "GS 62040 S"
                        },
                        new DeviceModel() // 89 - auto BMW
                        {
                            DeviceBrandId = 16,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "i3"
                        },
                        new DeviceModel() // 90 - auto BMW
                        {
                            DeviceBrandId = 16,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "i7"
                        },
                        new DeviceModel() // 91 - auto BMW
                        {
                            DeviceBrandId = 16,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "iX1"
                        },
                        new DeviceModel() // 92 - auto BMW
                        {
                            DeviceBrandId = 16,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "iX3"
                        },
                        new DeviceModel() // 93 - auto Volkswagen
                        {
                            DeviceBrandId = 17,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "ID.3"
                        },
                        new DeviceModel() // 94 - auto Volkswagen
                        {
                            DeviceBrandId = 17,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "ID.4"
                        },
                        new DeviceModel() // 95 - auto Volkswagen
                        {
                            DeviceBrandId = 17,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "eGolf 7"
                        },
                        new DeviceModel() // 96 - auto Renault
                        {
                            DeviceBrandId = 18,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "Zoe Intens R135 LP"
                        },
                        new DeviceModel() // 97 - auto Renault
                        {
                            DeviceBrandId = 18,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "Zoe Limited Q90"
                        },
                        new DeviceModel() // 98 - auto Renault
                        {
                            DeviceBrandId = 18,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "Zoe ZE"
                        },
                        new DeviceModel() // 99 - auto Renault
                        {
                            DeviceBrandId = 18,
                            DeviceTypeId = 4,
                            EnergyKwh = 50f,
                            StandByKwh = 0,
                            Mark = "Zoe BOSE"
                        }
                    });
                    context.SaveChanges();
                }
                
                if (!context.Devices.Any())
                {
                    context.Devices.AddRange(new[]
                    {
                        new Device() // 1
                        {
                            UserId = 4,
                            Name="TV FOX",
                            DeviceModelId = 1, 
                            Visibility = true, 
                            Controlability = true, 
                            TurnOn = false,
                            EnergyInKwh = 0.05f,
                            StandByKwh = 0
                        },
                        new Device() // 2
                        {
                            UserId = 5,
                            Name="Klima dnevna soba dole",
                            DeviceModelId = 5,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = false,
                            EnergyInKwh = 1.8f,
                            StandByKwh = 0
                        },
                        new Device() // 3
                        {
                            UserId = 5,
                            Name="Klima dnevna soba gore",
                            DeviceModelId = 6,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = false,
                            EnergyInKwh = 1.2f,
                            StandByKwh = 0
                        },
                        new Device() // 4
                        {
                            UserId = 6,
                            Name="Klima hodnik",
                            DeviceModelId = 4,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.1f,
                            StandByKwh = 0
                        },
                        new Device() // 5
                        {
                            UserId = 7,
                            Name="Klima velika",
                            DeviceModelId = 5,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.8f,
                            StandByKwh = 0
                        },
                        new Device() // 6
                        {
                            UserId = 8,
                            Name="Mala klima",
                            DeviceModelId = 7,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.1f,
                            StandByKwh = 0
                        },
                        new Device() // 7
                        {
                            UserId = 9,
                            Name="Klima",
                            DeviceModelId = 8,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.13f,
                            StandByKwh = 0
                        },
                        new Device() // 8
                        {
                            UserId = 10,
                            Name="Klima",
                            DeviceModelId = 8,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.13f,
                            StandByKwh = 0
                        },
                        new Device() // 9
                        {
                            UserId = 10,
                            Name="Elektricni sporet",
                            DeviceModelId = 9,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.45f,
                            StandByKwh = 0
                        },
                        new Device() // 10
                        {
                            UserId = 10,
                            Name="Friz",
                            DeviceModelId = 29,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.026f,
                            StandByKwh = 0
                        },
                        new Device() // 11
                        {
                            UserId = 11,
                            Name="El. sporet",
                            DeviceModelId = 9,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.45f,
                            StandByKwh = 0
                        },
                        new Device() // 12
                        {
                            UserId = 12,
                            Name="Sporet",
                            DeviceModelId = 10,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.5f,
                            StandByKwh = 0
                        },
                        new Device() // 13
                        {
                            UserId = 13,
                            Name="Elektr. sporet",
                            DeviceModelId = 11,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.45f,
                            StandByKwh = 0
                        },
                        new Device() // 14
                        {
                            UserId = 14,
                            Name="Elektricni sporet",
                            DeviceModelId = 12,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.37f,
                            StandByKwh = 0
                        },
                        new Device() // 15
                        {
                            UserId = 15,
                            Name="Elektricni donja kuhinja",
                            DeviceModelId = 13,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.185f,
                            StandByKwh = 0
                        },
                        new Device() // 16
                        {
                            UserId = 16,
                            Name="elektricni",
                            DeviceModelId = 14,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.7f,
                            StandByKwh = 0
                        },
                        new Device() // 17
                        {
                            UserId = 17,
                            Name="elektricni sporet",
                            DeviceModelId = 15,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 1.55f,
                            StandByKwh = 0
                        },
                        new Device() // 18
                        {
                            UserId = 17,
                            Name="Friz",
                            DeviceModelId = 28,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.026f,
                            StandByKwh = 0
                        },
                        new Device() // 19
                        {
                            UserId = 18,
                            Name="tv",
                            DeviceModelId = 16,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.03f,
                            StandByKwh = 0
                        },
                        new Device() // 20
                        {
                            UserId = 18,
                            Name="tv spavaca",
                            DeviceModelId = 17,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.03f,
                            StandByKwh = 0
                        },
                        new Device() // 21
                        {
                            UserId = 18,
                            Name="tv dnevna",
                            DeviceModelId = 16,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.03f,
                            StandByKwh = 0
                        },
                        new Device() // 22
                        {
                            UserId = 19,
                            Name="televizor",
                            DeviceModelId = 18,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.13f,
                            StandByKwh = 0
                        },
                        new Device() // 23
                        {
                            UserId = 20,
                            Name="televizor dnevna soba",
                            DeviceModelId = 19,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.075f,
                            StandByKwh = 0
                        },
                        new Device() // 24
                        {
                            UserId = 20,
                            Name="tv decija soba",
                            DeviceModelId = 20,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.07f,
                            StandByKwh = 0
                        },
                        new Device() // 25
                        {
                            UserId = 20,
                            Name="tv moja soba",
                            DeviceModelId = 21,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.1f,
                            StandByKwh = 0
                        },
                        new Device() // 26
                        {
                            UserId = 21,
                            Name="tv kancelarija",
                            DeviceModelId = 20,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.07f,
                            StandByKwh = 0
                        },
                        new Device() // 27
                        {
                            UserId = 21,
                            Name="frizider",
                            DeviceModelId = 23,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.024f,
                            StandByKwh = 0
                        },
                        new Device() // 28
                        {
                            UserId = 22,
                            Name="frizider kuhinja",
                            DeviceModelId = 24,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.022f,
                            StandByKwh = 0
                        },
                        new Device() // 29
                        {
                            UserId = 23,
                            Name="Frizider",
                            DeviceModelId = 25,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.025f,
                            StandByKwh = 0
                        },
                        new Device() // 30
                        {
                            UserId = 24,
                            Name="Friz",
                            DeviceModelId = 26,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.026f,
                            StandByKwh = 0
                        },
                        new Device() // 31
                        {
                            UserId = 24,
                            Name="Friz",
                            DeviceModelId = 27,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.026f,
                            StandByKwh = 0
                        },
                        new Device() // 32
                        {
                            UserId = 4,
                            Name="Bojler",
                            DeviceModelId = 30,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.162f,
                            StandByKwh = 0
                        },
                        new Device() // 33
                        {
                            UserId = 4,
                            Name="Bojler",
                            DeviceModelId = 36,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.28f,
                            StandByKwh = 0
                        },
                        new Device() // 34
                        {
                            UserId = 5,
                            Name="Bojler",
                            DeviceModelId = 31,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.162f,
                            StandByKwh = 0
                        },
                        new Device() // 35
                        {
                            UserId = 6,
                            Name="Bojler",
                            DeviceModelId = 32,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.162f,
                            StandByKwh = 0
                        },
                        new Device() // 36
                        {
                            UserId = 7,
                            Name="Bojler",
                            DeviceModelId = 33,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.2768f,
                            StandByKwh = 0
                        },
                        new Device() // 37
                        {
                            UserId = 8,
                            Name="Bojler",
                            DeviceModelId = 34,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.28f,
                            StandByKwh = 0
                        },
                        new Device() // 38
                        {
                            UserId = 9,
                            Name="Bojler",
                            DeviceModelId = 35,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.28f,
                            StandByKwh = 0
                        },
                        new Device() // 39
                        {
                            UserId = 10,
                            Name="kola",
                            DeviceModelId = 37,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 120f,
                            StandByKwh = 0
                        },
                        new Device() // 40
                        {
                            UserId = 11,
                            Name="tesla",
                            DeviceModelId = 38,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 50f,
                            StandByKwh = 0
                        },
                        new Device() // 41
                        {
                            UserId = 12,
                            Name="Kola",
                            DeviceModelId = 39,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 120f,
                            StandByKwh = 0
                        },
                        new Device() // 42
                        {
                            UserId = 13,
                            Name="auto",
                            DeviceModelId = 40,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 50f,
                            StandByKwh = 0
                        },
                        new Device() // 43
                        {
                            UserId = 24,
                            Name="solarna ploca 1",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 44
                        {
                            UserId = 24,
                            Name="solarna ploca 2",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 45
                        {
                            UserId = 24,
                            Name="solarna ploca 3",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 46
                        {
                            UserId = 23,
                            Name="solarna ploca",
                            DeviceModelId = 42,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.36f,
                            StandByKwh = 0
                        },
                        new Device() // 47
                        {
                            UserId = 22,
                            Name="solarni panel",
                            DeviceModelId = 43,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.33f,
                            StandByKwh = 0
                        },
                        new Device() // 48
                        {
                            UserId = 5,
                            Name="sijalica dnevna soba",
                            DeviceModelId = 44,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 49
                        {
                            UserId = 5,
                            Name="sijalica hodnik",
                            DeviceModelId = 44,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 50
                        {
                            UserId = 5,
                            Name="sijalica kupatilo",
                            DeviceModelId = 44,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 51
                        {
                            UserId = 5,
                            Name="sijalica kuhinja",
                            DeviceModelId = 44,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 52
                        {
                            UserId = 5,
                            Name="sijalica trpezarija",
                            DeviceModelId = 44,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 53
                        {
                            UserId = 5,
                            Name="sijalica spavaca",
                            DeviceModelId = 44,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 54
                        {
                            UserId = 5,
                            Name="sijalica decija",
                            DeviceModelId = 44,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 55
                        {
                            UserId = 5,
                            Name="sijalica garaza",
                            DeviceModelId = 44,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 56
                        {
                            UserId = 6,
                            Name="sijalica dnevna soba",
                            DeviceModelId = 45,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 57
                        {
                            UserId = 6,
                            Name="sijalica hodnik",
                            DeviceModelId = 45,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 58
                        {
                            UserId = 6,
                            Name="sijalica kupatilo",
                            DeviceModelId = 45,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 59
                        {
                            UserId = 6,
                            Name="sijalica kuhinja",
                            DeviceModelId = 45,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 60
                        {
                            UserId = 6,
                            Name="sijalica trpezarija",
                            DeviceModelId = 45,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 61
                        {
                            UserId = 6,
                            Name="sijalica spavaca",
                            DeviceModelId = 45,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 62
                        {
                            UserId = 6,
                            Name="sijalica decija",
                            DeviceModelId = 45,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 63
                        {
                            UserId = 6,
                            Name="sijalica garaza",
                            DeviceModelId = 45,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 64
                        {
                            UserId = 7,
                            Name="sijalica dnevna soba",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 65
                        {
                            UserId = 7,
                            Name="sijalica hodnik",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 66
                        {
                            UserId = 7,
                            Name="sijalica kupatilo",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 67
                        {
                            UserId = 7,
                            Name="sijalica kuhinja",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 68
                        {
                            UserId = 7,
                            Name="sijalica trpezarija",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 69
                        {
                            UserId = 7,
                            Name="sijalica spavaca",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 70
                        {
                            UserId = 7,
                            Name="sijalica decija",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 71
                        {
                            UserId = 7,
                            Name="sijalica garaza",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 72
                        {
                            UserId = 23,
                            Name="sijalica dnevna soba",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 73
                        {
                            UserId = 23,
                            Name="sijalica hodnik",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 74
                        {
                            UserId = 23,
                            Name="sijalica kupatilo",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 75
                        {
                            UserId = 23,
                            Name="sijalica kuhinja",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 76
                        {
                            UserId = 23,
                            Name="sijalica trpezarija",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 77
                        {
                            UserId = 23,
                            Name="sijalica spavaca",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 78
                        {
                            UserId = 23,
                            Name="sijalica decija",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 79
                        {
                            UserId = 23,
                            Name="sijalica garaza",
                            DeviceModelId = 46,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.009f,
                            StandByKwh = 0
                        },
                        new Device() // 80
                        {
                            UserId = 17,
                            Name="moj solarni panel",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 81
                        {
                            UserId = 17,
                            Name="moj solarni panel 2",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 82
                        {
                            UserId = 17,
                            Name="moj solarni panel 3",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 83
                        {
                            UserId = 17,
                            Name="moj solarni panel 4",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 84
                        {
                            UserId = 17,
                            Name="moj solarni panel 5",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 85
                        {
                            UserId = 17,
                            Name="moj solarni panel 6",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 86
                        {
                            UserId = 16,
                            Name="Panel 1",
                            DeviceModelId = 42,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.36f,
                            StandByKwh = 0
                        },
                        new Device() // 87
                        {
                            UserId = 16,
                            Name="Panel 2",
                            DeviceModelId = 42,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.36f,
                            StandByKwh = 0
                        },
                        new Device() // 88
                        {
                            UserId = 16,
                            Name="Panel 3",
                            DeviceModelId = 42,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.36f,
                            StandByKwh = 0
                        },
                        new Device() // 89
                        {
                            UserId = 11,
                            Name="Solarna ploca",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 90
                        {
                            UserId = 11,
                            Name="Solarna ploca",
                            DeviceModelId = 42,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.36f,
                            StandByKwh = 0
                        },
                        new Device() // 91
                        {
                            UserId = 20,
                            Name="Solarna ploca",
                            DeviceModelId = 41,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.4f,
                            StandByKwh = 0
                        },
                        new Device() // 92
                        {
                            UserId = 22,
                            Name="Solarna ploca",
                            DeviceModelId = 42,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.36f,
                            StandByKwh = 0
                        },
                        new Device() // 93
                        {
                            UserId = 22,
                            Name="Solarna ploca",
                            DeviceModelId = 42,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.36f,
                            StandByKwh = 0
                        },
                        new Device() // 94
                        {
                            UserId = 7,
                            Name="Solar. ploca",
                            DeviceModelId = 42,
                            Visibility = true,
                            Controlability = true,
                            TurnOn = true,
                            EnergyInKwh = 0.36f,
                            StandByKwh = 0
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}