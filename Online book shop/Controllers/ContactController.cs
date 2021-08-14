using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            List<ContactNumber> Obj__contactNumbers = new List<ContactNumber>();
            List<EmailAddress> Obj__emailAddress = new List<EmailAddress>();
            List<MusesAddress> Obj_address = new List<MusesAddress>();

            Configuration conf_contactNumbers = BusinessHandlerConfigurations.GetConfigByKey("CONTACT_NUMBERS");
            Configuration conf_emailAddress = BusinessHandlerConfigurations.GetConfigByKey("EMAIL_ADDRESS");
            Configuration conf_address = BusinessHandlerConfigurations.GetConfigByKey("ADDRESS");


            if (conf_contactNumbers != null && !string.IsNullOrEmpty(conf_contactNumbers.Value))
            {
                Obj__contactNumbers = JsonConvert.DeserializeObject<List<ContactNumber>>(conf_contactNumbers.Value);
            }
            if (conf_emailAddress != null && !string.IsNullOrEmpty(conf_emailAddress.Value))
            {
                Obj__emailAddress = JsonConvert.DeserializeObject<List<EmailAddress>>(conf_emailAddress.Value);
            }
            if (conf_address != null && !string.IsNullOrEmpty(conf_address.Value))
            {
                Obj_address = JsonConvert.DeserializeObject<List<MusesAddress>>(conf_address.Value);
            }

            ViewBag.contactNumbers = Obj__contactNumbers;
            ViewBag.emailAddress = Obj__emailAddress;
            ViewBag.address = Obj_address;
            return View();
        }
    }
}