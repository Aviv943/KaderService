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
                .HasOne(p => p.Group)
                .WithMany(g => g.Posts)
                .HasForeignKey(p => p.GroupId);

            modelBuilder
                .Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<Post>().HasOne(post => post.Creator).WithMany(user => user.Posts);
            modelBuilder.Entity<Comment>().HasOne(post => post.Creator).WithMany(user => user.Comments);
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Group> Groups { get; set; }
    }
}