using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;

namespace BLL.Interfaces.Services
{
    public interface IFriendshipService
    {

        Task<bool> DeleteFriendship(string userId1, string userId2);
        Task<FriendshipResult> AcceptFriendship(string userId1, string userId2);

    }
}
