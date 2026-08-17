using Example.SocialFeed.Domain.Entities;
using Example.SocialFeed.Infrastructure.Persistence;
using Example.SocialFeed.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Example.SocialFeed.Tests.Repositories;

public class CommentRepositoryTests
{
    private static AppDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_PersistsCommentAndAssignsId()
    {
        await using var context = CreateInMemoryContext(nameof(AddAsync_PersistsCommentAndAssignsId));
        var repository = new CommentRepository(context);

        var comment = new Comment
        {
            PostId = 1,
            AuthorName = "Blend 285",
            Text = "have a good day",
            CreatedAt = DateTime.UtcNow
        };

        var created = await repository.AddAsync(comment);

        created.Id.Should().BeGreaterThan(0);
        (await context.Comments.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetByPostIdAsync_ReturnsOnlyCommentsForThatPost_OrderedByCreatedAt()
    {
        await using var context = CreateInMemoryContext(nameof(GetByPostIdAsync_ReturnsOnlyCommentsForThatPost_OrderedByCreatedAt));
        context.Comments.AddRange(
            new Comment { PostId = 1, AuthorName = "Blend 285", Text = "b", CreatedAt = new DateTime(2024, 1, 1, 0, 1, 0) },
            new Comment { PostId = 1, AuthorName = "Blend 285", Text = "a", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0) },
            new Comment { PostId = 2, AuthorName = "Blend 285", Text = "other post", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0) }
        );
        await context.SaveChangesAsync();

        var repository = new CommentRepository(context);
        var result = await repository.GetByPostIdAsync(1);

        result.Should().HaveCount(2);
        result.Select(c => c.Text).Should().ContainInOrder("a", "b");
    }
}
