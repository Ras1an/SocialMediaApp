
using Wesal.Dtos.PostDto;
using Wesal.Models;

namespace WesalApi.Interfaces;

public interface IPostRepository
{

    Task<List<Post>> GetAllPosts(string userId);
    Task<Post> CreatePost(Post post);
    Task<Post> GetPost(int postId);
    Task<Post> UpdatePost(int postId, string postText);

    Task DeletePostInfo(int _postId);
    Task<List<Post>> SearchPost(string target, int page, int pageSize);
    Task<Post> DeletePost(Post _post);

}
