using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SMTP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Mail From (enter your e-mail): ");
            MailAddress mailFrom = new MailAddress(Console.ReadLine());
            Console.Write("Enter your login (login@mail.com): ");
            string login = Console.ReadLine();
            Console.Write("Enter your pass: ");
            string pass = String.Empty;
            bool isEnter = true;
            while (isEnter)
            {
                char s = Console.ReadKey(true).KeyChar;
                if (s == '\r')
                {
                    isEnter = false;
                }
                else
                {
                    pass += s;
                }
            }
            Console.Write("\nMail To (enter receiver e-mail): ");
            MailAddress mailTo = new MailAddress(Console.ReadLine());

            MailMessage msg = new MailMessage(mailFrom, mailTo);

            Console.Write("Subject of message: ");
            msg.Subject = Console.ReadLine();
            Console.Write("Your Message: ");
            msg.Body = Console.ReadLine();

            Console.Write("Enter your host-mail: ");
            string host = Console.ReadLine();
            Console.Write("Enter port: ");
            string port = Console.ReadLine();

            SmtpClient mailClient = new SmtpClient();
            mailClient.Host = "smtp." + host;
            mailClient.Port = Int32.Parse(port);
            mailClient.EnableSsl = true;
            mailClient.Credentials = new NetworkCredential(login, pass);

            Console.WriteLine("Sending message...");
            mailClient.Send(msg);
            Console.WriteLine("Message sent");
        }
    }
}
