using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck
{
    static class EmailService
    {
        public static void SendEmail(string tomail,string subject,string content)
        {
            try
            {
                var smtpClient = ConfigurationManager.AppSettings["smtpClient"];
                var sendEmail = ConfigurationManager.AppSettings["sendEmail"];
                var port = Int32.Parse(ConfigurationManager.AppSettings["port"]);
                var account = ConfigurationManager.AppSettings["emailAccount"];
                var password = ConfigurationManager.AppSettings["emaliPassword"];


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(sendEmail, sendEmail));
                message.To.Add(new MailboxAddress(tomail, tomail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = content };

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpClient, port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(account, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                LogHelper.Log.Error(e.Message + e.StackTrace);
            }
        }
    }
}
