using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wesal.Models;

namespace BLL.Dtos.PostDto;

public class PostAndScore
{
    public Post post { get; set; }
    public int score { get; set; }
}
