using System;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Data
{
    public class KaderContext : IdentityDbContext
    {
        public KaderContext(DbContextOptions<KaderContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //todo: Aviv validate that works as planned
            modelBuilder.Entity<Post>().Property(p => p.ImagesUri).HasConversion(images => string.Join(',', images), images => images.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Group> Groups { get; set; }
    }
}