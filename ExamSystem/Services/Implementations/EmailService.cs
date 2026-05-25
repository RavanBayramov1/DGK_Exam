using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ExamSystem.Services.Implementations;

public class EmailService(IConfiguration _config) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Email:SenderEmail"],
                _config["Email:AppPassword"]
            )
        };

        using var mailMessage = new MailMessage(
            _config["Email:SenderEmail"]!, toEmail, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mailMessage);
    }
}
