using Api.Dtos;
using Api.Dtos.AccountDto;
using Api.Interfaces;
using Wesal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wesal.Controllers;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : MainController
{

    public readonly SignInManager<AppUser> _SigninManager;
    public readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> AppUserManager, ITokenService tokenService, SignInManager<AppUser> signInManager): base(AppUserManager)
    {
        _SigninManager = signInManager;
        _tokenService = tokenService;  
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var AppUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var createdAppUser = await _userManager.CreateAsync(AppUser, registerDto.Password);
            
            if (createdAppUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(AppUser, "AppUser");
                if (roleResult.Succeeded)
                {
                    return Ok(
                            new NewUserDto
                            {
                                Username = AppUser.UserName,
                                Email = AppUser.Email,
                                Token = _tokenService.CreateToken(AppUser)

                            }
                        );
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, createdAppUser.Errors);
            }


        }

        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var AppUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

        
        _userManager.AddToRoleAsync(AppUser, "User");

        if (AppUser == null)
            return Unauthorized("Username or password incorrect");

        var result = await _SigninManager.CheckPasswordSignInAsync(AppUser, loginDto.Password, false);

        if (!result.Succeeded)
            return Unauthorized("Username or password incorrect");

        return Ok(new NewUserDto
        {
            Username = AppUser.UserName,
            Email = AppUser.Email,
            Token = _tokenService.CreateToken(AppUser)
        });
    }
}




//11514546541