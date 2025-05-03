using System;
using System.Collections.Generic;

namespace Wesal.Models;

public partial class City
{
    public int CityId { get; set; }

    public int CountryId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
