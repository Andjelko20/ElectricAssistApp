using Back_End.Model;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        { 
        }

        public DbSet<Film> film => Set<Film>();
    }
}
