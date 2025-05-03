using Api.Dtos;
using Api.Dtos.AccountDto;
using Api.Interfaces;
using Wesal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wesal.Models;
using Microsoft.AspNetCore.Authorization;
using Wesal.Dtos.PostDto;
using Api.Extensions;
using Wesal.Interfaces;
using System.Security.Claims;

namespace Wesal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : MainController
{

    public PostController(UserManager<AppUser> userManager, IProfileRepository profileRepository) : base(userManager, profileRepository)
    {

    }


    [Authorize]
    [HttpPost("CreatePost")]
    public async Task<IActionResult> CreatePost(CreatePostDto _post)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var UserId = appUser.Id;

        var post = new Post
        {
            AppUserId = UserId,
            PostText = _post.postText,
            PostPhoto = _post.postPhotoLink
        };

        await _profileRepo.CreatePost(post);

        return Ok("Post Created");
    }


    [Authorize]
    [HttpPut("UpdatePost")]
    public async Task<IActionResult> UpdatePost(int postId, string _postText)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (_postText == null)
            return BadRequest();

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userId = appUser.Id;


        if (string.IsNullOrEmpty(userId.ToString()))
            return Unauthorized("Post not found");

        var post = await _profileRepo.GetPost(postId);

        if (post is null)
            return NotFound("Post not found");

        if (post.AppUserId != userId)
            return NotFound("You do not have permission to edit it");


        await _profileRepo.UpdatePost(post.PostId, _postText);

        return Ok("Post Updated");



    }

    [Authorize]
    [HttpDelete("DeletePost")]
    public async Task<IActionResult> DeletePost(int postId)
    {
       // var user = await _userManager.GetUserAsync(User);
        //var userId = user.Id;
        var userId = _userManager.GetUserId(User);


        var post = await _profileRepo.GetPost(postId);

        if (post == null || post.AppUserId.ToString() != userId)
            return NotFound("Post not found or you are not authorized to access it.");

        await _profileRepo.DeletePost(post);

        return Ok("Post Deleted");
    }


    [Authorize]
    [HttpGet("GetPost")]
    public async Task<IActionResult> GetPost(int postId)
    {
        var post = await _profileRepo.GetPost(postId);

        if (post == null)
            return NotFound("Post Not Founded");


        return Ok(post);

    }


    [Authorize]
    [HttpGet("GetTimeline")]
    public async Task<IActionResult> GetTimeline()
    {
        var user = await _userManager.GetUserAsync(User);
        var userid = user.Id;

        var posts = await _profileRepo.GetTimeline(userid);


        if(posts.Any())
            return Ok(posts);

        return NotFound("No Posts");
    }





}
