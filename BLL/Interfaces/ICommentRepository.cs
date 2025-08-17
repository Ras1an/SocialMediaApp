using Wesal.Models;

namespace WesalApi.Interfaces;

public interface ICommentRepository
{
    Task<Comment> CreateComment(Comment comment);
    Task<Comment> GetComment(int commentId);
    Task<Comment> UpdateComment(int commentId, string commentText);
    Task<Comment> DeleteComment(int commentId);



}
