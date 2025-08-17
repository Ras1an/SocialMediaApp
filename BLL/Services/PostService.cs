using BLL.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Dtos.PostDto;
using Wesal.Models;
using WesalApi.Interfaces;
using AutoMapper;
namespace BLL.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepo;
    private readonly IMapper _mapper;

    public PostService(IPostRepository postRepo, IMapper mapper)
    {
        _postRepo = postRepo;
        _mapper = mapper;
    }

    public async Task<List<PostDto>> GetAllPostsAsync(string userId)
    {
        var posts = await _postRepo.GetAllPosts(userId);


        return _mapper.Map<List<PostDto>>(posts); 
    }

    public async Task<PostDto> CreatePostAsync(PostDto postDto)
    {
        var post = _mapper.Map<Post>(postDto);
        var createdPost = await _postRepo.CreatePost(post);
        return _mapper.Map<PostDto>(createdPost);
    }

    public async Task<PostDto> GetPostAsync(int postId)
    {
        var post = await _postRepo.GetPost(postId);
        return _mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> UpdatePostAsync(int postId, string postText)
    {
        var updated = await _postRepo.UpdatePost(postId, postText);
        return _mapper.Map<PostDto>(updated);
    }

    public async Task DeletePostInfoAsync(int postId)
    {
        await _postRepo.DeletePostInfo(postId);
    }

    public async Task<List<PostDto>> SearchPostAsync(string target, int page, int pageSize)
    {
        var posts = await _postRepo.SearchPost(target, page, pageSize);


        return _mapper.Map<List<PostDto>>(posts);
    }

    public async Task<PostDto> DeletePostAsync(PostDto postDto)
    {
        var post = _mapper.Map<Post>(postDto);
        var deleted = await _postRepo.DeletePost(post);
        return _mapper.Map<PostDto>(deleted);
    }
}
