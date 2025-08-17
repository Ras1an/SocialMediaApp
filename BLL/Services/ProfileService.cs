using AutoMapper;
using Azure.Core;
using BLL.Interfaces.Services;
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

namespace BLL.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepo;
    private readonly IMapper _mapper;

    public ProfileService(IProfileRepository profileRepo, IMapper mapper)
    {
        _profileRepo = profileRepo;
        _mapper = mapper;
    }

    public async Task<ProfileDto> GetProfileAsync(string userId){
        var profile = await _profileRepo.GetProfileAsync(userId);

        return _mapper.Map<ProfileDto>(profile);
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
        var friendShipRequest = new FriendShipRequest
        {
            FromFriendId = fromFriendId,
            ToFriendId = toFriendId
        };

        var friendRequest = _profileRepo.SendFriendShipRequest(friendShipRequest);

        return _mapper.Map<FriendRequestDto>(friendRequest);
    }


    public async Task<FriendRequestDto> GetFriendRequestAsync(int friendshipId) {

        var friendRequest = await _profileRepo.GetFriendShipRequest(friendshipId);

        return _mapper.Map<FriendRequestDto>(friendRequest);

    }


    public async Task<FriendRequestDto> HandleFriendRequestAsync(int friendshipId, bool accepted) {

        var friendRequest = await _profileRepo.HandelFriendshipRequest(friendshipId, accepted);

        return _mapper.Map<FriendRequestDto>(friendRequest);
    }


    //public async Task<List<FriendRequestDto>> GetFriendRequestsAsync(string userId) {
    //    var friendRequests = await _profileRepo.GetFriendShipRequests(userId);

    //    return _mapper.Map<List<FriendRequestDto>>(friendRequests);
    //}
        

    public async Task<List<string>> GetFriendsAsync(string userId) =>
        await _profileRepo.GetFriends(userId);

    public async Task<List<ProfileDto>> SuggestFriendsAsync(string userId) { 
        var suggestedFriends = await _profileRepo.SuggestFriends(userId);

        return _mapper.Map<List<ProfileDto>>(suggestedFriends);
    }
    public async Task<List<PostDto>> GetTimelineAsync(string userId, int page, int pageSize) { 
        var posts = await _profileRepo.GetTimeline(userId, page, pageSize);

        return _mapper.Map<List<PostDto>>(posts);
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

