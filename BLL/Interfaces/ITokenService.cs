using Wesal.Models;

namespace Api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser AppUser);
    }
}
