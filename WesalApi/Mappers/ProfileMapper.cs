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
                DateOfBirth = profile.DateOfBirth,
                Bio = profile.Bio,
                Gender = profile.Gender,
                ProfilePictureLink  = profile.ProfilePictureLink,
                CountryId = profile.CountryId,
                CityId = profile.CityId,

            };
        }
    }
}
