using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace YourApiNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailModel emailModel)
        {
            if (emailModel == null || string.IsNullOrEmpty(emailModel.to))
            {
                return BadRequest("Invalid email data.");
            }

            try
            {
                using (MailMessage mail = new MailMessage(emailModel.from, emailModel.to))
                {
                    mail.Subject = emailModel.subject;
                    mail.Body = emailModel.body; 

                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                    {
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(emailModel.from, emailModel.password);
                        smtp.Port = 587;

                        await smtp.SendMailAsync(mail);
                    }
                }

                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Email sending failed: {ex.Message}");
            }
        }
    }
}
