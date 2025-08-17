using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Dtos.CommentDto;
using BLL.Dtos.GetCommentDto;
using BLL.Dtos.LikeDto;
using Wesal.Dtos.PostDto;
using Wesal.Models;
using WesalApi.Dtos.CommentDto;
using WesalApi.Dtos.FriendRquestDto;
using WesalApi.Dtos.ProfileDto;
using WesalApi.Dtos.UserDto;

namespace BLL.Mapper;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<AppUser, UserDto>().ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id)).ForMember(des => des.Name, opt => opt.MapFrom(src => src.Profiles.FirstOrDefault().Name)).ForMember(des => des.photoLink, opt => opt.MapFrom(src => src.Profiles.FirstOrDefault().ProfilePictureLink)).ReverseMap();
        CreateMap<Like, LikeDto>().ForMember(des => des.user, opt => opt.MapFrom(src => src.AppUser)).ReverseMap();
        CreateMap<Post, PostDto>().ForMember(des => des.user, opt => opt.MapFrom(src => src.AppUser)).ReverseMap();
        CreateMap<Comment, CreateCommentDto>().ReverseMap();
        CreateMap<Comment, GetCommentDto>().ForMember(des => des.user, opt => opt.MapFrom(src => src.AppUser)).ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Wesal.Models.Profile, ProfileDto>().ReverseMap();
        CreateMap<FriendShipRequest, FriendRequestDto>().ReverseMap();
        // add other mappings here
    }
}

