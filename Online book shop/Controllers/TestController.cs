using Online_book_shop.Handlers.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public string SendTestMail()
        {
            EmailHandler.TestEmail2();
            return EmailHandler.Email("<h1>This is test email<h1/>", "noreply@musespublishers.com", "eranga.kdy@gmail.com", "This is test mail");
           // return "Done";
        }
        public string SendTestMailWithAttachment()
        {
            return EmailHandler.SendMailWithAttachment("<h1>This is test email<h1/>", "noreply@musespublishers.com", "eranga.kdy@gmail.com", "This is test mail", "\\Content\\UploadFiles\\Invoices", "Invoice_3044_2041.pdf");
            // return "Done";
        }
    }
}