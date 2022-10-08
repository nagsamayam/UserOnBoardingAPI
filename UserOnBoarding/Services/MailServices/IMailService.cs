using UserOnBoarding.Dtos;

namespace UserOnBoarding.Services.MailServices
{
    public interface IMailService
    {
        void SendMail(MailDto request);
    }
}
