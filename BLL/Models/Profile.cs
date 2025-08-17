using System;
using System.Collections.Generic;

namespace Wesal.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public string AppUserId { get; set; }

    public string? Name { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string? Bio { get; set; }

    public string? Gender { get; set; }

    public string? ProfilePictureLink { get; set; }

    public int? CountryId { get; set; }

    public int? CityId { get; set; }

    public virtual AppUser AppUser { get; set; } = null!;

    public virtual City? CityNavigation { get; set; }

    public virtual Country? CountryNavigation { get; set; }
}
