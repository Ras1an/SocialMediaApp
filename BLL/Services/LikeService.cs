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
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepo;

        public LikeService(ILikeRepository likeRepo)
        {
            _likeRepo = likeRepo;
        }
        public async Task<bool> CreateLikeAsync(string userId, int postId)
        {
            Like like = new Like()
            {
                AppUserId = userId,
                PostId = postId
            };

            return await _likeRepo.CreateLikeAsync(like);
        }

        public async Task<bool> DeleteLikeAsync(string userId, int postId)
        {
            return await _likeRepo.DeleteLikeAsync(userId, postId);
        }
    }
}
