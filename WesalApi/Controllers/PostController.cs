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
using WesalApi.Interfaces;
using BLL.Interfaces.Services;
using WesalApi.Dtos.UserDto;

namespace Wesal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : MainController
{
    private readonly IPostService _postService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public PostController(UserManager<AppUser> userManager, IPostService postService, IWebHostEnvironment webHostEnvironment) : base(userManager)
    {
        _postService = postService;
        _webHostEnvironment = webHostEnvironment;
    }



    [Authorize]
    [HttpGet("GetAllPosts")]
    public async Task<IActionResult> GetAllPosts()
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;

        var posts = await _postService.GetAllPostsAsync(userId, userId);

        if(!posts.Any())
            return NotFound("No posts yet");


        return Ok(posts);
    }


    [Authorize]
    [HttpGet("GetAllUserPosts")]
    public async Task<IActionResult> GetAllUserPosts(string userId)
    {
        var user = await _userManager.GetUserAsync(User);
   

        var posts = await _postService.GetAllPostsAsync(userId, user.Id);

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





        PostDto post = new PostDto
        {
            user = new UserDto
            {
                Id = user.Id
            },

            postText = _post.postText,
            postPhoto = imageUrl
        
        };

        var createdPost = await _postService.CreatePostAsync(post);

        return Ok(createdPost);
    }


    [Authorize]
    [HttpPut("UpdatePost")]
    public async Task<IActionResult> UpdatePost(int postId, [FromBody] string postText)
    {
        if (postText == null)
            return BadRequest();

        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;



        var result = await _postService.UpdatePostAsync(postId, userId, postText);

        if(!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [Authorize]
    [HttpDelete("DeletePost")]
    public async Task<IActionResult> DeletePost(int postId)
    {
       // var user = await _userManager.GetUserAsync(User);
        //var userId = user.Id;
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;
        
        var result = await _postService.DeletePostAsync(postId, userId);

        if(!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [Authorize]
    [HttpGet("SearchPost")]
    public async Task<IActionResult> SearchPost([FromQuery] string target, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(target))
            return BadRequest("Search query required");

        var posts = await _postService.SearchPostAsync(target, page, pageSize);

        if (!posts.Any())
            return NotFound("No Posts matched");



        return Ok(posts);

            
    }

    [Authorize]
    [HttpGet("GetPost")]
    public async Task<IActionResult> GetPost(int postId)
    {
        var post = await _postService.GetPostAsync(postId);

        if (post == null)
            return NotFound("Post Not Founded");


        return Ok(post);

    }


}
