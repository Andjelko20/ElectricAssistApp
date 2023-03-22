using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Server.Utilities;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using Server.Models.DropDowns.Devices.Agregations;

namespace Server.Data
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<DeviceCategory> DeviceCategories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<DeviceBrand> DeviceBrands { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<ChargingScheduler> ChargingSchedulers { get; set; }
        public DbSet<InclusionScheduler> InclusionSchedulers { get; set; }
        public DbSet<UserEnergyUsage> UserEnergyUsages { get; set; }
        public DbSet<DeviceEnergyUsage> DeviceEnergyUsages { get; set; }
        public DbSet<DeviceDefaultSettings> DeviceDefaultSettings { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<TypeBrand> TypeBrands { get; set; }
        public DbSet<TypeBrandModel> TypeBrandModels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
           .HasOne(u => u.Role)
           .WithMany()
           .HasForeignKey(u => u.RoleId);
            modelBuilder.Entity<ChargingScheduler>().HasKey(x => new { x.DeviceId, x.Day, x.Time });
            modelBuilder.Entity<InclusionScheduler>().HasKey(x => new { x.DeviceId, x.Day, x.TurnOn, x.TurnOff });
            modelBuilder.Entity<UserEnergyUsage>().HasKey(x => new { x.UserId, x.Date });
            modelBuilder.Entity<DeviceEnergyUsage>().HasKey(x => new { x.DeviceId, x.StartTime, x.EndTime });
            modelBuilder.Entity<DeviceDefaultSettings>().HasKey(x => new { x.DeviceModelId, x.DeviceBrandId });
            modelBuilder.Entity<Bill>().HasKey(x => new { x.UserId, x.Month, x.Year });
            modelBuilder.Entity<TypeBrand>().HasKey(x => new { x.TypeId, x.BrandId });
            modelBuilder.Entity<TypeBrandModel>().HasKey(x => new { x.TypeId, x.BrandId, x.ModelId });
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
                            Name="dispecer"
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
                        }

                    });
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(new[]
                    {
                        new UserModel()
                        {
                            Name="Admin admin",
                            RoleId=1,
                            Blocked=false,
                            Username="admin",
                            Password=HashGenerator.Hash("admin")
                        },
                        new UserModel()
                        {
                            Name="User User",
                            RoleId=3,
                            Blocked=true,
                            Username="user",
                            Password=HashGenerator.Hash("user")
                        }, 
                        new UserModel()
                        {
                            Name="DSO DSO", 
                            RoleId = 2, 
                            Blocked=false,
                            Username="dsodso", 
                            Password=HashGenerator.Hash("dsodso")
                        },
                        new UserModel()
                        {
                            Name="Prosumer",
                            RoleId=3,
                            Blocked=false,
                            Username="prosumer",
                            Password=HashGenerator.Hash("prosumer")
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.DeviceModels.Any())
                {
                    context.DeviceModels.AddRange(new[]
                    {
                        new DeviceModel()
                        {
                            Mark = "oznaka1"
                        },
                        new DeviceModel()
                        {
                            Mark = "oznaka2"
                        },
                        new DeviceModel()
                        {
                            Mark = "oznaka3"
                        }
                    }) ;
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
                        new DeviceType()
                        {
                            Name = "Fridge", 
                            CategoryId = 2
                        },
                        new DeviceType()
                        {
                            Name = "TV",
                            CategoryId = 2
                        },
                        new DeviceType()
                        {
                            Name = "Freezer",
                            CategoryId = 2
                        },
                        new DeviceType()
                        {
                            Name = "Boiler", 
                            CategoryId = 2
                        },
                        new DeviceType()
                        {
                            Name = "Electric car", 
                            CategoryId = 3
                        },
                        new DeviceType()
                        {
                            Name = "Solar panel",
                            CategoryId = 1
                        },
                        new DeviceType()
                        {
                            Name = "Bateries",
                            CategoryId = 3
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.DeviceBrands.Any())
                {
                    context.DeviceBrands.AddRange(new[]
                    {
                        new DeviceBrand()
                        {
                            Name = "VOX"
                        },
                        new DeviceBrand()
                        {
                            Name = "VIVAX"
                        },
                        new DeviceBrand()
                        {
                            Name = "TESLA"
                        },
                        new DeviceBrand()
                        {
                            Name = "SunPower"
                        },
                        new DeviceBrand()
                        {
                            Name = "Other"
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.DeviceDefaultSettings.Any())
                {
                    context.DeviceDefaultSettings.AddRange(new[]
                    {
                        new DeviceDefaultSettings
                        {
                            DeviceModelId = 1, 
                            DeviceBrandId = 1, 
                            DefaultKwh = 300
                        },
                        new DeviceDefaultSettings
                        {
                            DeviceModelId = 2,
                            DeviceBrandId = 1,
                            DefaultKwh = 150
                        },
                        new DeviceDefaultSettings
                        {
                            DeviceModelId = 2,
                            DeviceBrandId = 2,
                            DefaultKwh = 175
                        },
                        new DeviceDefaultSettings
                        {
                            DeviceModelId = 5,
                            DeviceBrandId = 3,
                            DefaultKwh = 1750
                        },
                        new DeviceDefaultSettings
                        {
                            DeviceModelId = 6,
                            DeviceBrandId = 4,
                            DefaultKwh = 740
                        },
                        new DeviceDefaultSettings
                        {
                            DeviceModelId = 7,
                            DeviceBrandId = 5,
                            DefaultKwh = 74
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
                if (!context.Devices.Any())
                {
                    context.Devices.AddRange(new[]
                    {
                        new Device()
                        {
                            UserId = 1, 
                            DeviceCategoryId = 1, 
                            DeviceTypeId = 1, 
                            DeviceBrandId = 1, 
                            DeviceModelId = 1, 
                            EnergyInKwh = 10, 
                            StandByKwh = 10, 
                            Visibility = true, 
                            Controlability = true, 
                            TurnOn = false
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.TypeBrands.Any())
                {
                    context.TypeBrands.AddRange(new[]
                    {
                        new TypeBrand()
                        {
                            TypeId = 1,
                            BrandId = 1
                        },
                        new TypeBrand()
                        {
                            TypeId = 1,
                            BrandId = 2
                        },
                        new TypeBrand()
                        {
                            TypeId = 1,
                            BrandId = 3
                        },
                        new TypeBrand()
                        {
                            TypeId = 3,
                            BrandId = 3
                        },
                        new TypeBrand()
                        {
                            TypeId = 3,
                            BrandId = 1
                        },
                        new TypeBrand()
                        {
                            TypeId = 6,
                            BrandId = 4
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.TypeBrandModels.Any())
                {
                    context.TypeBrandModels.AddRange(new[]
                    {
                        new TypeBrandModel()
                        {
                            TypeId = 1,
                            BrandId = 1, 
                            ModelId = 1, 
                            EnergyKwh = 10, 
                            StandByKwh = 2
                        },
                        new TypeBrandModel()
                        {
                            TypeId = 1,
                            BrandId = 2,
                            ModelId = 2,
                            EnergyKwh = 10,
                            StandByKwh = 2
                        },
                        new TypeBrandModel()
                        {
                            TypeId = 1,
                            BrandId = 3,
                            ModelId = 3,
                            EnergyKwh = 10,
                            StandByKwh = 2
                        },
                        new TypeBrandModel()
                        {
                            TypeId = 3,
                            BrandId = 3,
                            ModelId = 1,
                            EnergyKwh = 10,
                            StandByKwh = 2
                        },
                        new TypeBrandModel()
                        {
                            TypeId = 3,
                            BrandId = 1,
                            ModelId = 2,
                            EnergyKwh = 10,
                            StandByKwh = 2
                        },
                        new TypeBrandModel()
                        {
                            TypeId = 6,
                            BrandId = 4,
                            ModelId = 3,
                            EnergyKwh = 10,
                            StandByKwh = 2
                        }
                    });
                    context.SaveChanges();
                }

            }
        }

    }
}
