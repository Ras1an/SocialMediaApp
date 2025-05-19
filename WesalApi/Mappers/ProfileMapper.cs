using Wesal.Dtos.ProfileDto;
using Wesal.Models;

namespace Wesal.Mappers
{
    public static class ProfileMapper
    {
        public static Profile ToProfile(this CreateProfileDto profile)
        {
            return new Profile
            {
                Name = profile.Name,
                DateOfBirth = DateOnly.Parse(profile.DateOfBirth),
                Bio = profile.Bio,
                Gender = profile.Gender,
                CountryId = profile.CountryId,
                CityId = profile.CityId,

            };
        }
    }
}
