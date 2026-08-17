using Example.SocialFeed.Application.DTOs;
using Example.SocialFeed.Application.Interfaces;
using Example.SocialFeed.Domain.Entities;

namespace Example.SocialFeed.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<PostDto?> GetPostAsync(int postId, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdWithCommentsAsync(postId, cancellationToken);
        return post is null ? null : MapToDto(post);
    }

    internal static PostDto MapToDto(Post post) => new()
    {
        Id = post.Id,
        AuthorName = post.AuthorName,
        AuthorInitial = ToInitial(post.AuthorName),
        Content = post.Content,
        ImageUrl = post.ImageUrl,
        CreatedAt = post.CreatedAt,
        Comments = post.Comments
            .OrderBy(c => c.CreatedAt)
            .Select(CommentService.MapToDto)
            .ToList()
    };

    internal static string ToInitial(string name) =>
        string.IsNullOrWhiteSpace(name) ? "?" : name.Trim()[0].ToString().ToUpperInvariant();
}
