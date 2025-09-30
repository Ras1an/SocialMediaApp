using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;

namespace BLL.Interfaces;

public interface ILikeRepository
{

    Task<bool> CreateLikeAsync(Like like);
    Task<bool> DeleteLikeAsync(string userId, int postId);

    Task<List<int>> IsLiked(string userId, List<int> postIds);
    Task<Dictionary<int, int>> GetLikesCounts(List<int> postIds);
    Task<List<Like>> GetPostLikesAsync(int postId, int page, int pageSize);
}
