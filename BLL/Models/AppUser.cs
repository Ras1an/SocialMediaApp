using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace Wesal.Models;

public partial class AppUser : IdentityUser
{

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<FriendshipRequest> FriendshipRequestFromFriends { get; set; } = new List<FriendshipRequest>();

    public virtual ICollection<FriendshipRequest> FriendshipRequestToFriends { get; set; } = new List<FriendshipRequest>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    

}
