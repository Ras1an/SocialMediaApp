using System;
using System.Collections.Generic;

namespace Wesal.Models;

public partial class FriendshipRequest
{
    public int FriendshipRequestId { get; set; }

    public string FromFriendId { get; set; }

    public string ToFriendId { get; set; }

    public bool IsAccepted { get; set; }

    public DateTime? RequestedAt { get; set; }

    public virtual AppUser FromFriend { get; set; } = null!;

    public virtual AppUser ToFriend { get; set; } = null!;

}


public enum FriendshipResult
{
    NotFound,
    AlreadyAccepted,
    Accepted
}
