namespace Example.SocialFeed.Domain.Entities;

/// <summary>
/// A feed post (e.g. the "IT 08-1" post shown in the UI mockup) that users can comment on.
/// </summary>
public class Post
{
    public int Id { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// Optional caption/body text for the post.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// URL of the image attached to the post (as shown in the mockup).
    /// </summary>
    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
