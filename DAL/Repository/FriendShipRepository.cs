using BLL.Interfaces;
using BLL.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Data;
using Wesal.Models;

namespace DAL.Repository;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly AppDbContext _context;

    public FriendshipRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<FriendshipRequest?> GetFriendshipAsync(string userId1, string userId2)
    {
        return await _context.FriendshipRequests
                         .FirstOrDefaultAsync(f => (f.ToFriendId == userId1 && f.FromFriendId == userId2) || (f.ToFriendId == userId2 && f.FromFriendId == userId1));

    }

    public async Task<bool> DeleteFriendshipAsync(FriendshipRequest friendship)
    {
        _context.FriendshipRequests.Remove(friendship);
        await _context.SaveChangesAsync();

        return true;
    }


    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }


}
