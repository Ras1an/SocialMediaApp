using Wesal.Models;

namespace Wesal.Interfaces;

public interface IProfileRepository
{
    Task<Profile> CreateProfile(Profile profile);
    Task<Profile> UpdateProfile(Profile profile);

    Task<Profile> GetProfileAsync(string userId);
   
    Task<List<Profile>> GetAllFriend(string userId);
    Task<List<FriendShipRequest>> GetAllFriendRequests(string userId);
    Task<List<Profile>> SearchProfiles(string target, int page, int pageSize);
    Task<FriendShipRequest> SendFriendShipRequest(FriendShipRequest friendShipRequest);
    Task<FriendShipRequest> GetFriendShipRequest(int friendshipId);
    Task<FriendShipRequest> HandelFriendshipRequest(int friendshipId, bool accepted);
    Task<List<FriendShipRequest>> GetFriendShipRequests(string userId);
    Task<List<string>> GetFriends(string userId);
    Task<List<Profile>> SuggestFriends(string userId);



    Task<List<Post>> GetTimeline(string userId, int page, int pageSize);

    Task<List<Country>> GetCountries();

    Task<List<City>> GetCities(int countryId);


}