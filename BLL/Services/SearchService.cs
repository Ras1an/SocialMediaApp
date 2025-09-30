using AutoMapper;
using BLL.Dtos.Search;
using BLL.Interfaces;
using BLL.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Dtos.PostDto;
using Wesal.Interfaces;
using WesalApi.Dtos.UserDto;
using WesalApi.Interfaces;

namespace BLL.Services;

public class SearchService : ISearchService
{
    private readonly IProfileRepository _profileRepo;
    private readonly ILikeRepository _likeRepo;
    private readonly ICommentRepository _commentRepo;
    private readonly IPostRepository _postRepo;
    private readonly IMapper _mapper;

    public SearchService(IProfileRepository profileRepo, ILikeRepository likeRepo, ICommentRepository commentRepo, IPostRepository postRepo, IMapper mapper)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
        _likeRepo = likeRepo;
        _commentRepo = commentRepo;
        _mapper = mapper;
    }

   
    public async Task<SearchResultDto> SearchPostsAndUsersAsync(string userId, string target, int page, int pageSize)
    {
        var postsDto = await SearchPostsAsync(userId, target, page, pageSize);


        var usersDto = await SearchUsersAsync(target, page, pageSize);

        
        var searchResult = new SearchResultDto() { 
            posts = postsDto,
            users = usersDto
        };


        return searchResult;
    }


    public async Task<List<PostDto>> SearchPostsAsync(string userId, string target, int page, int pageSize)
    {

        var posts = await _postRepo.SearchPosts(target, page, pageSize);
        var postsDto = _mapper.Map<List<PostDto>>(posts);

        var postIds = postsDto.Select(p => p.postId).ToList();

        var likedPosts = await _likeRepo.IsLiked(userId, postIds);
        var likesCount = await _likeRepo.GetLikesCounts(postIds);
        var commentsCount = await _commentRepo.GetCommentsCount(postIds);

        foreach (var post in postsDto)
        {
            post.likesCount = likesCount.TryGetValue(post.postId, out var lc) ? lc : 0;
            post.commentsCount = commentsCount.TryGetValue(post.postId, out var cc) ? cc : 0;
            post.isLiked = likedPosts.Contains(post.postId);
        }

        return postsDto;
    }


    public async Task<List<UserDto>> SearchUsersAsync(string target, int page, int pageSize)
    {
        var users = await _profileRepo.SearchProfiles(target, page, pageSize);
        var usersDto = _mapper.Map<List<UserDto>>(users);

        return usersDto;
    }
}
