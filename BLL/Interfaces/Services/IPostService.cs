using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Dtos.PostDto;

namespace BLL.Interfaces.Services;

public interface IPostService
{
    Task<List<PostDto>> GetAllPostsAsync(string userId);
    Task<PostDto> CreatePostAsync(PostDto postDto);
    Task<PostDto> GetPostAsync(int postId);
    Task<PostDto> UpdatePostAsync(int postId, string postText);
    Task DeletePostInfoAsync(int postId);
    Task<List<PostDto>> SearchPostAsync(string target, int page, int pageSize);
    Task<PostDto> DeletePostAsync(PostDto postDto);
}

