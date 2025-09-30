using BLL.Dtos.PostDto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Hosting;
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


    public async Task<Profile> CreateProfile(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        return profile;
    }

    public async Task<FriendshipRequest?> IsFriend(string currentUserId, string friendId)
    {
       return await _context.FriendshipRequests.FirstOrDefaultAsync(f => (f.FromFriendId == currentUserId && f.ToFriendId == friendId) || (f.FromFriendId == friendId && f.ToFriendId == currentUserId));
    }
    public async Task<Profile> GetProfileAsync(string userId)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.AppUserId == userId);

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

    private async Task<List<string>> GetAllFriendIds(string userId)
    {
        var friendsIds = await _context.FriendshipRequests.Where(f => f.IsAccepted && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(f => f.FromFriendId == userId ? f.ToFriendId : f.FromFriendId).Distinct().ToListAsync();
        
        return friendsIds;
    }
    public async Task<List<Profile>> GetAllFriend(string userId)
    {
        var friendsIds = await GetAllFriendIds(userId);


        var friends = await _context.Profiles.Where(p => friendsIds.Contains(p.AppUserId)).ToListAsync();


        return friends;
    }


    public async Task<List<FriendshipRequest>> GetAllFriendRequests(string userId)
    {

        var friendRequests = await _context.FriendshipRequests.Include(f => f.FromFriend).ThenInclude(FromFriend => FromFriend.Profiles).Where(f => f.IsAccepted == false && f.ToFriendId == userId).OrderByDescending(f => f.RequestedAt).ToListAsync();

        return friendRequests;

    }


    public async Task<List<Profile>> SearchProfiles(string target, int page, int pageSize)
    {
        var profiles = await _context.Profiles.Where(p => p.Name.Contains(target))
            .OrderByDescending(p => p.Name == target).ThenByDescending(p => p.Name.StartsWith(target)).ThenBy(p => p.Name.IndexOf(target)).ThenBy(p => p.Name.Length)
            .Skip((page-1)* pageSize).Take(pageSize).ToListAsync();

        return profiles;
    }



    public async Task<List<string>> GetFriends(string userId)
    {
        var friendsIds = await _context.FriendshipRequests.Where(f =>  f.IsAccepted == true && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(f => f.ToFriendId == userId ? f.FromFriendId : f.ToFriendId).Distinct().ToListAsync();
        //var friends = await _context.Profiles.Where(p => friendsIds.Contains(p.AppUserId)).Select(p => new ProfileDto
        //{
        //    id = p.AppUserId,
        //    name = p.Name,
        //    photoUrl = p.ProfilePictureLink
        //}).ToListAsync();

        return friendsIds;
    }
    public async Task<List<Profile>> SuggestFriends(string userId)
    {
        var myFriendsIds = await GetFriends(userId);
        var friendsOfFriendsIds = await _context.FriendshipRequests.Where(f => f.IsAccepted == true && (myFriendsIds.Contains(f.ToFriendId) || myFriendsIds.Contains(f.FromFriendId)) && f.ToFriendId != userId && f.FromFriendId != userId)
            .Select(f => myFriendsIds.Contains(f.ToFriendId)? f.FromFriendId: f.ToFriendId)
            .Where(fofId => !myFriendsIds.Contains(fofId)).Distinct().ToListAsync();


        var pendingRequestIds = await _context.FriendshipRequests.Where(f => f.IsAccepted == false && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(p => p.ToFriendId == userId? p.FromFriendId: p.ToFriendId).ToListAsync();
        
        var excludedIds = myFriendsIds.Append(userId).Concat(friendsOfFriendsIds).ToList();

        var randomProfiles = await _context.Profiles.Where(p => !excludedIds.Contains(p.AppUserId)).OrderBy(p => Guid.NewGuid()).Take(5).Select(u => u.AppUserId).ToListAsync();

        var finalList = friendsOfFriendsIds.Concat(randomProfiles).Where(id => !pendingRequestIds.Contains(id)).Distinct().ToList();


        
        var friendsOfFriendsProfiles = await _context.Profiles.Where(p => finalList.Contains(p.AppUserId)).ToListAsync();


        return friendsOfFriendsProfiles;
    }


    public async Task<FriendshipRequest> SendFriendshipRequest(FriendshipRequest friendshipRequest)
    {
        await _context.FriendshipRequests.AddAsync(friendshipRequest);
        await _context.SaveChangesAsync();

        return friendshipRequest;
    }



    public async Task<FriendshipRequest> GetFriendshipRequest(int friendshipId)
    {
        var friendship = await _context.FriendshipRequests.FirstOrDefaultAsync(f => f.FriendshipRequestId == friendshipId); 
        
        return friendship;
    }

    public async Task<List<FriendshipRequest>> GetFriendshipRequests(string userId)
    {
        var friendships = await _context.FriendshipRequests.Where(f => f.ToFriendId == userId).OrderByDescending(f => f.RequestedAt).ToListAsync();

        return friendships;

    }

    public async Task<FriendshipRequest> HandelFriendshipRequest(int friendshipId, bool accepted = true)
    {
        var friendship = await _context.FriendshipRequests.FirstOrDefaultAsync(f => f.FriendshipRequestId == friendshipId);
        friendship.IsAccepted = accepted;
        await _context.SaveChangesAsync();

        return friendship;
    }

    //private async Task ScoreFunction()
    //{
    //    var friendsIds = await GetAllFriendIds(userId);
    //    friendsIds.Add(userId); // include current user

    //    var posts = await _context.Posts.Where(p => friendsIds.Contains(p.AppUserId)).Select(p => new
    //    {
    //        Post = p,
    //        Score = (p.CreatedAt > DateTime.UtcNow.AddHours(-24)? 50 : 0) + 
    //                 (p.Likes.Count * 2) + 
    //                 (p.Comments.Count * 2)
    //    });

    //    return null;
    //}

    // do not forget to delete that function !!!!!!!!!



    // The next function not return Post
    public async Task<List<Post>> GetTimeLineRelevent(string userId, int page, int pageSize)
    {
        var friendIds = await GetAllFriendIds(userId);
        friendIds.Add(userId);


        var lastWeek = DateTime.UtcNow.AddDays(-7);
        var posts = await _context.Posts.Where(p => friendIds.Contains(p.AppUserId)).OrderByDescending(p => ((p.CreatedAt > lastWeek)? 50 : 0) +
                (p.Likes.Count * 5) +
                (p.Comments.Count * 6)).Skip((page - 1) * pageSize).Take(pageSize)
                .Include(p => p.Comments).ThenInclude(c => c.AppUser.Profiles).Include(p => p.Likes).ThenInclude(l => l.AppUser.Profiles).Include(p => p.AppUser.Profiles).ToListAsync();

        /*
        score = (p.CreatedAt > lastWeek) ? 50 : 0 +
                (p.Likes.Count * 5) +
                (p.Comments.Count * 6)
       */
        return posts;
    }

    //public async Task<List<Post>> GetTimeline(string userId, int page, int pageSize)
    //{
    //    var friendsIds = await GetAllFriendIds(userId);


    //    var posts = await _context.Posts.Where(p => friendsIds.Contains(p.AppUserId)).OrderByDescending(p => p.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).Include(p => p.Comments).ThenInclude(c => c.AppUser.Profiles).Include(p => p.Likes).ThenInclude(l => l.AppUser.Profiles).Include(p => p.AppUser.Profiles).ToListAsync();


    //    return posts;
    //}



    public async Task<List<Post>> GetTimeline(string userId, int page, int pageSize)
    {
        var friendIds = await GetAllFriendIds(userId);
        friendIds.Add(userId);


        var posts = await _context.Posts.Where(p => friendIds.Contains(p.AppUserId)).OrderByDescending(p => p.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).Include(p => p.AppUser.Profiles).ToListAsync();

        return posts;
    }

    public async Task<List<Post>> GetRandomTimeline(int pageSize) {
        
        return await _context.Posts.OrderBy(p => Guid.NewGuid()).Take(pageSize).Include(p => p.AppUser.Profiles).ToListAsync();
    }

    public async Task<List<Country>> GetCountries()
    {
        var countries =  await _context.Countries.ToListAsync();


        return countries;
    }


    public async Task<List<City>> GetCities(int countryId)
    {
        var cities = await _context.Cities.Where(c => c.CountryId == countryId).ToListAsync();


        return cities;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    
}