using BetaCycle4.Models;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using BetaCycle4.Logger;


namespace BetaCycle4.Logic
{
    public class EmailService: IEmailService
    {
        private readonly DbTracer _dbTracer;
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration, DbTracer dbTracer) 
        {
            _dbTracer = dbTracer;
            _config = configuration;
        }

        public void sendEmail(EmailModel emailModel)
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("alberto", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };

            using(var client = new SmtpClient()) 
            {
                try
                {
                    client.Connect(_config["EmailSettings:SmtpService"], 465, true);
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception ex) 
                {
                    _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            
            }
        }
    }
}
