using System;
using System.Collections.Generic;

namespace Wesal.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string AppUserId { get; set; }

    public string? PostText { get; set; }

    public string? PostPhoto { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AppUser AppUser { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}
