using Web.Email.Model;

namespace Web.Email.Services
{
    public interface IEmailService
    {
        public void SendEmail(Message message);
    }
}
