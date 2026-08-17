using Example.SocialFeed.Application.DTOs;

namespace Example.SocialFeed.Application.Interfaces;

public interface ICommentService
{
    /// <summary>
    /// Returns all comments for a post, oldest first. Throws NotFoundException if the post doesn't exist.
    /// </summary>
    Task<List<CommentDto>> GetCommentsAsync(int postId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new comment (as the current user) to a post and returns it.
    /// Throws NotFoundException if the post doesn't exist.
    /// Throws DomainValidationException if the text is empty/whitespace or too long.
    /// </summary>
    Task<CommentDto> AddCommentAsync(int postId, CreateCommentRequest request, CancellationToken cancellationToken = default);
}
