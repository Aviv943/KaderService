using KaderService.Services.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Data
{
    public class KaderContext : IdentityDbContext<User>
    {
        public KaderContext (DbContextOptions<KaderContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Post> Post { get; set; }

        public DbSet<Models.Comment> Comment { get; set; }

        public DbSet<Models.Group> Group { get; set; }
    }
}
