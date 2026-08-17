using Example.SocialFeed.Domain.Entities;

namespace Example.SocialFeed.Application.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);

    Task<Comment> AddAsync(Comment comment, CancellationToken cancellationToken = default);
}
