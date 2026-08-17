using Example.SocialFeed.Application.Interfaces;
using Example.SocialFeed.Domain.Entities;
using Example.SocialFeed.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Example.SocialFeed.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _dbContext;

    public PostRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Post?> GetByIdWithCommentsAsync(int postId, CancellationToken cancellationToken = default) =>
        _dbContext.Posts
            .Include(p => p.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);

    public Task<bool> ExistsAsync(int postId, CancellationToken cancellationToken = default) =>
        _dbContext.Posts.AsNoTracking().AnyAsync(p => p.Id == postId, cancellationToken);
}
