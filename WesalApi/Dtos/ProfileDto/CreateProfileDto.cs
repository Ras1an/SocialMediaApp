namespace Wesal.Dtos.ProfileDto
{
    public class CreateProfileDto
    {
        public string Name { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public string? Bio { get; set; }

        public string Gender { get; set; } = null!;

        public string? ProfilePictureLink { get; set; }

        public int? CountryId { get; set; } = null!;

        public int? CityId { get; set; } = null!;

    }
}
