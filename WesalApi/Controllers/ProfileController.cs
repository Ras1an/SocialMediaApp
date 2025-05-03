using Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wesal.Dtos.ProfileDto;
using Wesal.Interfaces;
using Wesal.Mappers;
using Wesal.Models;

namespace Wesal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : MainController
    {

        
        public ProfileController(UserManager<AppUser> userManager, IProfileRepository profileRepo): base(userManager, profileRepo)
        {
        }


        [Authorize]
        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            var profile = await _profileRepo.GetProfileAsync(appUser.Id);

            return Ok(profile);
        }

        [Authorize]
        [HttpPost("CreateProfile")]
        public async Task<IActionResult> CreateProfile(CreateProfileDto profile)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            var _profile = profile.ToProfile();

            _profile.AppUserId = appUser.Id;

            await _profileRepo.CreateProfile(_profile);

            return Ok("Profile Created Successfully");
        }



        [Authorize]
        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(CreateProfileDto profile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            var _profile = profile.ToProfile();

            _profile.AppUserId = appUser.Id;

            await _profileRepo.UpdateProfile(_profile);

            return Ok(_profile);
        }


        [Authorize]
        [HttpPost("SendFriendShipRequest")]
        public async Task<IActionResult> SendFriendShipRequest(int ToUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            var FromUserId = user.Id;

            var friendShipRequest = new FriendShipRequest
            {
                FromFriendId = FromUserId,
                ToFriendId = ToUserId
            };


            friendShipRequest = await _profileRepo.SendFriendShipRequest(friendShipRequest);

            return Ok(friendShipRequest);
        }

    }
}
