namespace WesalApi.Dtos.FriendRquestDto;
using WesalApi.Dtos.UserDto;
public class FriendRequestDto
{
    public int FriendshipRequestId { get; set; }
    public UserDto FromFriend { get; set; }
    public DateTime? RequestedAt { get; set; }
}
