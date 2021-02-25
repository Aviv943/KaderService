using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Data
{
    public class KaderContext : DbContext
    {
        public KaderContext (DbContextOptions<KaderContext> options)
            : base(options)
        {
        }

        public DbSet<KaderService.Services.Models.Post> Post { get; set; }

        public DbSet<KaderService.Services.Models.Comment> Comment { get; set; }

        public DbSet<KaderService.Services.Models.Group> Group { get; set; }
    }
}
