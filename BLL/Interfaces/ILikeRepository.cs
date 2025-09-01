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

    Task<bool> IsLiked(string userId, int postId);
}
