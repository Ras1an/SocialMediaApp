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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wesal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : MainController
{

    public PostController(UserManager<AppUser> userManager, IProfileRepository profileRepository, IWebHostEnvironment webHostEnvironment) : base(userManager, profileRepository, webHostEnvironment)
    {

    }



    [Authorize]
    [HttpGet("GetAllPosts")]
    public async Task<IActionResult> GetAllPosts()
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;

        var posts = await _profileRepo.GetAllPosts(userId);

        if(!posts.Any())
            return NotFound("No posts yet");


        return Ok(posts);
    }


    [Authorize]
    [HttpGet("GetAllUserPosts")]
    public async Task<IActionResult> GetAllUserPosts(string userId)
    {
        var posts = await _profileRepo.GetAllPosts(userId);

        if (!posts.Any())
            return NotFound("No posts yet");


        return Ok(posts);
    }



    [Authorize]
    [HttpPost("CreatePost")]
    public async Task<IActionResult> CreatePost([FromForm] CreatePostDto _post)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.GetUserAsync(User);


        string imageUrl = "";
        if (_post.Image != null && _post.Image.Length > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(_post.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await _post.Image.CopyToAsync(stream);
            }


            imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
        }





        var post = new Post
        {
            AppUserId = user.Id,
            PostText = _post.postText,
            PostPhoto = imageUrl
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
    [HttpGet("SearchPost")]
    public async Task<IActionResult> SearchPost([FromQuery] string target, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(target))
            return BadRequest("Search query required");

        var posts = await _profileRepo.SearchPost(target, page, pageSize);

        if (!posts.Any())
            return NotFound("No Posts matched");



        return Ok(posts);

            
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
    public async Task<IActionResult> GetTimeline( int page = 1, int pageSize = 10)
    {
        var user = await _userManager.GetUserAsync(User);
        var userid = user.Id;

        var posts = await _profileRepo.GetTimeline(userid, page, pageSize);


        if(posts.Any())
            return Ok(posts);

        return NotFound("No Posts");
    }





}
