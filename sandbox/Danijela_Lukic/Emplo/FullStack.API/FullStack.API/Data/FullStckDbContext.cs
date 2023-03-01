using FullStack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Data
{
    public class FullStckDbContext : DbContext
    {
        public FullStckDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Employees> Employees { get; set; }
    }
}
