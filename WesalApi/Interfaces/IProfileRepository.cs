using Wesal.Dtos.PostDto;
using Wesal.Dtos.ProfileDto;
using Wesal.Models;

namespace Wesal.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> CreateProfile(Profile profile);
        Task<Profile> UpdateProfile(Profile profile);

        Task<Profile> GetProfileAsync(int userId);

        // posts

        Task<Post> CreatePost(Post post);
        Task<Post> GetPost(int postId);
        Task<Post> UpdatePost(int postId,string postText);

        Task DeletePostInfo(int _postId);
        Task<Post> DeletePost(Post _post);

        Task <List<Post>> GetTimeline(int userId);


        // profiles
        
        Task<FriendShipRequest> SendFriendShipRequest(FriendShipRequest friendShipRequest);
        
    }
}
