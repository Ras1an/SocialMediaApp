using BLL.Dtos.LikeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;

namespace BLL.Interfaces.Services
{
    public interface ILikeService
    {
        Task<bool> CreateLikeAsync(string userId, int postId);
        Task<bool> DeleteLikeAsync(string userId, int postId);


        Task<List<LikeDto>> GetPostLikesAsync(int postId, int page, int pageSize);
    }
}
