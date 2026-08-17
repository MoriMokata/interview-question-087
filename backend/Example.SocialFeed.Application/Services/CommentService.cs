using Example.SocialFeed.Application.DTOs;
using Example.SocialFeed.Application.Interfaces;
using Example.SocialFeed.Domain.Entities;
using Example.SocialFeed.Domain.Exceptions;

namespace Example.SocialFeed.Application.Services;

public class CommentService : ICommentService
{
    public const int MaxCommentLength = 1000;

    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CommentService(
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        ICurrentUserProvider currentUserProvider)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(int postId, CancellationToken cancellationToken = default)
    {
        if (!await _postRepository.ExistsAsync(postId, cancellationToken))
        {
            throw NotFoundException.ForPost(postId);
        }

        var comments = await _commentRepository.GetByPostIdAsync(postId, cancellationToken);
        return comments.OrderBy(c => c.CreatedAt).Select(MapToDto).ToList();
    }

    public async Task<CommentDto> AddCommentAsync(
        int postId,
        CreateCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!await _postRepository.ExistsAsync(postId, cancellationToken))
        {
            throw NotFoundException.ForPost(postId);
        }

        var text = request.Text?.Trim() ?? string.Empty;

        if (text.Length == 0)
        {
            throw new DomainValidationException("Comment text cannot be empty.");
        }

        if (text.Length > MaxCommentLength)
        {
            throw new DomainValidationException($"Comment text cannot exceed {MaxCommentLength} characters.");
        }

        var comment = new Comment
        {
            PostId = postId,
            AuthorName = _currentUserProvider.GetCurrentUserName(),
            Text = text,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _commentRepository.AddAsync(comment, cancellationToken);
        return MapToDto(created);
    }

    internal static CommentDto MapToDto(Comment comment) => new()
    {
        Id = comment.Id,
        PostId = comment.PostId,
        AuthorName = comment.AuthorName,
        AuthorInitial = PostService.ToInitial(comment.AuthorName),
        Text = comment.Text,
        CreatedAt = comment.CreatedAt
    };
}
