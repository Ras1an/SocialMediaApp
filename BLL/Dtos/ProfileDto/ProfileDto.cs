namespace WesalApi.Dtos.ProfileDto;

public class ProfileDto
{
    public string AppUserId { get; set; }
    public string name { get; set; }
    public string profilePictureLink { get; set; }

    public FriendStatus? friendStatus { get; set; }

}


public enum FriendStatus
{
    NotFriend = 1,
    PendingSent = 2,
    PendingReceived = 3,
    Friend = 4,
}
