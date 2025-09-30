using BLL.Interfaces.Services;
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
    private readonly ISearchService _searchService;
    public SearchController(UserManager<AppUser> userManager, ISearchService searchService) : base(userManager)
    {
        _searchService = searchService;
    }


    //[Authorize]
    //[HttpGet("SearchUsers/{target}")]
    //public async Task<IActionResult> SearchUsers(string target)
    //{

    //}

    [Authorize]
    [HttpGet("SearchPostsAndUsers")]
    public async Task<IActionResult> SearchPostsAndUsers(string targert, int page, int pageSize)
    {
        var user = await _userManager.GetUserAsync(User);

        var searchResult = await _searchService.SearchPostsAndUsersAsync(user.Id,targert, page, pageSize);
        return Ok(searchResult);
    }


    [Authorize]
    [HttpGet("SearchPosts")]
    public async Task<IActionResult> SearchPosts(string targert, int page, int pageSize)
    {
        var user = await _userManager.GetUserAsync(User);

        var posts = await _searchService.SearchPostsAsync(user.Id, targert, page, pageSize);
        return Ok(posts);
    }



    [Authorize]
    [HttpGet("SearchUsers")]
    public async Task<IActionResult> SearchUsers(string targert, int page, int pageSize)
    {

        var users = await _searchService.SearchUsersAsync(targert, page, pageSize);
        return Ok(users);
    }





}
