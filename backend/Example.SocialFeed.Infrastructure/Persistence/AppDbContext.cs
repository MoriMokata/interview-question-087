using Example.SocialFeed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Example.SocialFeed.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();

    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.AuthorName).IsRequired().HasMaxLength(200);
            entity.Property(p => p.ImageUrl).HasMaxLength(2000);
            entity.HasMany(p => p.Comments)
                  .WithOne(c => c.Post)
                  .HasForeignKey(c => c.PostId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.AuthorName).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Text).IsRequired().HasMaxLength(1000);
        });

        // Seed data reproducing the "IT 08-1" mockup: one post by "Change can"
        // with an existing comment from "Blend 285" (the app's current user).
        modelBuilder.Entity<Post>().HasData(new Post
        {
            Id = 1,
            AuthorName = "Change can",
            Content = null,
            ImageUrl = "https://i.pinimg.com/1200x/58/b8/94/58b894d8c2f1bfd5056362933f9bb056.jpg",
            CreatedAt = new DateTime(2021, 10, 16, 16, 0, 0, DateTimeKind.Utc)
        });

        modelBuilder.Entity<Comment>().HasData(new Comment
        {
            Id = 1,
            PostId = 1,
            AuthorName = "Blend 285",
            Text = "have a good day",
            CreatedAt = new DateTime(2021, 10, 16, 16, 5, 0, DateTimeKind.Utc)
        });
    }
}
