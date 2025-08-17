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


    public async Task<Profile> CreateProfile(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        return profile;
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


    public async Task<List<Profile>> GetAllFriend(string userId)
    {
        var friendsIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == true && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(f => f.FromFriendId == userId ? f.ToFriendId : f.FromFriendId).Distinct().ToListAsync();


        var friends = await _context.Profiles.Where(p => friendsIds.Contains(p.AppUserId)).ToListAsync();


        return friends;
    }


    public async Task<List<FriendShipRequest>> GetAllFriendRequests(string userId)
    {

        var friendRequests = await _context.FriendShipRequests.Include(f => f.FromFriend).ThenInclude(FromFriend => FromFriend.Profiles).Where(f => f.IsAccepted == false && f.ToFriendId == userId).ToListAsync();

        return friendRequests;

    }


    public async Task<List<Profile>> SearchProfiles(string target, int page, int pageSize)
    {
        var profiles = await _context.Profiles.Where(p => p.Name.ToLower().Contains(target.ToLower())).OrderBy(p => p.Name.ToLower()).Skip((page-1)* pageSize).Take(pageSize).ToListAsync();

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
    public async Task<List<Profile>> SuggestFriends(string userId)
    {
        var myFriendsIds = await GetFriends(userId);
        var friendsOfFriendsIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == true && (myFriendsIds.Contains(f.ToFriendId) || myFriendsIds.Contains(f.FromFriendId)) && f.ToFriendId != userId && f.FromFriendId != userId)
            .Select(f => myFriendsIds.Contains(f.ToFriendId)? f.FromFriendId: f.ToFriendId)
            .Where(fofId => !myFriendsIds.Contains(fofId)).Distinct().ToListAsync();


        var pendingRequestIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == false && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(p => p.ToFriendId == userId? p.FromFriendId: p.ToFriendId).ToListAsync();
        
        var excludedIds = myFriendsIds.Append(userId).Concat(friendsOfFriendsIds).ToList();

        var randomProfiles = await _context.Profiles.Where(p => !excludedIds.Contains(p.AppUserId)).OrderBy(p => Guid.NewGuid()).Take(5).Select(u => u.AppUserId).ToListAsync();

        var finalList = friendsOfFriendsIds.Concat(randomProfiles).Where(id => !pendingRequestIds.Contains(id)).Distinct().ToList();


        
        var friendsOfFriendsProfiles = await _context.Profiles.Where(p => finalList.Contains(p.AppUserId)).ToListAsync();


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


    public async Task<List<Post>> GetTimeline(string userId, int page, int pageSize)
    {
        var friendsIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == true && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(f => f.FromFriendId == userId ? f.ToFriendId : f.FromFriendId).Distinct().ToListAsync();

        friendsIds.Add(userId);


        // we should include the image of the user
        var posts = await _context.Posts.Where(p => friendsIds.Contains(p.AppUserId)).OrderByDescending(p => Guid.NewGuid()).Skip((page - 1) * pageSize).Take(pageSize).Include(p => p.Comments).ThenInclude(c => c.AppUser.Profiles).Include(p => p.Likes).ThenInclude(l => l.AppUser.Profiles).Include(p => p.AppUser.Profiles).ToListAsync();

        //.Where(p => friendsIds.Contains(userId)).ToListAsync();

        //.Include(p=> p.Likes).OrderByDescending(p => p.CreatedAt).ToListAsync(); 

        return posts;
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

    
}