using BLL.Dtos.CommentDto;
using BLL.Dtos.GetCommentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;
using WesalApi.Dtos.CommentDto;

namespace BLL.Interfaces.Services;

public interface ICommentService
{
    Task<GetCommentDto> CreateCommentAsync(CommentDto commentDto);
    Task<CommentDto> GetCommentAsync(int commentId);
    Task<GetCommentDto> GetCommentForPostAsync(int commentId);
    Task<CommentDto> UpdateCommentAsync(int commentId, string commentText);
    Task<CommentDto> DeleteCommentAsync(int commentId);
    Task<List<GetCommentDto>> GetPostCommentsAsync(int postId, int page, int pageSize);
}
