using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace MvcProject1.PL.Helpers
{
    public interface ISendEmail
    {
        void Send(Email email);
    }
    public class SendEmail : ISendEmail
    {
        private readonly EmailSettings _options;

        public SendEmail(IOptions<EmailSettings> options)
        {
            this._options = options.Value;
        }

        public async void Send(Email email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.UserName, _options.Email));
            message.To.Add(new MailboxAddress(email.Name, email.To));
            message.Subject = email.Subject;
            message.Body = new TextPart("html")
            {
                Text = email.Body
            };

           
            using (var client = new SmtpClient())
                {
                    client.Connect(_options.Server,_options.Port,false);
                    client.Authenticate(_options.Email, _options.Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
        }
    }
}
