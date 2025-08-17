using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;
using WesalApi.Dtos.UserDto;

namespace BLL.Dtos.LikeDto;

public partial class LikeDto
{
    public int LikeId { get; set; }

    public UserDto user { get; set; }


}

