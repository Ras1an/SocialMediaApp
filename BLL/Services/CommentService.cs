using BLL.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;
using WesalApi.Dtos.CommentDto;
using WesalApi.Interfaces;
using AutoMapper;
using BLL.Dtos.CommentDto;

namespace BLL.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepo;
    private readonly IMapper _mapper;

    public CommentService(ICommentRepository commentRepo, IMapper mapper)
    {
        _commentRepo = commentRepo;
        _mapper = mapper;
    }

    public async Task<CommentDto> CreateCommentAsync(CommentDto commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);
        var created = await _commentRepo.CreateComment(comment);
        return _mapper.Map<CommentDto>(created);
    }

    public async Task<CommentDto> GetCommentAsync(int commentId)
    {
        var comment = await _commentRepo.GetComment(commentId);
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<CommentDto> UpdateCommentAsync(int commentId, string commentText)
    {
        var updated = await _commentRepo.UpdateComment(commentId, commentText);
        return _mapper.Map<CommentDto>(updated);
    }

    public async Task<CommentDto> DeleteCommentAsync(int commentId)
    {
        var deleted = await _commentRepo.DeleteComment(commentId);
        return _mapper.Map<CommentDto>(deleted);
    }
}

