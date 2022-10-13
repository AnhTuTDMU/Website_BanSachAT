using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.IO;

namespace Website_BanSachAT.Controllers
{
    public class FileAndMailController : Controller
    {
        // GET: FileAndMail
        [HttpGet]
        public ActionResult SendMail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendMail(Mail model)
        {
           
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("2024802010056@student.tdmu.edu.vn", "o f i f z u m t h f q f r h f u"),
                EnableSsl = true
            };
            var mess = new MailMessage();
            mess.From = new MailAddress(model.From);
            mess.ReplyToList.Add(model.From);
            mess.To.Add(new MailAddress(model.To));
            mess.Subject = model.Subject;
            mess.Body = model.Notes;
            var f = Request.Files["attachment"];
            var path = Path.Combine(Server.MapPath("~/UploadFile"), f.FileName);
            if (!System.IO.File.Exists(path))
            {
                f.SaveAs(path);
            }
            Attachment data = new Attachment(Server.MapPath("~/UploadFile/" + f.FileName), MediaTypeNames.Application.Octet);
            mess.Attachments.Add(data);
            mail.Send(mess);
            return View("SendMail");
        }
    }
}