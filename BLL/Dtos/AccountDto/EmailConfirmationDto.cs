using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.AccountDto;

public class EmailConfirmationDto
{
    public string userId { get; set; }

    public string token { get; set; }
}
