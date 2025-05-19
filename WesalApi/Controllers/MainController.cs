using Api.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wesal.Interfaces;
using Wesal.Models;

namespace Wesal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly IProfileRepository? _profileRepo;
        public readonly IWebHostEnvironment? _webHostEnvironment;
        public MainController(UserManager<AppUser> AppUserManager, IProfileRepository profileRepository = null, IWebHostEnvironment webHostEnvironment = null)
        {
            _userManager = AppUserManager;
            _profileRepo = profileRepository;
            _webHostEnvironment = webHostEnvironment;

        }

    }
}
