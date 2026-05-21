using ExamSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ExamSystem.Services.Implementations;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential("sizin_sistem_maili@gmail.com", "app_parolunuz")
        };

        var mailMessage = new MailMessage("sizin_sistem_maili@gmail.com", toEmail, subject, body)
        {
            IsBodyHtml = true // Link göndərdiyimiz üçün HTML formatında gedir
        };

        await client.SendMailAsync(mailMessage);
    }
}
