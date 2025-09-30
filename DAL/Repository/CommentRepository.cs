using Microsoft.EntityFrameworkCore;
using Wesal.Data;
using Wesal.Models;
using WesalApi.Interfaces;

namespace WesalApi.Repository;

public class CommentRepository : ICommentRepository
{


    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateComment(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        var newComment = await _context.Comments.Include(c => c.AppUser.Profiles).FirstOrDefaultAsync(c => c.CommentId == comment.CommentId);

        return newComment;
    }

    public async Task<Comment> GetComment(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);

        return comment;
    }

    public async Task<Comment> UpdateComment(int commentId, string commentText)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);


        comment.CommentText = commentText;

        await _context.SaveChangesAsync();
        return comment;

    }

    public async Task<Comment> DeleteComment(int commentId)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return comment;
    }



    public async Task<Dictionary <int, int>> GetCommentsCount(List<int> postIds)
    {
        return await _context.Comments.Where(c => postIds.Contains(c.PostId))
                    .GroupBy(c => c.PostId).Select(g => new
                    {
                        postId = g.Key,
                        count = g.Count()
                    }).ToDictionaryAsync(g => g.postId, g => g.count);

    }


    public async Task<List<Comment>> GetPostComments(int postId, int page, int pageSize)
    {
        return await _context.Comments.Where(c => c.PostId == postId).OrderByDescending(c => c.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).Include(c => c.AppUser.Profiles).ToListAsync();
    }

}
