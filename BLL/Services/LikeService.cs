using AutoMapper;
using BLL.Dtos.LikeDto;
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
        private readonly IMapper _mapper;

        public LikeService(ILikeRepository likeRepo, IMapper mapper)
        {
            _likeRepo = likeRepo;
            _mapper = mapper;
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

        public async Task<List<LikeDto>> GetPostLikesAsync(int postId, int page, int pageSize)
        {
            var likes = await _likeRepo.GetPostLikesAsync(postId, page, pageSize);

            return _mapper.Map<List<LikeDto>>(likes);
        }
    }
}
