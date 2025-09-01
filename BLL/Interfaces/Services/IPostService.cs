using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Dtos.PostDto;
using Wesal.Models;

namespace BLL.Interfaces.Services;

public interface IPostService
{
    Task<List<PostDto>> GetAllPostsAsync(string userId, string currentUserId);
    Task<PostDto> CreatePostAsync(PostDto postDto);
    Task<PostDto> GetPostAsync(int postId);
    Task<(bool Success, string Message)> UpdatePostAsync(int postId, string userId, string postText);
    Task<List<PostDto>> SearchPostAsync(string target, int page, int pageSize);
    Task<(bool Success, string Message)> DeletePostAsync(int postId, string userId);
}

