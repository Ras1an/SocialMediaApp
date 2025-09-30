using BLL.Interfaces;
using BLL.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;

namespace BLL.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepo;

        public FriendshipService(IFriendshipRepository friendshipRepo)
        {
            _friendshipRepo = friendshipRepo;
            
        }

        public async Task<FriendshipResult> AcceptFriendship(string userId1, string userId2)
        {
            var friendship = await _friendshipRepo.GetFriendshipAsync(userId1, userId2);

            if (friendship == null)
                return FriendshipResult.NotFound;
            if (friendship.IsAccepted)
                return FriendshipResult.AlreadyAccepted;

            friendship.IsAccepted = true;
            await _friendshipRepo.SaveAsync();
           
            return FriendshipResult.Accepted;
        }

        public async Task<bool> DeleteFriendship(string userId1, string userId2)
        {
            var friendship = await _friendshipRepo.GetFriendshipAsync(userId1, userId2);

            if (friendship == null)
                return false;

            return await _friendshipRepo.DeleteFriendshipAsync(friendship);
        }
    }
}
