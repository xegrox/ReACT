namespace ReACT.Services;

using System.Net;
using System.Net.Mail;

public class EmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendEmailAsync(string recipient, string subject, string body)
    {
        var config = _configuration.GetSection("Smtp");
        var client = new SmtpClient(config.GetValue<string>("Host"))
        {
            Port = config.GetValue<int>("Port"),
            Credentials = new NetworkCredential(
                config.GetValue<string>("Username"),
                config.GetValue<string>("Password")
            ),
            EnableSsl = true
        };

        return client.SendMailAsync(new MailMessage
        {
            From = new MailAddress(config.GetValue<string>("FromAddress")),
            To = { new MailAddress(recipient) },
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        });
    }
}
