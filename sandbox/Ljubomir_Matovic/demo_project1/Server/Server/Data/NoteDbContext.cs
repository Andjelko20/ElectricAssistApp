using Microsoft.EntityFrameworkCore;
using Server.Model;

namespace Server.Data
{
    public class NoteDbContext : DbContext
    {
        public NoteDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }  
    }
}
