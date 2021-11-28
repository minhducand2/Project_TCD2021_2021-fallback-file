using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers
{
    public class Email
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }

    [Route("api/[controller]/[action]")]
    public class MailController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SendMail([FromBody] Email email)
        {
            var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 465);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;

            client.Credentials = new System.Net.NetworkCredential("luongthanhbinh45@gmail.com", "efvnihlfxrlcjqzr");

            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress("luongthanhbinh45@gmail.com");

            mailMessage.To.Add(email.To);

            if (!string.IsNullOrEmpty(email.Cc))
            {
                mailMessage.CC.Add(email.Cc);
            }

            mailMessage.Body = email.Text;

            mailMessage.Subject = email.Subject;

            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

            await client.SendMailAsync(mailMessage);

            return Ok();
        }
    }
}