using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WesalApi.Dtos.UserDto;

namespace BLL.Dtos.GetCommentDto;

public class GetCommentDto
{
        public int CommentId { get; set; }

        public UserDto user { get; set; }

        public string? CommentText { get; set; }

        public DateTime? CreatedAt { get; set; }


    }
