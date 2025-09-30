using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Data;
using Wesal.Models;

namespace DAL.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateLikeAsync(Like like)
        {
            var postFounded = await _context.Posts.FirstOrDefaultAsync(post => post.PostId == like.PostId);

            if (postFounded == null)
                return false;

            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteLikeAsync(string userId, int postId)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(like => like.PostId ==  postId && like.AppUserId == userId);

            if(like == null)
                return false;

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<List<int>> IsLiked(string userId, List<int> postIds)
        {
            return await _context.Likes.Where(l => postIds.Contains(l.PostId) && l.AppUserId == userId).Select(l => l.PostId).ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetLikesCounts(List<int> postIds)
        {
            return await _context.Likes.Where(l => postIds.Contains(l.PostId))
                .GroupBy(l => l.PostId).Select(g => new {postId = g.Key, count = g.Count() })
                .ToDictionaryAsync(x => x.postId, x => x.count);
        }

        public Task<List<Like>> GetPostLikesAsync(int postId, int page, int pageSize)
        {
            return _context.Likes.Where(l => l.PostId == postId).Skip((page - 1) * pageSize).Take(pageSize).Include(l => l.AppUser.Profiles).ToListAsync();
        }
    }
}
