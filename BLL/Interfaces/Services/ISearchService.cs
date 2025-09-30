using BLL.Dtos.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Dtos.PostDto;
using WesalApi.Dtos.UserDto;

namespace BLL.Interfaces.Services
{
    public interface ISearchService
    {
        Task<SearchResultDto> SearchPostsAndUsersAsync(string userId, string target, int page, int pageSize);
        Task<List<PostDto>> SearchPostsAsync(string userId, string target, int page, int pageSize);
        Task<List<UserDto>> SearchUsersAsync(string target, int page, int pageSize);
    }
}
