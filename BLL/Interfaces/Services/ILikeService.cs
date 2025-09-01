using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface ILikeService
    {
        Task<bool> CreateLikeAsync(string userId, int postId);
        Task<bool> DeleteLikeAsync(string userId, int postId);
    }
}
