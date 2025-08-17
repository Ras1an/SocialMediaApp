using Microsoft.EntityFrameworkCore;
using Wesal.Data;
using Wesal.Dtos.PostDto;
using Wesal.Models;
using WesalApi.Dtos.UserDto;
using WesalApi.Interfaces;

namespace WesalApi.Repository;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<List<Post>> GetAllPosts(string userId)
    {
        var posts = await _context.Posts.Where(p => p.AppUserId == userId).AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync();

        return posts;
    }


    public async Task<Post> CreatePost(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Post> GetPost(int postId)
    {
        return await _context.Posts.Include(p => p.AppUser.Profiles).Include(p => p.Comments).ThenInclude(c => c.AppUser.Profiles).Include(p => p.Likes).ThenInclude(l => l.AppUser.Profiles).FirstOrDefaultAsync(p => p.PostId == postId);

    }

    public async Task<Post> UpdatePost(int postId, string postText)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);

        post.PostText = postText;

        await _context.SaveChangesAsync();
        return post;
    }


    public async Task DeletePostInfo(int _postId)
    {
        var likes = await _context.Likes.Where(l => l.PostId == _postId).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();

    }

    public async Task<Post> DeletePost(Post _post)
    {
        await DeletePostInfo(_post.PostId);

        var post = _context.Posts.Remove(_post);
        await _context.SaveChangesAsync();

        return _post;
    }
    public async Task<List<Post>> SearchPost(string target, int page, int pageSize)
    {

        var posts = await _context.Posts.Where(p => p.PostText.ToLower().Contains(target.ToLower())).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return posts;
    }


}
