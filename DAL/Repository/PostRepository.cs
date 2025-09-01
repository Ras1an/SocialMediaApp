using Microsoft.AspNetCore.Http.HttpResults;
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


        var posts = await _context.Posts.Where(p => p.AppUserId == userId).Include(p => p.AppUser.Profiles).Include(p => p.Comments).ThenInclude(c => c.AppUser.Profiles).Include(p => p.Likes).ThenInclude(l => l.AppUser.Profiles).AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync();

        return posts;
    }


    public async Task<Post> CreatePost(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        var newPost = await GetPost(post.PostId);

        return newPost;
    }

    public async Task<Post?> GetPost(int postId)
    {
        return await _context.Posts.Include(p => p.AppUser.Profiles).Include(p => p.Comments).ThenInclude(c => c.AppUser.Profiles).Include(p => p.Likes).ThenInclude(l => l.AppUser.Profiles).FirstOrDefaultAsync(p => p.PostId == postId);

    }

    public async Task UpdatePost(int postId, string postText)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);

        post.PostText = postText;

        await _context.SaveChangesAsync();
    }



    public async Task DeletePost(int postId)
    {
        await _context.Likes.Where(l => l.PostId == postId).ExecuteDeleteAsync();
        await _context.Comments.Where(c => c.PostId == postId).ExecuteDeleteAsync();
        await _context.Posts.Where(p => p.PostId == postId).ExecuteDeleteAsync();
      
    }
    public async Task<List<Post>> SearchPost(string target, int page, int pageSize)
    {

        var posts = await _context.Posts.Where(p => p.PostText.ToLower().Contains(target.ToLower())).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return posts;
    }


}
