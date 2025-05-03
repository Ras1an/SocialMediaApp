using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsEntensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
    }

}

