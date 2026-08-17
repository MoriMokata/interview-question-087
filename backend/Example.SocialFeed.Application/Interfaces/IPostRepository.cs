using Example.SocialFeed.Domain.Entities;

namespace Example.SocialFeed.Application.Interfaces;

public interface IPostRepository
{
    /// <summary>
    /// Returns a post including its comments, ordered oldest first, or null if it doesn't exist.
    /// </summary>
    Task<Post?> GetByIdWithCommentsAsync(int postId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int postId, CancellationToken cancellationToken = default);
}
