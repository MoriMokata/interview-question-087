namespace Example.SocialFeed.Domain.Entities;

/// <summary>
/// A comment left on a <see cref="Post"/>.
/// </summary>
public class Comment
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public Post? Post { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
