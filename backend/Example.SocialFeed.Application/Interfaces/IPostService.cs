using Example.SocialFeed.Application.DTOs;

namespace Example.SocialFeed.Application.Interfaces;

public interface IPostService
{
    /// <summary>
    /// Returns the post (with its comments) for the given id, or null if it doesn't exist.
    /// </summary>
    Task<PostDto?> GetPostAsync(int postId, CancellationToken cancellationToken = default);
}
