using System;
using System.Collections.Generic;

namespace Wesal.Models;

public partial class Like
{
    public int LikeId { get; set; }

    public int PostId { get; set; }

    public string AppUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AppUser AppUser { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
