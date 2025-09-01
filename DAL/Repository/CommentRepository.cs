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





}
