using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.AccountDto;

public class ChangePasswordDto
{
    [Required]
    public string currentPassword { get; set; }

    [Required]
    [MinLength(6)]
    public string newPassword { get; set; }
}
