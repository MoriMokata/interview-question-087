using Example.SocialFeed.Application.DTOs;
using Example.SocialFeed.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Example.SocialFeed.Api.Controllers;

[ApiController]
[Route("api/posts/{postId:int}/comments")]
[Produces("application/json")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Gets all comments for a post, oldest first.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CommentDto>>> GetComments(int postId, CancellationToken cancellationToken)
    {
        var comments = await _commentService.GetCommentsAsync(postId, cancellationToken);
        return Ok(comments);
    }

    /// <summary>
    /// Adds a new comment to a post as the current user (posted when the client presses Enter).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommentDto>> AddComment(
        int postId,
        [FromBody] CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var created = await _commentService.AddCommentAsync(postId, request, cancellationToken);
        return CreatedAtAction(nameof(GetComments), new { postId }, created);
    }
}
