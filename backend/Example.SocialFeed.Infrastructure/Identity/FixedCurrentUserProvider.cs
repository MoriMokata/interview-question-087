using Example.SocialFeed.Application.Interfaces;

namespace Example.SocialFeed.Infrastructure.Identity;

/// <summary>
/// Stand-in for a real authentication system. Always returns "Blend 285", the user
/// shown interacting with the comment box in the UI mockup. Replace with an
/// implementation backed by ASP.NET Core auth (e.g. reading HttpContext.User)
/// once a real login flow exists — no other code needs to change.
/// </summary>
public class FixedCurrentUserProvider : ICurrentUserProvider
{
    private const string CurrentUserName = "Blend 285";

    public string GetCurrentUserName() => CurrentUserName;
}
