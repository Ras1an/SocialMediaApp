using BLL.Dtos.GetCommentDto;
using BLL.Dtos.LikeDto;
using Wesal.Models;
using WesalApi.Dtos.UserDto;

namespace Wesal.Dtos.PostDto;

public class PostDto
{
    public int postId { get; set; }
    public UserDto user { get; set; }
    public string postText { get; set; }
    public string postPhoto { get; set; }
    public DateTime? createdAt { get; set; }
    public bool? isLiked { get; set; }
    public List<GetCommentDto> comments { get; set; }

    public List<LikeDto> likes { get; set; }

}