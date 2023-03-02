using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Item> items;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

       
    }
}
