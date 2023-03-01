using FullStack_Demo_API.Models;
using Microsoft.EntityFrameworkCore; 

namespace FullStack_Demo_API.DB
{
    public class FullStackDemoDbContext : DbContext
    {
        public FullStackDemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Zaposlen> Zaposleni { get; set; }
    }
}
