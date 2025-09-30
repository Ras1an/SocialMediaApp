using BLL.Dtos.PostDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Dtos.PostDto;
using Wesal.Models;
using WesalApi.Dtos.CountryDto;
using WesalApi.Dtos.FriendRquestDto;
using WesalApi.Dtos.ProfileDto;

namespace BLL.Interfaces.Services;

public interface IProfileService
{
    Task<ProfileDto> GetProfileAsync(string currentUserId, string userId);
    Task<List<ProfileDto>> GetAllFriendsAsync(string userId);
    Task<List<FriendRequestDto>> GetAllFriendRequestsAsync(string userId);
    Task<ProfileDto> CreateProfileAsync(Profile profile);
    Task<ProfileDto> UpdateProfileAsync(Profile profile);
    Task<bool> ChangeNameAsync(string userId, string name);
    Task<List<ProfileDto>> SearchProfilesAsync(string target, int page, int pageSize);
    Task<FriendRequestDto> SendFriendRequestAsync(string fromFriendId, string toFriendId);
    Task<FriendRequestDto> GetFriendRequestAsync(int friendshipId);
    Task<FriendRequestDto> HandleFriendRequestAsync(int friendshipId, bool accepted);
   // Task<List<FriendRequestDto>> GetFriendRequestsAsync(string userId);
    Task<List<string>> GetFriendsAsync(string userId);
    Task<List<ProfileDto>> SuggestFriendsAsync(string userId);

    Task<List<PostDto>> GetTimelineReleventAsync(string userId, int page, int pageSize);
    Task<List<PostDto>> GetRandomTimelineAsync(string userId, int pageSize);
    Task<List<PostDto>> GetTimelineAsync(string userId, int page, int pageSize);
    Task<List<CountryDto>> GetCountriesAsync();
    Task<List<CityDto>> GetCitiesAsync(int countryId);
}

