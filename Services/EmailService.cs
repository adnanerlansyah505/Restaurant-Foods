using System.Net;
using System.Net.Mail;
using RestaurantFoods.Services.Interfaces;

namespace RestaurantFoods.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");

        using var client = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]!),
            Credentials = new NetworkCredential(
                smtpSettings["Username"],
                smtpSettings["Password"]
            ),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(smtpSettings["From"]!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await client.SendMailAsync(mail);
    }
}