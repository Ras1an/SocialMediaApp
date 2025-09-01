using BLL.Dtos.CommentDto;
using BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wesal.Controllers;
using Wesal.Interfaces;
using Wesal.Models;
using WesalApi.Dtos.CommentDto;
using WesalApi.Interfaces;


namespace WesalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : MainController
{
    private readonly ICommentService _commentService;
    public CommentController(UserManager<AppUser> userManager, ICommentService commentService) : base(userManager)
    {
        _commentService = commentService;
    }

    [Authorize]
    [HttpGet("GetComment")]
    public async Task<IActionResult> GetComment(int commentId)
    {
        var comment = await _commentService.GetCommentAsync(commentId);

        if (comment == null)
            return NotFound("Comment not found");

        return Ok(comment);

    }

    [Authorize]
    [HttpGet("GetCommentForPost")]
    public async Task<IActionResult> GetCommentForPost(int commentId)
    {
        var comment = await _commentService.GetCommentForPostAsync(commentId);

        if (comment == null)
            return NotFound("Comment not found");

        return Ok(comment);

    }



    [Authorize]
    [HttpPost("CreateComment")]

    public async Task<IActionResult> CreateComment([FromBody]CreateCommentDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var user = await _userManager.GetUserAsync(User);
        string userId = user.Id;

        CommentDto comment = new CommentDto() { 
            AppUserId = userId,
            CommentText = commentDto.CommentText,
            PostId = commentDto.PostId
        };

        try { 
        var createdComment = await _commentService.CreateCommentAsync(comment);
            // create endpoint GetComment
        return CreatedAtAction(nameof(GetComment), new { id = createdComment.CommentId}, createdComment);

        }

        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the comment");
        }

    }



    [Authorize]
    [HttpPut("UpdateComment/{commentId}")]
    public async Task<IActionResult> UpdateComment(int commentId, [FromBody] string commentText)
    {

        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;

        var comment = await _commentService.GetCommentAsync(commentId);

        if (comment == null)
            return NotFound("Comment not found");

        if (comment.AppUserId != userId)
            return Forbid();

        
        try
        {
            var createdComment = await _commentService.UpdateCommentAsync(commentId, commentText);
            
            return CreatedAtAction(nameof(GetComment), new { id = createdComment.CommentId }, createdComment);

        }

        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while Updating the comment");
        }


    }


    [Authorize]
    [HttpDelete("DeleteComment")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;

        var comment = await _commentService.GetCommentAsync(commentId);

        if (comment == null)
            return NotFound("Comment not found");

        if (comment.AppUserId != userId)
            return Forbid();


        try
        {
            var deleteComment = await _commentService.DeleteCommentAsync(commentId);

            return NoContent();

        }

        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the comment");
        }



    }

}

