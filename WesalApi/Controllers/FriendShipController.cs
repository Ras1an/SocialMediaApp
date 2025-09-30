using BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wesal.Controllers;
using Wesal.Models;

namespace WesalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendshipController : MainController
{
    private readonly IFriendshipService _friendshipService;

    public FriendshipController(UserManager<AppUser> AppUserManager, IFriendshipService friendshipService) : base(AppUserManager)
    {
        _friendshipService = friendshipService;
    }


    [Authorize]
    [HttpDelete("DeleteFriendship/{userId2}")]
    public async Task<IActionResult> DeleteFriendship(string userId2)
    {
        var user = await _userManager.GetUserAsync(User);
        var userId1 = user.Id;

         var success = await _friendshipService.DeleteFriendship(userId1, userId2);

         if(!success)
              return NotFound();

         return NoContent();

    }



    [Authorize]
    [HttpPut("AcceptFriendRequest/{userId2}")]
    public async Task<IActionResult> AcceptFriendRequest(string userId2)
    {
        var user = await _userManager.GetUserAsync(User);
        var userId1 = user.Id;

        var result = await _friendshipService.AcceptFriendship(userId1, userId2);


        return result switch {
            FriendshipResult.NotFound => NotFound("Friend request not found."),
            FriendshipResult.AlreadyAccepted => BadRequest("Friend request already accepted."),
            FriendshipResult.Accepted => Ok("Friend request accepted successfully."),
            _ => StatusCode(500, "Unexpected error.")
        };
    }
}
