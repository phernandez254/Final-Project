using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace HITEF.Models
{
    public class EmailHandler
    {
        public async static Task SendEmailAsync(string _subject, string _message)
        {
            try
            {
                var emailSenderAddress = ConfigurationManager.AppSettings["EmailSenderAddress"];
				var emailSenderPassword = ConfigurationManager.AppSettings["EmailSenderPassword"];
                var displayName = ConfigurationManager.AppSettings["DisplayName"];
				var emailReceiverAddress = ConfigurationManager.AppSettings["EmailReceiverAddress"];

				MailMessage myEmail = new MailMessage();
                myEmail.From = new MailAddress(emailSenderAddress, displayName);
				myEmail.To.Add(emailReceiverAddress);
				myEmail.Subject = _subject;
                myEmail.Body = _message;
                myEmail.IsBodyHtml = true;

                using (SmtpClient mySMTPClient = new SmtpClient())
                {
                    mySMTPClient.EnableSsl = true;
                    mySMTPClient.Host = "smtp.gmail.com";
                    mySMTPClient.Port = 587; // TSl is 587, SSL is 465 and another SSL option is 25. TSL works fine.
                    mySMTPClient.UseDefaultCredentials = false;
                    mySMTPClient.Credentials = new NetworkCredential(emailSenderAddress, emailSenderPassword);
                    mySMTPClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mySMTPClient.SendCompleted += (s, e) => { mySMTPClient.Dispose(); };
                    await mySMTPClient.SendMailAsync(myEmail);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}