using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Wesal.Data;
using Wesal.Dtos.PostDto;
using Wesal.Interfaces;
using Wesal.Models;
using WesalApi.Dtos.CountryDto;
using WesalApi.Dtos.FriendRquestDto;
using WesalApi.Dtos.ProfileDto;
using WesalApi.Dtos.UserDto;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wesal.Repository;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
    }






    // Posts



    public async Task<List<PostDto>> GetAllPosts(string userId)
    {
        var posts = await _context.Posts.Where(p => p.AppUserId == userId).Select(
            p => new PostDto
            {
                postId = p.PostId,
                postText = p.PostText,
                postPhoto = p.PostPhoto,
                createdAt = p.CreatedAt,
                //userId = p.AppUserId
            }).AsNoTracking().OrderByDescending(p=> p.createdAt).ToListAsync();

        return posts;
    }


    public async Task<Post> CreatePost(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        return post;
    }

    public async Task<Post> GetPost(int postId)
    {
        return await _context.Posts.Include(p => p.Comments).Include(p => p.Likes).FirstOrDefaultAsync(p => p.PostId == postId);

    }

    public async Task<Post> UpdatePost(int postId, string postText)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);

        post.PostText = postText;

        await _context.SaveChangesAsync();

        return post;
    }


    public async Task DeletePostInfo(int _postId)
    {
        var likes = await _context.Likes.Where(l => l.PostId == _postId).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();

    }

    public async Task<Post> DeletePost(Post _post)
    {
        await DeletePostInfo(_post.PostId);

        var post = _context.Posts.Remove(_post);
        await _context.SaveChangesAsync();

        return _post;
    }

    public async Task<List<PostDto>> SearchPost(string target, int page, int pageSize)
    {

        var posts = await _context.Posts.Where(p => p.PostText.ToLower().Contains(target.ToLower())).Skip((page - 1) * pageSize).Take(pageSize).Select(p => new PostDto
        {
            postId = p.PostId,
            //userId = p.AppUserId,
            postText = p.PostText,
            createdAt = p.CreatedAt,
            postPhoto = p.PostPhoto
        }).ToListAsync();

        return posts;

    }


    public async Task<List<PostDto>> GetTimeline(string userId, int page, int pageSize)
    {
        var friendsIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == true && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(f => f.FromFriendId == userId ? f.ToFriendId : f.FromFriendId).Distinct().ToListAsync();

        friendsIds.Add(userId);
        

        // we should include the image of the user
        var posts =  await _context.Posts.Where(p => friendsIds.Contains(p.AppUserId)).Include(p => p.AppUser).ThenInclude(appUser => appUser.Profiles).Select(p => new PostDto
        {
            postId =p.PostId,
            user = new UserDto {
                Id = p.AppUserId,
                Name = p.AppUser.Profiles.FirstOrDefault().Name,
                photoLink = p.AppUser.Profiles.FirstOrDefault().ProfilePictureLink
            },
            postText = p.PostText,
            postPhoto = p.PostPhoto,
            createdAt = p.CreatedAt,
        }).OrderByDescending(p => Guid.NewGuid()).Skip((page -1) * pageSize).Take(pageSize).ToListAsync();

        //.Where(p => friendsIds.Contains(userId)).ToListAsync();

        //.Include(p=> p.Likes).OrderByDescending(p => p.CreatedAt).ToListAsync(); 

        return posts;
    }

    /*
    public async Task<List<Post>> GetTimeline(string userId)
    {
        var friendsIds = (await _context.FriendShipRequests
    .Where(f => f.IsApproved && (f.ToFriendId == userId || f.FromFriendId == userId))
    .Select(f => f.FromFriendId == userId ? f.ToFriendId : f.FromFriendId)
    .ToListAsync());


        
        var allPosts = await _context.Posts.Include(p => p.Likes).OrderByDescending(p => p.CreatedAt).ToListAsync();
        var posts = allPosts.Where(p => friendsIds.Contains(p.AppUserId)).ToList();


        return posts;

        

    }
    */
    // Profiles

    public async Task<Profile> CreateProfile(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        return profile;
    }


    public async Task<ProfileDto> GetProfileAsync(string userId)
    {
        var profile = await _context.Profiles.Select(p => new ProfileDto
        {
            id = p.AppUserId,
            name = p.Name,
            photoUrl = p.ProfilePictureLink
        }).FirstOrDefaultAsync(p => p.id == userId);

        return profile;

    }

    public async Task<Profile> UpdateProfile(Profile profile)
    {
        var _profile = await _context.Profiles.FirstOrDefaultAsync(p => p.AppUserId == profile.AppUserId);

        if (_profile == null)
            return null;

        _profile.Name = profile.Name;
        _profile.DateOfBirth = profile.DateOfBirth;
        _profile.ProfilePictureLink = profile.ProfilePictureLink;
        //_profile.AppUserId = profile.AppUserId;
        _profile.Gender = profile.Gender;
        _profile.Bio = profile.Bio;
        _profile.CountryId = profile.CountryId;
        _profile.CityId = profile.CityId;

        await _context.SaveChangesAsync();

        return _profile;
    }


    public async Task<List<ProfileDto>> GetAllFriend(string userId)
    {
        var friendsIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == true && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(f => f.FromFriendId == userId ? f.ToFriendId : f.FromFriendId).Distinct().ToListAsync();


        var friends = await _context.Profiles.Where(p => friendsIds.Contains(p.AppUserId)).Select(p => new ProfileDto
        {
            id = p.AppUserId,
            name = p.Name,
            photoUrl = p.ProfilePictureLink
        }).ToListAsync();


        return friends;
    }


    public async Task<List<FriendRequestDto>> GetAllFriendRequests(string userId)
    {

        var friendRequests = await _context.FriendShipRequests.Include(f => f.FromFriend).ThenInclude(FromFriend => FromFriend.Profiles).Where(f => f.IsAccepted == false && f.ToFriendId == userId).Select(f => new FriendRequestDto
        {
            FriendShipRequestId = f.FriendShipRequestId,
            FromFriendId = f.FromFriendId,
            name = f.FromFriend.Profiles.FirstOrDefault().Name,
            photoUrl = f.FromFriend.Profiles.FirstOrDefault().ProfilePictureLink,
            RequestedAt = f.RequestedAt
        }).ToListAsync();



        return friendRequests;

    }


    public async Task<List<ProfileDto>> SearchProfiles(string target, int page, int pageSize)
    {
        var profiles = await _context.Profiles.Where(p => p.Name.ToLower().Contains(target.ToLower())).OrderBy(p => p.Name.ToLower()).Skip((page-1)* pageSize).Take(pageSize).Select(p => new ProfileDto
        {
            id = p.AppUserId,
            name = p.Name,
            photoUrl = p.ProfilePictureLink
        } ).ToListAsync();

        return profiles;

    }



    public async Task<List<string>> GetFriends(string userId)
    {
        var friendsIds = await _context.FriendShipRequests.Where(f =>  f.IsAccepted == true && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(f => f.ToFriendId == userId ? f.FromFriendId : f.ToFriendId).Distinct().ToListAsync();
        //var friends = await _context.Profiles.Where(p => friendsIds.Contains(p.AppUserId)).Select(p => new ProfileDto
        //{
        //    id = p.AppUserId,
        //    name = p.Name,
        //    photoUrl = p.ProfilePictureLink
        //}).ToListAsync();

        return friendsIds;
    }
    public async Task<List<ProfileDto>> SuggestFriends(string userId)
    {
        var myFriendsIds = await GetFriends(userId);
        var friendsOfFriendsIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == true && (myFriendsIds.Contains(f.ToFriendId) || myFriendsIds.Contains(f.FromFriendId)) && f.ToFriendId != userId && f.FromFriendId != userId)
            .Select(f => myFriendsIds.Contains(f.ToFriendId)? f.FromFriendId: f.ToFriendId)
            .Where(fofId => !myFriendsIds.Contains(fofId)).Distinct().ToListAsync();


        var pendingRequestIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == false && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(p => p.ToFriendId == userId? p.FromFriendId: p.ToFriendId).ToListAsync();
        
        var excludedIds = myFriendsIds.Append(userId).Concat(friendsOfFriendsIds).ToList();

        var randomProfiles = await _context.Profiles.Where(p => !excludedIds.Contains(p.AppUserId)).OrderBy(p => Guid.NewGuid()).Take(5).Select(u => u.AppUserId).ToListAsync();

        var finalList = friendsOfFriendsIds.Concat(randomProfiles).Where(id => !pendingRequestIds.Contains(id)).Distinct().ToList();


        
        var friendsOfFriendsProfiles = await _context.Profiles.Where(p => finalList.Contains(p.AppUserId)).Select(p => new ProfileDto
        {
            id = p.AppUserId,
            name = p.Name,
            photoUrl = p.ProfilePictureLink
        }).ToListAsync();


        return friendsOfFriendsProfiles;
    }


    public async Task<FriendShipRequest> SendFriendShipRequest(FriendShipRequest friendShipRequest)
    {
        await _context.AddAsync(friendShipRequest);
        await _context.SaveChangesAsync();

        return friendShipRequest;
    }



    public async Task<FriendShipRequest> GetFriendShipRequest(int friendshipId)
    {
        var friendship = await _context.FriendShipRequests.FirstOrDefaultAsync(f => f.FriendShipRequestId == friendshipId); 
        
        return friendship;
    }

    public async Task<List<FriendShipRequest>> GetFriendShipRequests(string userId)
    {
        var friendShips = await _context.FriendShipRequests.Where(f => f.ToFriendId == userId).ToListAsync();

        return friendShips;

    }

    public async Task<FriendShipRequest> HandelFriendshipRequest(int friendshipId, bool accepted = true)
    {
        var friendship = await _context.FriendShipRequests.FirstOrDefaultAsync(f => f.FriendShipRequestId == friendshipId);
        friendship.IsAccepted = accepted;
        await _context.SaveChangesAsync();

        return friendship;
    }


    // Comment

    public async Task<Comment> CreateComment(Comment comment)
    {
         await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;


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




    // Countries & Cities
    public async Task<List<CountryDto>> GetCountries()
    {
        var countries =  await _context.Countries.Select(c => new CountryDto
        {
            countryId = c.CountryId,
            countryName = c.CountryName
        }).ToListAsync();


        return countries;
    }


    public async Task<List<CityDto>> GetCities(int countryId)
    {
        var cities = await _context.Cities.Where(c => c.CountryId == countryId).Select(c => new CityDto
        {
            cityId = c.CityId,
            cityName = c.CityName,
            countryId = countryId
        }).ToListAsync();


        return cities;
    }

    
}