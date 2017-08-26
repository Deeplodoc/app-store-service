using AppStoreService.Core;
using AppStoreService.Core.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace AppStoreService.Dal.Senders
{
    public class YandexEmailSender : ISend<Email>
    {
        public async Task SendAsync(Email item)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Интернет магазин", item.AddressFrom));
            emailMessage.To.Add(new MailboxAddress("", item.AddressTo));
            emailMessage.Subject = item.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = item.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 465, true);
                await client.AuthenticateAsync("store.onlinestore@yandex.ru", "Cypresshill2016");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}