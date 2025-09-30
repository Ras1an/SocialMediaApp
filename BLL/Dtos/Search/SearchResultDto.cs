using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WesalApi.Dtos.UserDto;
namespace BLL.Dtos.Search;
using Wesal.Dtos.PostDto;
public class SearchResultDto
{
   public List<UserDto> users;
   public List<PostDto> posts;
}
