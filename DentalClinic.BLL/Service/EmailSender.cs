using Microsoft.AspNetCore.Identity.UI.Services;

using System.Net;
using System.Net.Mail;


namespace kashop.bll.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("jannatbassam2001@gmail.com", "hboe tqwa lytx pbrv")
            };

            return client.SendMailAsync(
                new MailMessage(from: "jannatbassam2001@gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                )
                { IsBodyHtml = true });
        }
    }
}
