using Demo.PL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("norhanmahmod13@gmail.com", "jzpoaanahlsbnjru");
            client.Send("norhanmahmod13@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
