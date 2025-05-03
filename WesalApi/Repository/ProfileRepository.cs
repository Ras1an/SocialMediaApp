using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Wesal.Data;
using Wesal.Dtos.PostDto;
using Wesal.Interfaces;
using Wesal.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wesal.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _context;

        public ProfileRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Profile> CreateProfile(Profile profile)
        {
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();

            return profile;
        }

        
        public async Task<Profile> GetProfileAsync(int userId)
        {
            var profile = await _context.Profiles.FindAsync(userId);
            
            return profile;

        }

        public async Task<Profile> UpdateProfile(Profile profile)
        {
            var _profile = await _context.Profiles.FirstOrDefaultAsync(p => p.AppUserId == profile.AppUserId);

            if (_profile == null)
                return null;

            _profile.Name = profile.Name;
            _profile.DateOfBirth = profile.DateOfBirth;
            _profile.ProfilePictureLink = profile.ProfilePictureLink;
            //_profile.AppUserId = profile.AppUserId;
            _profile.Gender = profile.Gender;
            _profile.Bio = profile.Bio;
            _profile.CountryId = profile.CountryId;
            _profile.CityId = profile.CityId;

            await _context.SaveChangesAsync();

            return _profile;
        }




        // Posts


        public async Task<Post> CreatePost(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<Post> GetPost(int postId)
        {
            return await _context.Posts.Include(p => p.Comments).Include(p => p.Likes).FirstOrDefaultAsync(p => p.PostId == postId);

        }

        public async Task<Post> UpdatePost(int postId, string postText)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);

            post.PostText = postText;

            await _context.SaveChangesAsync();

            return post;
        }


        public async Task DeletePostInfo(int _postId)
        {
            var likes = await _context.Likes.Where(l => l.PostId == _postId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

        }

        public async Task<Post> DeletePost(Post _post)
        {
            await DeletePostInfo(_post.PostId);

            var post = _context.Posts.Remove(_post);
            await _context.SaveChangesAsync();

            return _post;
        }


        public async Task<List<Post>> GetTimeline(int userId)
        {
            var friendsIds = await _context.FriendShipRequests.Where(f => f.IsAccepted == true && (f.ToFriendId == userId || f.FromFriendId == userId)).Select(f => f.FromFriendId == userId ? f.ToFriendId : f.FromFriendId).Distinct().ToListAsync();

            
            /*
             await _context.Posts
                .Where(p => friendsIds.Contains(p.AppUserId)).OrderByDescending(p => p.CreatedAt).Select(p => new TimelinePostDto{ postId = p.PostId, userId = p.AppUserId , postText = p.PostText, postPhoto = p.PostPhoto, createdAt = p.CreatedAt , likeCount = p.Likes.Count()}).ToListAsync(); 

             */
            // not return posts

            //var allPosts = await _context.Posts.ToListAsync();
            //var posts = allPosts.Where(p => friendsIds.Contains(p.AppUserId)).ToList();

            //return posts;



            return await _context.Posts.Where(p => friendsIds.Contains(p.AppUserId)).ToListAsync();

            //.Where(p => friendsIds.Contains(userId)).ToListAsync();

            //.Include(p=> p.Likes).OrderByDescending(p => p.CreatedAt).ToListAsync(); 


        }

        /*
        public async Task<List<Post>> GetTimeline(string userId)
        {
            var friendsIds = (await _context.FriendShipRequests
        .Where(f => f.IsApproved && (f.ToFriendId == userId || f.FromFriendId == userId))
        .Select(f => f.FromFriendId == userId ? f.ToFriendId : f.FromFriendId)
        .ToListAsync());


            
            var allPosts = await _context.Posts.Include(p => p.Likes).OrderByDescending(p => p.CreatedAt).ToListAsync();
            var posts = allPosts.Where(p => friendsIds.Contains(p.AppUserId)).ToList();


            return posts;

            

        }
        */
        // Profiles
        public async Task<FriendShipRequest> SendFriendShipRequest(FriendShipRequest friendShipRequest)
        {
            await _context.AddAsync(friendShipRequest);
            await _context.SaveChangesAsync();

            return friendShipRequest;
        }

        
    }
}
