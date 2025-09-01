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
using BLL.Interfaces;
namespace BLL.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepo;
    private readonly ILikeRepository _likeRepo;
    private readonly IMapper _mapper;

    public PostService(IPostRepository postRepo, ILikeRepository likeRepo, IMapper mapper)
    {
        _postRepo = postRepo;
        _likeRepo = likeRepo;
        _mapper = mapper;
    }

    public async Task<List<PostDto>> GetAllPostsAsync(string userId, string currentUserId)
    {
        var posts = await _postRepo.GetAllPosts(userId);

        var mappedposts = _mapper.Map<List<PostDto>>(posts);

        foreach (var post in mappedposts)
        {
            post.isLiked = await _likeRepo.IsLiked(currentUserId, post.postId);
        }

        return mappedposts;

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

    public async Task<(bool Success, string Message)> UpdatePostAsync(int postId, string userId, string postText)
    {
        var post = await _postRepo.GetPost(postId);

        if (post == null)
            return (false, "Post not found");
        if (post.AppUserId != userId)
            return (false, "You are not authorized to delete that post!");

        await _postRepo.UpdatePost(postId, postText);

        return (true, "Post edited successfully.");
    }


    public async Task<List<PostDto>> SearchPostAsync(string target, int page, int pageSize)
    {
        var posts = await _postRepo.SearchPost(target, page, pageSize);


        return _mapper.Map<List<PostDto>>(posts);
    }

    public async Task<(bool Success, string Message)> DeletePostAsync(int postId, string userId)
    {
        var post = await _postRepo.GetPost(postId);

        if (post == null)
            return (false, "Post not found");

        if (post.AppUserId != userId)
            return (false, "You are not authorized to delete that post!");

        await _postRepo.DeletePost(postId);
        return (true, "Post deleted successfully");

    }
}
