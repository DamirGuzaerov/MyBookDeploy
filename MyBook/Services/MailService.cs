using MyBook.Core.Interfaces;
using System.Net;
using System.Net.Mail;

namespace MyBook.Services
{
    public class MailService : IMailService
    {
        const string companyEmail = "mybooksemestrovka@gmail.com";
        const string companyPass = "mybook@semestrovka.com";
        const string companyName = "MyBook"; 
        private void SendEmailDefault(string emailAddress, string emailTheme, string bodyHtml)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(companyEmail, companyName); 
                message.To.Add(emailAddress);
                message.Subject = emailTheme; 
                message.Body = bodyHtml;
               // message.Attachments.Add(new Attachment("... путь к файлу ...")); //добавить вложение к письму при необходимости

                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com")) //используем сервера Google
                {
                    client.Credentials = new NetworkCredential(companyEmail,companyPass); //логин-пароль от аккаунта
                    client.Port = 587; //порт 587 либо 465
                    client.EnableSsl = true; //SSL обязательно
                    client.Send(message);
                }
            }
            catch (Exception e)
            {
                //TODO show error
            }
        }

        public void SendGiftSubscr(string email)
        {
            var body = "<img src=\"https://i2.mybook.io/next/assets/buyGift/selectionImage0.png\"><br>";
            SendEmailDefault(email, "Gift for ya", body);
        }
    }
}
