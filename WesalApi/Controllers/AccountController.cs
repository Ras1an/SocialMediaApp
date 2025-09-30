using Api.Dtos;
using Api.Dtos.AccountDto;
using Api.Interfaces;
using Wesal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wesal.Controllers;
using Microsoft.AspNetCore.Authorization;
using BLL.Dtos.AccountDto;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using BLL.Interfaces.EmailService;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : MainController
{

    public readonly SignInManager<AppUser> _SigninManager;
    public readonly ITokenService _tokenService;
    public readonly IEmailSender _emailSender;
    public readonly IConfiguration _config;

    public AccountController(UserManager<AppUser> AppUserManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IConfiguration config, IEmailSender emailSender): base(AppUserManager)
    {
        _SigninManager = signInManager;
        _tokenService = tokenService;
        _config = config;
        _emailSender = emailSender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid input", Errors = ModelState });


            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                return BadRequest(new { Message = "Email already exists." });


            var AppUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var createdAppUser = await _userManager.CreateAsync(AppUser, registerDto.Password);

            if (!createdAppUser.Succeeded)
                return BadRequest(new { Message = "User creation failed", Errors = createdAppUser.Errors });


                var roleResult = await _userManager.AddToRoleAsync(AppUser, "User");
                if (roleResult.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(AppUser);
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    var clientUrl = _config["Frontend:BaseUrl"] ?? "http://127.0.0.1:3000";
                    var callback = $"{clientUrl}/confirm-email.html?userId={AppUser.Id}&token={encodedToken}";

                    await _emailSender.SendEmailAsync(AppUser.Email, "Confirm your email", $"Please confirm your account by clicking <a href=\"{callback}\">here</a>.");
                    

                    return Ok(new { Message = "Registration successful. Check email to confirm." });
                }
                else
                {
                    return BadRequest(new { Message = "User creation failed", Errors = roleResult.Errors });
                }

        }

        catch (Exception ex)
        {
           // _logger.LogError(ex, "Error occured during registration");
            return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later."});
        }
    }


    [HttpPost("ResendConfirmation")]
    public async Task<IActionResult> ResendConfirmation([FromBody] ResendDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.email);
        if (user == null)
            return Ok();

        if(user.EmailConfirmed)
            return BadRequest(new { Message = "Already confirmed."});

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var clientUrl = _config["Frontend:BaseUrl"] ?? "http://127.0.0.1:3000";
        var callback = $"{clientUrl}/confirm-email.html?userId={user.Id}&token={encodedToken}";

        await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by clicking <a href=\"{callback}\">here</a>.");

        return Ok(new { Message = "Confirmation email sent succussfully"});
    }



    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationDto dto)
    {
        if (dto.userId == null || dto.token == null)
            return BadRequest("Missing Parameters");

        var user = await _userManager.FindByIdAsync(dto.userId);
        if (user == null)
            return NotFound();

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.token));
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        if (result.Succeeded)
            return Ok(new { Message = "Email confirmed" });

        return BadRequest(new { Message = "Email confirmation failed.", Errors = result.Errors });

    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Message = "Invalid input", Errors = ModelState });

        var AppUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

        

        if (AppUser == null)
            return Unauthorized(new { Message = "Username or password incorrect" });

        var result = await _SigninManager.CheckPasswordSignInAsync(AppUser, loginDto.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new { Message = "Username or password incorrect" });

        if(!AppUser.EmailConfirmed)
            return Unauthorized(new { Message = "Please confirm your email before logging in.", Email = AppUser.Email});

        return Ok(new NewUserDto
        {
            AppUserId = AppUser.Id,
            Email = AppUser.Email,
            Token = _tokenService.CreateToken(AppUser)
        });
    }



    [Authorize]
    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        
        var result = await _userManager.ChangePasswordAsync(user, dto.currentPassword, dto.newPassword);

        if (!result.Succeeded) {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors});
        }


        return Ok("Password Changed Successfully");
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.email);
        if (user != null) { 
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var clientUrl = _config["Frontend:BaseUrl"] ?? "http://127.0.0.1:3000";
        var resetLink = $"{clientUrl}/reset-password.html?userId={user.Id}&token={encodedToken}";
        await _emailSender.SendEmailAsync(user.Email, "Reset Password", $"You can reset your password by clicking <a href=\"{resetLink}\">here</a>.");
        }


        return Ok(new { Message = "If the email exists, a reset link was sent." });
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.userId);

        if (user == null)
            return BadRequest(new { Message = "Invalid user" });

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.token));
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.newPassword);

        if(result.Succeeded)
            return Ok(new { Message = "Password reset successful"});

        return BadRequest(new { Message = "Password reset failed.", Errors = result.Errors});
    }

}

//asfd