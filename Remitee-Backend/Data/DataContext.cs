using Microsoft.EntityFrameworkCore;
using Remitee_Backend.Core.Entities;

namespace Remitee_Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            // Fluent API configurations can be added here if needed
        }
    }
}
