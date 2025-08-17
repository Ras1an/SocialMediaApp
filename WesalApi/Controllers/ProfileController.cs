using Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using Wesal.Dtos.ProfileDto;
using Wesal.Interfaces;
using Wesal.Mappers;
using Wesal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using BLL.Interfaces.Services;

namespace Wesal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : MainController
{
    private readonly IProfileService _profileService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProfileController(UserManager<AppUser> userManager, IProfileService profileService, IWebHostEnvironment webHostEnvironment) : base(userManager)
    {
        _profileService = profileService;
        _webHostEnvironment = webHostEnvironment;
    }

    //my profile
    [Authorize]
    [HttpGet("GetProfile")]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _userManager.GetUserAsync(User);

        
        var profile = await _profileService.GetProfileAsync(user.Id);

        return Ok(profile);
    }


    // any user profile
    [Authorize]
    [HttpGet("GetUserProfile")]
    public async Task<IActionResult> GetUserProfile(string userId)
    {
        var profile = await _profileService.GetProfileAsync(userId);


        return Ok(profile);
    }


    [Authorize]
    [HttpPost("CreateProfile")]
    public async Task<IActionResult> CreateProfile([FromForm]CreateProfileDto profile)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized("User not found.");

        string imageUrl = "";
        if (profile.Image != null && profile.Image.Length > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profile.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profile.Image.CopyToAsync(stream);
            }


            imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
        }

        var newProfile = new Profile
        {
            Name = profile.Name,
            DateOfBirth = profile.DateOfBirth,
            Bio = profile.Bio,
            Gender = profile.Gender,
            CountryId = profile.CountryId,
            CityId = profile.CityId,
            AppUserId = user.Id,
            ProfilePictureLink = imageUrl
        };

        var _profile = await _profileService.CreateProfileAsync(newProfile);

        return Ok(_profile);
    }



    [Authorize]
    [HttpGet("GetTimeline")]
    public async Task<IActionResult> GetTimeline(int page = 1, int pageSize = 10)
    {
        var user = await _userManager.GetUserAsync(User);
        var userid = user.Id;

        var posts = await _profileService.GetTimelineAsync(userid, page, pageSize);


        if (posts.Any())
            return Ok(posts);

        return NotFound("No Posts");
    }






    [Authorize]
    [HttpGet("GetCountries")]
    public async Task<IActionResult> GetCountries()
    {
        var countries =  await _profileService.GetCountriesAsync();

        return Ok(countries);
    }

    [Authorize]
    [HttpGet("GetCities/{countryId}")]
    public async Task<IActionResult> GetCities(int countryId)
    {
        var cities = await _profileService.GetCitiesAsync(countryId);

        return Ok(cities);
    }



    [Authorize]
    [HttpPost("UpdateProfile")]
    public async Task<IActionResult> UpdateProfile(CreateProfileDto profile)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var username = User.GetUsername();

        var appUser = await _userManager.FindByNameAsync(username);

        // var _profile = profile.ToProfile();


        string imageUrl = "";
        if (profile.Image != null && profile.Image.Length > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profile.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profile.Image.CopyToAsync(stream);
            }


            imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
        }



        var _profile = new Profile
        {
            AppUserId = appUser.Id,
            Name = profile.Name,
            DateOfBirth = profile.DateOfBirth,
            Bio = profile.Bio,
            Gender = profile.Gender,
            CountryId = profile.CountryId,
            CityId = profile.CityId,
            ProfilePictureLink = imageUrl
        };

        _profile.AppUserId = appUser.Id;

        await _profileService.UpdateProfileAsync(_profile);

        return Ok(_profile);
    }



    [Authorize]
    [HttpGet("GetAllFriends")]
    public async Task<IActionResult> GetAllFriends()
    {
        var user = await _userManager.GetUserAsync(User);

        var friends = await _profileService.GetAllFriendsAsync(user.Id);

        if (!friends.Any())
            return NotFound("No Friends Yet!");



        return Ok(friends);    
    }

    [Authorize]
    [HttpGet("GetAllUserFriends")]
    public async Task<IActionResult> GetAllUserFriends(string userId)
    {
        
        var friends = await _profileService.GetAllFriendsAsync(userId);

        if (!friends.Any())
            return NotFound("No Friends Yet!");



        return Ok(friends);
    }



    [Authorize]
    [HttpGet("GetAllFriendRequests")]
    public async Task<IActionResult> GetAllFriendRequests()
    {
        var user = await _userManager.GetUserAsync(User);

        var friends = await _profileService.GetAllFriendRequestsAsync(user.Id);

        if (!friends.Any())
            return NotFound("No Friends Yet!");



        return Ok(friends);
    }



    [Authorize]
    [HttpGet("SearchProfiles")]
    public async Task<IActionResult> SearchProfiles([FromQuery]string target, [FromQuery] int page = 1, [FromQuery]  int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(target))
            return BadRequest("Search query required");

        var profiles = await _profileService.SearchProfilesAsync(target, page, pageSize);

        if (!profiles.Any())
            return NotFound("No users with that query");

        return Ok(profiles);
    }



    [Authorize]
    [HttpPost("SendFriendShipRequest/{toUserId}")]
    public async Task<IActionResult> SendFriendShipRequest(string ToUserId)
    {
        var user = await _userManager.GetUserAsync(User);
        var FromUserId = user.Id;

        if(await _profileService.GetProfileAsync(ToUserId) ==null)
            return BadRequest("There is no user with that ID");

        
        var friendShipRequest = await _profileService.SendFriendRequestAsync(FromUserId, ToUserId);

        return Ok(friendShipRequest);
    }


    //[Authorize]
    //[HttpGet("GetFriendRequests")]
    //public async Task<IActionResult> GetFriendRequests()
    //{
    //    var user = await _userManager.GetUserAsync(User);
    //    var userId = user.Id;

    //    var friendships = await _profileService.GetFriendRequestsAsync(userId);

    //    if (friendships == null)
    //        return NotFound("No friend Requests found");

    //    return Ok(friendships); 
    //}



    [Authorize]
    [HttpPut("ِAcceptFriendRequest/{friendshipId}")]
    public async Task<IActionResult> AcceptFriendRequest(int friendshipId)
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;

        var friendship = await _profileService.GetFriendRequestAsync(friendshipId);

        if (friendship == null)
            return NotFound("Friend request not found");
        


        try{
            var updatedFriendship = await _profileService.HandleFriendRequestAsync(friendshipId, true);
            return Ok();
        }

        catch (Exception ex)
        {
            return BadRequest("Failed to handel friend request");
        }
    }


    [Authorize]
    [HttpGet("SuggestFriends")]
    public async Task<IActionResult> SuggestFriends()
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;

        var friendsSuggestions = await _profileService.SuggestFriendsAsync(userId);

        if (!friendsSuggestions.Any())
            return NotFound("No Friends Suggestions");


        return Ok(friendsSuggestions);
    }


}
