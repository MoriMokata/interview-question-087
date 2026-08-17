namespace Example.SocialFeed.Domain.Exceptions;

/// <summary>
/// Thrown when a requested entity cannot be found.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public static NotFoundException ForPost(int postId) =>
        new($"Post with id '{postId}' was not found.");
}

/// <summary>
/// Thrown when input fails domain validation rules.
/// </summary>
public class DomainValidationException : Exception
{
    public DomainValidationException(string message) : base(message)
    {
    }
}
