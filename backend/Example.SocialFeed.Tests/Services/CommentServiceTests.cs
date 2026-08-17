using Example.SocialFeed.Application.DTOs;
using Example.SocialFeed.Application.Interfaces;
using Example.SocialFeed.Application.Services;
using Example.SocialFeed.Domain.Entities;
using Example.SocialFeed.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace Example.SocialFeed.Tests.Services;

public class CommentServiceTests
{
    private readonly Mock<IPostRepository> _postRepository = new();
    private readonly Mock<ICommentRepository> _commentRepository = new();
    private readonly Mock<ICurrentUserProvider> _currentUserProvider = new();
    private readonly CommentService _sut;

    public CommentServiceTests()
    {
        _currentUserProvider.Setup(p => p.GetCurrentUserName()).Returns("Blend 285");
        _sut = new CommentService(_postRepository.Object, _commentRepository.Object, _currentUserProvider.Object);
    }

    [Fact]
    public async Task AddCommentAsync_WithValidText_PersistsCommentAsCurrentUser()
    {
        // Arrange
        const int postId = 1;
        _postRepository.Setup(r => r.ExistsAsync(postId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _commentRepository
            .Setup(r => r.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Comment c, CancellationToken _) =>
            {
                c.Id = 42;
                return c;
            });

        var request = new CreateCommentRequest { Text = "have a good day" };

        // Act
        var result = await _sut.AddCommentAsync(postId, request);

        // Assert
        result.Id.Should().Be(42);
        result.PostId.Should().Be(postId);
        result.AuthorName.Should().Be("Blend 285");
        result.AuthorInitial.Should().Be("B");
        result.Text.Should().Be("have a good day");

        _commentRepository.Verify(r => r.AddAsync(
            It.Is<Comment>(c => c.AuthorName == "Blend 285" && c.Text == "have a good day" && c.PostId == postId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddCommentAsync_TrimsSurroundingWhitespace()
    {
        _postRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _commentRepository
            .Setup(r => r.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Comment c, CancellationToken _) => c);

        var result = await _sut.AddCommentAsync(1, new CreateCommentRequest { Text = "  hello there  " });

        result.Text.Should().Be("hello there");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task AddCommentAsync_WithEmptyOrWhitespaceText_ThrowsDomainValidationException(string? text)
    {
        _postRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var act = async () => await _sut.AddCommentAsync(1, new CreateCommentRequest { Text = text! });

        await act.Should().ThrowAsync<DomainValidationException>();
        _commentRepository.Verify(r => r.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddCommentAsync_WithTextExceedingMaxLength_ThrowsDomainValidationException()
    {
        _postRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var tooLong = new string('a', CommentService.MaxCommentLength + 1);

        var act = async () => await _sut.AddCommentAsync(1, new CreateCommentRequest { Text = tooLong });

        await act.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task AddCommentAsync_WhenPostDoesNotExist_ThrowsNotFoundException()
    {
        _postRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = async () => await _sut.AddCommentAsync(999, new CreateCommentRequest { Text = "hi" });

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetCommentsAsync_ReturnsCommentsOrderedByCreatedAt()
    {
        _postRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _commentRepository.Setup(r => r.GetByPostIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new List<Comment>
        {
            new() { Id = 2, PostId = 1, AuthorName = "Blend 285", Text = "second", CreatedAt = new DateTime(2024, 1, 1, 10, 1, 0) },
            new() { Id = 1, PostId = 1, AuthorName = "Blend 285", Text = "first", CreatedAt = new DateTime(2024, 1, 1, 10, 0, 0) }
        });

        var result = await _sut.GetCommentsAsync(1);

        result.Select(c => c.Text).Should().ContainInOrder("first", "second");
    }

    [Fact]
    public async Task GetCommentsAsync_WhenPostDoesNotExist_ThrowsNotFoundException()
    {
        _postRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = async () => await _sut.GetCommentsAsync(999);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
