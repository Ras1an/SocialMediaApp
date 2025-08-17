using Api.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wesal.Interfaces;
using Wesal.Models;
using WesalApi.Interfaces;

namespace Wesal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager;
        //public readonly IProfileRepository? _profileRepo;
        //public readonly IPostRepository? _postRepo;
        //public readonly ICommentRepository? _commentRepo;
        //public readonly ICountryCityRepository? _countrycityRepo;
        //public readonly IWebHostEnvironment? _webHostEnvironment;
        //public MainController(UserManager<AppUser> AppUserManager, IProfileRepository profileRepository = null, IPostRepository postRepository = null, ICommentRepository commentRepository = null, ICountryCityRepository countryCityRepository = null  , IWebHostEnvironment webHostEnvironment = null)
        //{
        //    _userManager = AppUserManager;
        //    _profileRepo = profileRepository;
        //    _webHostEnvironment = webHostEnvironment;
        //    _postRepo = postRepository;
        //    _commentRepo = commentRepository;
        //    _countrycityRepo = countryCityRepository;

        //}



        public MainController(UserManager<AppUser> AppUserManager)
        {
            _userManager = AppUserManager;

        }

    }
}
