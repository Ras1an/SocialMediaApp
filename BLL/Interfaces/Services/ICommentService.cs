using BLL.Dtos.CommentDto;
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
    Task<CommentDto> CreateCommentAsync(CommentDto commentDto);
    Task<CommentDto> GetCommentAsync(int commentId);
    Task<CommentDto> UpdateCommentAsync(int commentId, string commentText);
    Task<CommentDto> DeleteCommentAsync(int commentId);
}
