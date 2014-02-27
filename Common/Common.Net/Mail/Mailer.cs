using System.Net;
using System.Net.Mail;
using System.Text;

namespace Common.Net.Mail
{
    /// <summary>
    /// Helper для отправки e-mail сообщений
    /// </summary>
    public static class Mailer
    {
        /// <summary>
        /// Отправляет e-mail
        /// </summary>
        /// <param name="server">Адрес SMTP-сервера</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="fromAddress">Адрес отправителя</param>
        /// <param name="fromName">Имя отправителя</param>
        /// <param name="toAddresses">Адреса получателей, разделенные запятыми</param>
        /// <param name="subject">Тема</param>
        /// <param name="body">Тело сообщения</param>
        public static void Send(string server, string login, string password, string fromAddress, string fromName, string toAddresses, string subject, string body)
        {
            MailMessage message = CreateMessage(fromAddress, fromName, toAddresses, subject, body);
            Send(server, login, password, message);
        }
        /// <summary>
        /// Отправляет e-mail
        /// </summary>
        /// <param name="server">Адрес SMTP-сервера</param>
        /// <param name="fromAddress">Адрес отправителя</param>
        /// <param name="fromName">Имя отправителя</param>
        /// <param name="toAddresses">Адреса получателей, разделенные запятыми</param>
        /// <param name="subject">Тема</param>
        /// <param name="body">Сообщение</param>
        public static void Send(string server, string fromAddress, string fromName, string toAddresses, string subject, string body)
        {
            MailMessage message = CreateMessage(fromAddress, fromName, toAddresses, subject, body);
            Send(server, message);
        }
        /// <summary>
        /// Отправляет e-mail
        /// </summary>
        /// <param name="server">Адрес SMTP-сервера</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="message">E-mail сообщение</param>
        public static void Send(string server, string login, string password, MailMessage message)
        {
            SmtpClient client = new SmtpClient(server)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(login, password)
            };
            client.Send(message);
        }
        /// <summary>
        /// Отправляет e-mail
        /// </summary>
        /// <param name="server">Адрес SMTP-сервера</param>
        /// <param name="message">E-mail сообщение</param>
        public static void Send(string server, MailMessage message)
        {
            SmtpClient client = new SmtpClient(server)
            {
                UseDefaultCredentials = true
            };
            client.Send(message);
        }
        /// <summary>
        /// Создает E-mail сообщение
        /// </summary>
        /// <param name="fromAddress">Адрес отправителя</param>
        /// <param name="fromName">Имя отправителя</param>
        /// <param name="toAddresses">Список получателей, разделенный запятыми</param>
        /// <param name="subject">Тема</param>
        /// <param name="body">Тело сообщения</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>E-mail сообщение</returns>
        public static MailMessage CreateMessage(string fromAddress, string fromName, string toAddresses, string subject, string body, Encoding encoding)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromAddress, fromName)
            };
            mail.To.Add(toAddresses);
            mail.Subject = subject;
            mail.SubjectEncoding = encoding;
            mail.Body = body;
            mail.BodyEncoding = encoding;
            mail.IsBodyHtml = true;
            return mail;
        }
        /// <summary>
        /// Создает E-mail сообщение
        /// </summary>
        /// <param name="fromAddress">Адрес отправителя</param>
        /// <param name="fromName">Имя отправителя</param>
        /// <param name="toAddresses">Список получателей, разделенный запятыми</param>
        /// <param name="subject">Тема</param>
        /// <param name="body">Тело сообщения</param>
        /// <returns>E-mail сообщение</returns>
        public static MailMessage CreateMessage(string fromAddress, string fromName, string toAddresses, string subject, string body)
        {
            return CreateMessage(fromAddress, fromName, toAddresses, subject, body, Encoding.UTF8);
        }
    }
}