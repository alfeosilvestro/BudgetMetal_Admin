using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Com.BudgetMetal.Common
{
    public class SendingMail
    {
        public void SendMail(string toMail, string cCMail, string subject, string body)
        {
            
            SmtpClient client = new SmtpClient("mail.mritmyanmar.com");
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("info@mritmyanmar.com", "nnhhyy66");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@mritmyanmar.com");
            mailMessage.To.Add(toMail);
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
        }
    }
}
