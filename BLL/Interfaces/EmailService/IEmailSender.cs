using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.EmailService;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string html);
}
