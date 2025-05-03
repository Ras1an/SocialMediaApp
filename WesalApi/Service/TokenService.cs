using Api.Interfaces;
using Wesal.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Service;

public class TokenService : ITokenService
{

    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration congfig)
    {
        _config = congfig;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]));
        
    }
    public string CreateToken(AppUser AppUser)
    {
        var Claims = new List<Claim>
        {
            
            new Claim(JwtRegisteredClaimNames.Email, AppUser.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, AppUser.UserName),
            new Claim(ClaimTypes.NameIdentifier, AppUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, AppUser.Id.ToString())

        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
