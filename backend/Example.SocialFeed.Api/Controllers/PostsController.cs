using Example.SocialFeed.Application.DTOs;
using Example.SocialFeed.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Example.SocialFeed.Api.Controllers;

[ApiController]
[Route("api/posts")]
[Produces("application/json")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// Gets a post (including its comments) by id.
    /// </summary>
    [HttpGet("{postId:int}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDto>> GetById(int postId, CancellationToken cancellationToken)
    {
        var post = await _postService.GetPostAsync(postId, cancellationToken);
        return post is null ? NotFound() : Ok(post);
    }
}
