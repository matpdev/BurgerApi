using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BurgerApi.Services
{
    public class EmailSender
    {
        public void SendEmail(string to, string toName, string apiKey)
        {
            var from = new MailAddress("matheus2ep@gmail.com", "Matheus Alves");
            var toAddress = new MailAddress(to, toName);

            const string fromPasword = "pqjzewjhnhawqbmq";
            const string subject = "Confirmação do Email";
            string body =
                @$"
                CONFIRME O EMAIL AGORA

                http://localhost:5232/auth/confirmemail?key={apiKey}
            ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(from.Address, fromPasword),
                Timeout = 20000,
            };

            using (
                var message = new MailMessage(from, toAddress) { Subject = subject, Body = body, }
            )
            {
                smtp.Send(message);
            }
            return;
        }
    }
}
