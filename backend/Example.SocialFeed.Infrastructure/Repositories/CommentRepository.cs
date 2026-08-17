using Example.SocialFeed.Application.Interfaces;
using Example.SocialFeed.Domain.Entities;
using Example.SocialFeed.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Example.SocialFeed.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _dbContext;

    public CommentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Comment>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default) =>
        _dbContext.Comments
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<Comment> AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return comment;
    }
}
