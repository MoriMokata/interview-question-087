namespace Example.SocialFeed.Application.DTOs;

public class CommentDto
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// First letter of the author's name, used to render the round avatar badge in the UI.
    /// </summary>
    public string AuthorInitial { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Payload sent by the client when submitting a new comment (Enter key in the UI).
/// The author is resolved server-side from the current user context, not supplied by the client.
/// </summary>
public class CreateCommentRequest
{
    public string Text { get; set; } = string.Empty;
}
