using System;
using System.Collections.Generic;

namespace Wesal.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int PostId { get; set; }

    public string AppUserId { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AppUser AppUser { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
