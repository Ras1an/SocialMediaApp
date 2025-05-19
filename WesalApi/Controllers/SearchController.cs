using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wesal.Controllers;
using Wesal.Interfaces;
using Wesal.Models;

namespace WesalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : MainController
{
    public SearchController(UserManager<AppUser> userManager, IProfileRepository profileRepository) : base(userManager, profileRepository)
    {

    }


    //[Authorize]
    //[HttpGet("SearchUsers/{target}")]
    //public async Task<IActionResult> SearchUsers(string target)
    //{

    //}



}
