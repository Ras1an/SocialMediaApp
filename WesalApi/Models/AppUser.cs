using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace Wesal.Models;

public partial class AppUser : IdentityUser<int>
{

    //public Guid Id { get; set; }

    //public string? UserName { get; set; }

    //public string? NormalizedUserName { get; set; }

    //public string? Email { get; set; }

    //public string? NormalizedEmail { get; set; }

    //public bool EmailConfirmed { get; set; }

    //public string? PasswordHash { get; set; }

    //public string? SecurityStamp { get; set; }

    //public string? ConcurrencyStamp { get; set; }

    //public string? PhoneNumber { get; set; }

    //public bool PhoneNumberConfirmed { get; set; }

    //public bool TwoFactorEnabled { get; set; }

    //public DateTimeOffset? LockoutEnd { get; set; }

    //public bool LockoutEnabled { get; set; }

    //public int AccessFailedCount { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<FriendShipRequest> FriendShipRequestFromFriends { get; set; } = new List<FriendShipRequest>();

    public virtual ICollection<FriendShipRequest> FriendShipRequestToFriends { get; set; } = new List<FriendShipRequest>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
