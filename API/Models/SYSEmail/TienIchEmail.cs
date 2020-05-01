
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace API.Models.SYSEmail
{
    public class TienIchEmail
    {
        
        public static void sendEmail(MessageEmail message)
        {
           
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("phucbv.vnpt@gmail.com");
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject;
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Html));

            SmtpServer.Port = 465;
            SmtpServer.Credentials = new System.Net.NetworkCredential("phucbv.vnpt@gmail.com", "fgzdjbkflqskcvyp");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

        }

        public static void sendEmail(MessageEmail message, string host, int port, bool enablessl, string email, string password)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(host);
            mail.From = new MailAddress(email);
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject;
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Html));

            SmtpServer.Port = port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(email, password);
            SmtpServer.EnableSsl = enablessl;
            SmtpServer.Send(mail);
        }

        public static async Task<int> SendEmailAsync(string subject, string body, string to, string host, int port, bool enablessl, string email, string password)
        {
            try
            {
                Task task = Task.Run(() =>
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(host);
                    mail.From = new MailAddress(email);
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.Body = body;

                    SmtpServer.Port = port;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(email, password);
                    SmtpServer.EnableSsl = enablessl;
                    SmtpServer.Send(mail);
                });
                await task;
                return 1;
            }
            catch
            {
                return 0;
            }

        }

       
    }
}