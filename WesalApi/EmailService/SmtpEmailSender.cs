using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using BLL.Interfaces.EmailService;
using MailKit.Security;

namespace WesalApi.EmailService;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _s;

    public SmtpEmailSender(IOptions<SmtpSettings> options)
    {
        _s = options.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string html)
    {

        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_s.fromName, _s.from));
        msg.To.Add(MailboxAddress.Parse(to));
        msg.Subject = subject;
        msg.Body = new TextPart("html") { Text = html };

        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true; // for local dev only not production

        await client.ConnectAsync(_s.host, _s.port, SecureSocketOptions.StartTls);
        if (!String.IsNullOrEmpty(_s.username))
            await client.AuthenticateAsync(_s.username, _s.password);
        await client.SendAsync(msg);
        await client.DisconnectAsync(true);
    }
}
