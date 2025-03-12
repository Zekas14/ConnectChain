using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ConnectChain.Settings;

namespace ConnectChain.Helpers
{
    public interface IMailServices
    {
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachment = null);

    }

    public class MailServices(IOptions<MailSetting> mailSetting): IMailServices
    {
        private readonly MailSetting _mailSetting = mailSetting.Value;

        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachment = null)
        {

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse("ahmedzrks123@gmail.com"),
                Subject = subject,

            };
            email.To.Add(MailboxAddress.Parse(mailTo));
            var builder = new BodyBuilder();
            if (attachment != null)
            {
                byte[] fileBytes;
                foreach (var item in attachment)
                {
                    if (item.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        item.CopyTo(ms);
                        fileBytes = ms.ToArray();
                        builder.Attachments.Add(item.FileName, fileBytes, ContentType.Parse(item.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress("ConnectChain", "ahmedzrks123@gmail.com"));
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com",587 , MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("ahmedzrks123@gmail.com", "wdjz zaaw dcmq mkev");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }
    }
}
