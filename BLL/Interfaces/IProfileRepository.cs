using BLL.Dtos.PostDto;
using Wesal.Models;
using WesalApi.Dtos.ProfileDto;

namespace Wesal.Interfaces;

public interface IProfileRepository
{
    Task<Profile> CreateProfile(Profile profile);
    Task<Profile> UpdateProfile(Profile profile);

    Task<FriendshipRequest?> IsFriend(string currentUserId, string friendId);
    Task<Profile> GetProfileAsync(string userId);
   
    Task<List<Profile>> GetAllFriend(string userId);
    Task<List<FriendshipRequest>> GetAllFriendRequests(string userId);
    Task<List<Profile>> SearchProfiles(string target, int page, int pageSize);
    Task<FriendshipRequest> SendFriendshipRequest(FriendshipRequest friendshipRequest);
    Task<FriendshipRequest> GetFriendshipRequest(int friendshipId);
    Task<FriendshipRequest> HandelFriendshipRequest(int friendshipId, bool accepted);
    Task<List<FriendshipRequest>> GetFriendshipRequests(string userId);
    Task<List<string>> GetFriends(string userId);
    Task<List<Profile>> SuggestFriends(string userId);

    Task<List<Post>> GetTimeLineRelevent(string userId, int page, int pageSize);
    Task<List<Post>> GetTimeline(string userId, int page, int pageSize);
    Task<List<Post>> GetRandomTimeline(int pageSize);

    Task<List<Country>> GetCountries();

    Task<List<City>> GetCities(int countryId);

    Task SaveAsync();


}