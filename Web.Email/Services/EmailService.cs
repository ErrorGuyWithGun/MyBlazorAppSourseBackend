using MailKit.Net.Smtp;
using MimeKit;
using Web.Email.Model;
using Web.Email.Services;

namespace Web.Email.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        public EmailService(EmailConfiguration emailConfiguration) => 
            _emailConfiguration = emailConfiguration;
        private MimeMessage CreateEmailMessage(Message message) 
        {
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
            mimeMessage.To.AddRange(message.To);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return mimeMessage;

        }
        public void SendEmail(Message message) 
        {
 
                Send(CreateEmailMessage(message));
      
        }

        private void Send(MimeMessage message) 
        {
            using var client = new SmtpClient();
            try 
            {
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
                client.Send(message);
            }
            finally 
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
