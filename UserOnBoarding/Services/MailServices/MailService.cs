using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using UserOnBoarding.Dtos;
using MailKit.Net.Smtp;

namespace UserOnBoarding.Services.MailServices
{
    public class MailService: IMailService
    {
        public readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendMail(MailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Mail:From:Address").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("Mail:Host").Value, Convert.ToInt32(_config.GetSection("Mail:Port").Value), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("Mail:Username").Value, _config.GetSection("Mail:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
