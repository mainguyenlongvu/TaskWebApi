using System.Net;
using System.Net.Mail;

namespace TaskWebApi.CoreHelper.Helper
{
    public class SendMail
    {
        public bool SendEmail(string to, string subject, string body)
        {
            try
            {
                MailMessage msg = new MailMessage(ConstantHelper.emailSender, to, subject, body);

                using (var client = new SmtpClient(ConstantHelper.hostEmail, ConstantHelper.portEmail))
                {
                    client.EnableSsl = true;
                    NetworkCredential credential = new NetworkCredential(ConstantHelper.emailSender, ConstantHelper.passwordSender);
                    client.UseDefaultCredentials = false;
                    client.Credentials = credential;
                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
