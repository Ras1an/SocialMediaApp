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
        public MainController(UserManager<AppUser> AppUserManager)
        {
            _userManager = AppUserManager;

        }

    }
}
