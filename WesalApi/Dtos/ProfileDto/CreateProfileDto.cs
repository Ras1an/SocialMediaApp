namespace Wesal.Dtos.ProfileDto
{
    public class CreateProfileDto
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public string Gender { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public IFormFile? Image { get; set; }

    }
}
