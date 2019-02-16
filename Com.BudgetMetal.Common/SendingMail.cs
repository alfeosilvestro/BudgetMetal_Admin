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
            try
            {
                //SmtpClient client = new SmtpClient("mail.google.com");
                //client.Port = 25;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new NetworkCredential("contact@thantsinaung.com", "nnhhyy66");

                //MailMessage mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress("contact@thantsinaung.com");
                //mailMessage.To.Add(toMail);
                //mailMessage.Body = body;
                //mailMessage.Subject = subject;
                //client.Send(mailMessage);
                //info@mritmyanmar.com
                var fromAddress = new MailAddress("ezytender@gmail.com", "EzyTender");

                //var fromAddress = new MailAddress("info@mritmyanmar.com", "EzyTender");
                var toAddress = new MailAddress(toMail, toMail);
                const string fromPassword = "nnhhyy66";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                //add error log
            }

        }
    }
}
