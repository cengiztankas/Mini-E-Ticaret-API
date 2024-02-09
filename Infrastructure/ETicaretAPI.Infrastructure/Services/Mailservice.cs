using ETicaretAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class Mailservice : IMailService
    {
        readonly IConfiguration _configuration;

        public Mailservice(IConfiguration configuration)
        {
            _configuration = configuration;
        }

  

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] {to},subject,body,isBodyHtml); // sendMessageAsync func çogulunu çağrıdık.
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            mail.Subject = subject;
            mail.Body = body;
            foreach (var to in tos)
            {
                mail.To.Add(to);
            }
            mail.From = new(_configuration["Mail:Username"], _configuration["Mail:MailDisplayName"],System.Text.Encoding.UTF8);


            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port =int.Parse(_configuration["Mail:Port"]);
            smtp.Host = _configuration["Mail:Host"];
            smtp.EnableSsl = bool.Parse(_configuration["Mail:EnableSSL"]);
         await   smtp.SendMailAsync(mail);

        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            //StringBuilder mail = new();
            //mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"");
            //mail.AppendLine(_configuration["AngularClientUrl"]);
            //mail.AppendLine("/auth/password-update/");
            //mail.AppendLine(userId);
            //mail.AppendLine("/");
            //mail.AppendLine(resetToken);
            //mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>NG - Mini|E-Ticaret");
         
            string baseurl = _configuration["AngularClientUrl"];
            string mymail = $"Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target='_blank' href='{baseurl}/auth/password-update/{userId}/{resetToken}'>Yeni şifre talebi için tıklayınız...</a>";
            await SendMailAsync(to, "Şifre Yenileme Talebi", mymail.ToString());
        }

        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName)
        {

            string mail = $"Sayın {userName} Merhaba<br>" +
                $"{orderDate} tarihinde vermiş olduğunuz {orderCode} kodlu siparişiniz tamamlanmış ve kargo firmasına verilmiştir.<br>Hayrını görünüz efendim...";

            await SendMailAsync(to, $"{orderCode} Sipariş Numaralı Siparişiniz Tamamlandı", mail);
        }
    }
}
