using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Server.Helpers;
using Server.Models;

namespace Server.Data
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
           .HasOne(u => u.Role)
           .WithMany()
           .HasForeignKey(u => u.RoleId);
        }

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
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

    }
}
