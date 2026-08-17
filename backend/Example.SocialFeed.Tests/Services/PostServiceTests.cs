using Example.SocialFeed.Application.Interfaces;
using Example.SocialFeed.Application.Services;
using Example.SocialFeed.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Example.SocialFeed.Tests.Services;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _postRepository = new();
    private readonly PostService _sut;

    public PostServiceTests()
    {
        _sut = new PostService(_postRepository.Object);
    }

    [Fact]
    public async Task GetPostAsync_WhenPostExists_ReturnsDtoWithCommentsOrderedByCreatedAt()
    {
        var post = new Post
        {
            Id = 1,
            AuthorName = "Change can",
            ImageUrl = "https://i.pinimg.com/1200x/58/b8/94/58b894d8c2f1bfd5056362933f9bb056.jpg",
            CreatedAt = new DateTime(2021, 10, 16, 16, 0, 0),
            Comments = new List<Comment>
            {
                new() { Id = 2, PostId = 1, AuthorName = "Blend 285", Text = "second", CreatedAt = new DateTime(2021, 10, 16, 16, 10, 0) },
                new() { Id = 1, PostId = 1, AuthorName = "Blend 285", Text = "have a good day", CreatedAt = new DateTime(2021, 10, 16, 16, 5, 0) }
            }
        };

        _postRepository.Setup(r => r.GetByIdWithCommentsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);

        var result = await _sut.GetPostAsync(1);

        result.Should().NotBeNull();
        result!.AuthorName.Should().Be("Change can");
        result.AuthorInitial.Should().Be("C");
        result.Comments.Should().HaveCount(2);
        result.Comments.Select(c => c.Text).Should().ContainInOrder("have a good day", "second");
        result.Comments.All(c => c.AuthorInitial == "B").Should().BeTrue();
    }

    [Fact]
    public async Task GetPostAsync_WhenPostDoesNotExist_ReturnsNull()
    {
        _postRepository.Setup(r => r.GetByIdWithCommentsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Post?)null);

        var result = await _sut.GetPostAsync(999);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("Blend 285", "B")]
    [InlineData("change can", "C")]
    [InlineData("  spaced name", "S")]
    public void ToInitial_ReturnsUppercaseFirstLetter(string name, string expected)
    {
        PostService.ToInitial(name).Should().Be(expected);
    }
}
