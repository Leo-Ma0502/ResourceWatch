using Microsoft.EntityFrameworkCore;
using ResourceWatcher.Models;

namespace ResourceWatcher.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ImageModel> Images { get; set; }
    }
}