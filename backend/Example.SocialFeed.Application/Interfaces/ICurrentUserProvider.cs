namespace Example.SocialFeed.Application.Interfaces;

/// <summary>
/// Resolves the identity of the user currently interacting with the app.
/// There is no login flow in this exercise, so the implementation returns a fixed
/// user ("Blend 285", per the UI mockup). Swapping in a real authentication-backed
/// implementation later requires no change to callers.
/// </summary>
public interface ICurrentUserProvider
{
    string GetCurrentUserName();
}
