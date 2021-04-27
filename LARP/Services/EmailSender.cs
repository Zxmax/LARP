using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace LARP.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var message = new MailMessage();
            message.To.Add(new MailAddress(email, email));
            message.From = new MailAddress("abhuvwxyz@hotmail.com", "TestEmailSender");
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            using var client = new SmtpClient("smtp.live.com", 25)
            {
                Port = 25,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("abhuvwxyz@hotmail.com", "zxm19960614,"),
                EnableSsl = true
            };
            client.Send(message);
            return Task.CompletedTask;
        }
    }
}
