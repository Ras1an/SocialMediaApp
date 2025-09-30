using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;

namespace BLL.Interfaces;

public interface IFriendshipRepository
{

    Task<FriendshipRequest> GetFriendshipAsync(string userId1, string userId2);
    Task<bool> DeleteFriendshipAsync(FriendshipRequest friendship);
    Task SaveAsync();
}
