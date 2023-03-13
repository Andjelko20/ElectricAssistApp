using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Server.Helpers;
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
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<ChargingScheduler> ChargingSchedulers { get; set; }


        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context=serviceScope.ServiceProvider.GetService<SqliteDbContext>();

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
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

    }
}
