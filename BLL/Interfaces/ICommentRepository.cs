using Wesal.Models;

namespace WesalApi.Interfaces;

public interface ICommentRepository
{
    Task<Comment> CreateComment(Comment comment);
    Task<Comment> GetComment(int commentId);
    Task<Comment> UpdateComment(int commentId, string commentText);
    Task<Comment> DeleteComment(int commentId);
    Task<Dictionary<int, int>> GetCommentsCount(List<int> postIds);
    Task<List<Comment>> GetPostComments(int postId, int page, int pageSize);
}
