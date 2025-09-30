using AutoMapper;
using Azure.Core;
using BLL.Dtos.PostDto;
using BLL.Interfaces;
using BLL.Interfaces.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Wesal.Dtos.PostDto;
using Wesal.Interfaces;
using Wesal.Models;
using WesalApi.Dtos.CountryDto;
using WesalApi.Dtos.FriendRquestDto;
using WesalApi.Dtos.ProfileDto;
using WesalApi.Interfaces;

namespace BLL.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepo;
    private readonly ILikeRepository _likeRepo;
    private readonly ICommentRepository _commentRepo;
    private readonly IMapper _mapper;

    public ProfileService(IProfileRepository profileRepo, ILikeRepository likeRepo, ICommentRepository commentRepo, IMapper mapper)
    {
        _profileRepo = profileRepo;
        _likeRepo = likeRepo;
        _commentRepo = commentRepo;
        _mapper = mapper;
    }


    public async Task<ProfileDto> GetProfileAsync(string currentUserId, string userId)
    {
        var profile = await _profileRepo.GetProfileAsync(userId);
        if (profile == null)
            return null;

        var profileDto = _mapper.Map<ProfileDto>(profile);
        if (currentUserId != userId && userId != null) {
            var friendRequest = await _profileRepo.IsFriend(currentUserId, userId);


            if (friendRequest == null)
                profileDto.friendStatus = FriendStatus.NotFriend;
          
            else if (friendRequest.IsAccepted)
                profileDto.friendStatus = FriendStatus.Friend;
            else  
                profileDto.friendStatus = (currentUserId == friendRequest.FromFriendId)? FriendStatus.PendingSent : FriendStatus.PendingReceived;

        }

        return profileDto;

    }

    public async Task<List<ProfileDto>> GetAllFriendsAsync(string userId) {
        var friends = await _profileRepo.GetAllFriend(userId);

        return _mapper.Map<List<ProfileDto>>(friends); 

}

    public async Task<List<FriendRequestDto>> GetAllFriendRequestsAsync(string userId)
    {
        var friends = await _profileRepo.GetAllFriendRequests(userId);

        return _mapper.Map<List<FriendRequestDto>> (friends);
    }

    public async Task<ProfileDto> CreateProfileAsync(Wesal.Models.Profile profile)
    {
        //var profile = _mapper.Map<Wesal.Models.Profile>(profileDto);
        //var profile = _mapper.Map<Wesal.Models.Profile>(profileDto);
        var created = await _profileRepo.CreateProfile(profile);
        return _mapper.Map<ProfileDto>(created);
    }

    public async Task<ProfileDto> UpdateProfileAsync(Wesal.Models.Profile profile)
    {
        var _profile = _mapper.Map<Wesal.Models.Profile>(profile);
        var updated = await _profileRepo.UpdateProfile(_profile);

        return _mapper.Map<ProfileDto>(updated);
    }

    public async Task<List<ProfileDto>> SearchProfilesAsync(string target, int page, int pageSize)
    {
        var profiles = await _profileRepo.SearchProfiles(target, page, pageSize);

        return _mapper.Map<List<ProfileDto>>(profiles);
    }
    public async Task<FriendRequestDto> SendFriendRequestAsync(string fromFriendId, string toFriendId) {
        var friendshipRequest = new FriendshipRequest
        {
            FromFriendId = fromFriendId,
            ToFriendId = toFriendId
        };

        var friendRequest = await _profileRepo.SendFriendshipRequest(friendshipRequest);

        return _mapper.Map<FriendRequestDto>(friendRequest);
    }


    public async Task<FriendRequestDto> GetFriendRequestAsync(int friendshipId) {

        var friendRequest = await _profileRepo.GetFriendshipRequest(friendshipId);

        return _mapper.Map<FriendRequestDto>(friendRequest);

    }


    public async Task<FriendRequestDto> HandleFriendRequestAsync(int friendshipId, bool accepted) {

        var friendRequest = await _profileRepo.HandelFriendshipRequest(friendshipId, accepted);

        return _mapper.Map<FriendRequestDto>(friendRequest);
    }


    //public async Task<List<FriendRequestDto>> GetFriendRequestsAsync(string userId) {
    //    var friendRequests = await _profileRepo.GetFriendshipRequests(userId);

    //    return _mapper.Map<List<FriendRequestDto>>(friendRequests);
    //}
        

    public async Task<List<string>> GetFriendsAsync(string userId) =>
        await _profileRepo.GetFriends(userId);

    public async Task<List<ProfileDto>> SuggestFriendsAsync(string userId) { 
        var suggestedFriends = await _profileRepo.SuggestFriends(userId);

        return _mapper.Map<List<ProfileDto>>(suggestedFriends);
    }

    public async Task<bool> ChangeNameAsync(string userId, string name)
    {
        var profile = await _profileRepo.GetProfileAsync(userId); 

        if(profile != null) { 
            profile.Name = name;
            await _profileRepo.SaveAsync();
            return true;
        }

        return false;
    }
    public async Task<List<PostDto>> GetTimelineReleventAsync(string userId, int page, int pageSize)
    {
        var posts = await _profileRepo.GetTimeLineRelevent(userId, page, pageSize);
        if (posts.Count == 0)
            return new List<PostDto>();

        var mappedposts = _mapper.Map<List<PostDto>>(posts);


        var postIds = mappedposts.Select(p => p.postId).ToList();
        var likedPosts = await _likeRepo.IsLiked(userId, postIds);

        foreach (var post in mappedposts)
        {
            post.isLiked = likedPosts.Contains(post.postId);
        }

        return mappedposts;

    }


    /*
    public async Task<List<PostDto>> GetTimelineAsync(string userId, int page, int pageSize)
    {
        var posts = await _profileRepo.GetTimeline(userId, page, pageSize);
        var mappedposts = _mapper.Map<List<PostDto>>(posts);

        foreach (var post in mappedposts)
        {
            post.isLiked = await _likeRepo.IsLiked(userId, post.postId);
        }

        return mappedposts;
    }

    */

    public async Task<List<PostDto>> GetTimelineAsync(string userId, int page, int pageSize)
    {
        var posts = await _profileRepo.GetTimeline(userId, page, pageSize);
        if (posts.Count == 0)
            return new List<PostDto>();
        var mappedposts = _mapper.Map<List<PostDto>>(posts);

        var postIds = mappedposts.Select(p => p.postId).ToList();

        var likedPosts = await _likeRepo.IsLiked(userId, postIds);
        var likesCount = await _likeRepo.GetLikesCounts(postIds);
        var commentsCount = await _commentRepo.GetCommentsCount(postIds);

        foreach(var post in mappedposts)
        {
            post.likesCount = likesCount.TryGetValue(post.postId, out var lc) ? lc : 0;
            post.commentsCount = commentsCount.TryGetValue(post.postId, out var cc) ? cc : 0;
            post.isLiked = likedPosts.Contains(post.postId);
        }

        return mappedposts;
    }



    public async Task<List<PostDto>> GetRandomTimelineAsync(string userId, int pageSize)
    {
        var posts = await _profileRepo.GetRandomTimeline(pageSize);
        var mappedposts = _mapper.Map<List<PostDto>>(posts);

        var postIds = mappedposts.Select(p => p.postId).ToList();

        var likedPosts = await _likeRepo.IsLiked(userId, postIds);
        var likesCount = await _likeRepo.GetLikesCounts(postIds);
        var commentsCount = await _commentRepo.GetCommentsCount(postIds);

        foreach (var post in mappedposts)
        {
            post.likesCount = likesCount.TryGetValue(post.postId, out var lc) ? lc : 0;
            post.commentsCount = commentsCount.TryGetValue(post.postId, out var cc) ? cc : 0;
            post.isLiked = likedPosts.Contains(post.postId);
        }

        return mappedposts;
    }

    public async Task<List<CountryDto>> GetCountriesAsync() {  
        var countries = await _profileRepo.GetCountries();

        return _mapper.Map<List<CountryDto>>(countries);
    }

    public async Task<List<CityDto>> GetCitiesAsync(int countryId) { 
        var cities = await _profileRepo.GetCities(countryId);

        return _mapper.Map<List<CityDto>>(cities);
    }

    
}

