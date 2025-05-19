using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wesal.Controllers;
using Wesal.Interfaces;
using Wesal.Models;
using WesalApi.Dtos.CommentDto;

namespace WesalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : MainController
{

    public CommentController(UserManager<AppUser> userManager, IProfileRepository profileRepository) : base(userManager, profileRepository)
    {

    }

    [Authorize]
    [HttpGet("GetComment")]
    public async Task<IActionResult> GetComment(int commentId)
    {
        var comment = await _profileRepo.GetComment(commentId);

        if (comment == null)
            return NotFound("Comment not found");

        return Ok(comment);

    }



    [Authorize]
    [HttpPost("CreateComment")]

    public async Task<IActionResult> CreateComment([FromBody]CommentDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var user = await _userManager.GetUserAsync(User);
        string userId = user.Id;

        Comment comment = new Comment() { 
            AppUserId = userId,
            CommentText = commentDto.CommentText,
            PostId = commentDto.PostId
        };

        try { 
        var createdComment = await _profileRepo.CreateComment(comment);
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

        var comment = await _profileRepo.GetComment(commentId);

        if (comment == null)
            return NotFound("Comment not found");

        if (comment.AppUserId != userId)
            return Forbid();

        
        try
        {
            var createdComment = await _profileRepo.UpdateComment(commentId, commentText);
            
            return CreatedAtAction(nameof(GetComment), new { id = createdComment.CommentId }, createdComment);

        }

        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while Updating the comment");
        }


    }


    [Authorize]
    [HttpDelete("DeleteComment/{commendId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {


        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;

        var comment = await _profileRepo.GetComment(commentId);

        if (comment == null)
            return NotFound("Comment not found");

        if (comment.AppUserId != userId)
            return Forbid();


        try
        {
            var deleteComment = await _profileRepo.DeleteComment(commentId);

            return NoContent();

        }

        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the comment");
        }



    }

}

