namespace Example.SocialFeed.Application.DTOs;

public class PostDto
{
    public int Id { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// First letter of the author's name, used to render the round avatar badge in the UI.
    /// </summary>
    public string AuthorInitial { get; set; } = string.Empty;

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<CommentDto> Comments { get; set; } = new();
}
