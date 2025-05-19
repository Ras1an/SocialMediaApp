using Wesal.Dtos.PostDto;
using Wesal.Dtos.ProfileDto;
using Wesal.Models;
using WesalApi.Dtos.CountryDto;
using WesalApi.Dtos.FriendRquestDto;
using WesalApi.Dtos.ProfileDto;

namespace Wesal.Interfaces;

public interface IProfileRepository
{


    // posts

    Task<List<PostDto>> GetAllPosts(string userId);
    Task<Post> CreatePost(Post post);
    Task<Post> GetPost(int postId);
    Task<Post> UpdatePost(int postId,string postText);

    Task DeletePostInfo(int _postId);
    Task<List<PostDto>> SearchPost(string target, int page, int pageSize);
    Task<Post> DeletePost(Post _post);

    Task <List<PostDto>> GetTimeline(string userId, int page, int pageSize);


    // profiles


    Task<Profile> CreateProfile(Profile profile);
    Task<Profile> UpdateProfile(Profile profile);

    Task<ProfileDto> GetProfileAsync(string userId);
   
    Task<List<ProfileDto>> GetAllFriend(string userId);
    Task<List<FriendRequestDto>> GetAllFriendRequests(string userId);
    Task<List<ProfileDto>> SearchProfiles(string target, int page, int pageSize);
    Task<FriendShipRequest> SendFriendShipRequest(FriendShipRequest friendShipRequest);
    Task<FriendShipRequest> GetFriendShipRequest(int friendshipId);
    Task<FriendShipRequest> HandelFriendshipRequest(int friendshipId, bool accepted);
    Task<List<FriendShipRequest>> GetFriendShipRequests(string userId);
    Task<List<string>> GetFriends(string userId);
    Task<List<ProfileDto>> SuggestFriends(string userId);




    // Comments
    Task<Comment> CreateComment(Comment comment);
    Task<Comment> GetComment(int commentId);
    Task<Comment> UpdateComment(int commentId, string commentText);
    Task<Comment> DeleteComment(int commentId);




    // Countries & Cities

    Task<List<CountryDto>> GetCountries();

    Task<List<CityDto>> GetCities(int countryId);


}