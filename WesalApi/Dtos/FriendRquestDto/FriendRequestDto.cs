namespace WesalApi.Dtos.FriendRquestDto;

public class FriendRequestDto
{
    public int FriendShipRequestId { get; set; }

    public string FromFriendId { get; set; }
    public string name { get; set; }
    public string photoUrl { get; set; }
    public DateTime? RequestedAt { get; set; }
}
