using BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wesal.Controllers;
using Wesal.Models;

namespace WesalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : MainController
    {

        private readonly ILikeService _likeService;
        public LikeController(UserManager<AppUser> userManager, ILikeService likeService) : base(userManager)
        {
            _likeService = likeService;
        }


        [Authorize]
        [HttpPost("CreateLike/{postId}")]
        public async Task<IActionResult> CreateLike(int postId)
        {
            var user = await _userManager.GetUserAsync(User);

            var userId = user.Id;

            var isSucceeded = await _likeService.CreateLikeAsync(userId, postId);

            if (isSucceeded)
                return Ok();

            return BadRequest("Can not put like for that post. Almost post not found.");

        }


        [Authorize]
        [HttpDelete("DeleteLike/{postId}")]
        public async Task<IActionResult> DeleteLike(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var isSucceeded = await _likeService.DeleteLikeAsync(userId, postId);

            if (isSucceeded)
                return Ok();

            return BadRequest("Can not remove like for that post. Almost post not found.");


        }

        [Authorize]
        [HttpGet("GetPostLikes")]
        public async Task<IActionResult> GetPostLikes(int postId, int page, int pageSize)
        {
            var likes = await _likeService.GetPostLikesAsync(postId, page, pageSize);

            return Ok(likes);

        }
    }
}
