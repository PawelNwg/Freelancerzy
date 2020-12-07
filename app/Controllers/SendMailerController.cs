using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Web;
using app.Models;

namespace freelancerzy.Controllers
{
    public class SendMailerController : Controller
    {
        public IActionResult Email()
        {
            return View();
        }
        //https://localhost:44326/SendMailer/Email
        [HttpPost]
        public ViewResult Email(freelancerzy.Models.Mail _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("pawucio13@gmail.com", "T6dcejPehRC5SU"); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Email", _objModelMail);
            }
            else
            {
                return View();
            }
        }
    }
}
