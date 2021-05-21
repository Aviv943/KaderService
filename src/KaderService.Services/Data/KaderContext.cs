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

            modelBuilder
                .Entity<Post>()
                .HasOne(post => post.Creator)
                .WithMany(user => user.Posts);

            modelBuilder
                .Entity<Comment>()
                .HasOne(post => post.Creator)
                .WithMany(user => user.Comments);

            //modelBuilder
            //    .Entity<Group>()
            //    .HasOne(@group => group.Category)
            //    .WithMany(category => category.Groups)


            //modelBuilder
            //    .Entity<Category>()
            //    .HasMany(category => category.Groups)
            //    .WithOne(@group => @group.Category)
            //    .HasForeignKey(g => g.CategoryId);

            modelBuilder
                .Entity<RelatedPost>()
                .HasKey(table => new { CustomerId = table.UserId, ItemId = table.PostId });
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Group> Groups { get; set; }
        
        public DbSet<Category> Categories { get; set; }

        public DbSet<RelatedPost> RelatedPosts { get; set; }
    }
}