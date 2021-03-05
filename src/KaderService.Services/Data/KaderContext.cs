using System;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Data
{
    public class KaderContext : IdentityDbContext<User>
    {
        public KaderContext(DbContextOptions<KaderContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //todo: Aviv validate that works as planned
            modelBuilder
                .Entity<Post>()
                .Property(p => p.ImagesUri)
                .HasConversion(images => 
                    string.Join(',', images),
                    images => images.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder
                .Entity<Group>()
                .HasMany(group => group.Managers)
                .WithMany(user => user.ManagerInGroups)
                .UsingEntity(j => j.ToTable("GroupsManagers"));

            modelBuilder
                .Entity<Group>()
                .HasMany(group => group.Members)
                .WithMany(user => user.MemberInGroups)
                .UsingEntity(j => j.ToTable("GroupsMembers"));

            modelBuilder
                .Entity<Post>()
                .HasOne(post => post.Group)
                .WithMany(group => group.Posts)
                .HasForeignKey<string>(p => p.);
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Group> Groups { get; set; }
    }
}