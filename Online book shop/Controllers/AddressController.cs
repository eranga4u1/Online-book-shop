using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
namespace Online_book_shop.Controllers
{
    public class AddressController : Controller
    {
        // GET: Address
        public ActionResult Index()
        {
           
            ViewBag.CourierCharges = BusinessHandlerDeliveryCharges.GetCourierCharges();
            Configuration conf_district = BusinessHandlerConfigurations.GetConfigByKey("DISTRICT_SRILANKA");
            Configuration conf_countries = BusinessHandlerConfigurations.GetConfigByKey("COUNTRIES");
            ViewBag.District = JsonConvert.DeserializeObject<List<string>>(conf_district.Value);
            ViewBag.Countries = JsonConvert.DeserializeObject<List<string>>(conf_countries.Value);
            if (!string.IsNullOrEmpty(Request.QueryString["location"]))
            {
                ViewBag.Type = Request.QueryString["location"];
            }
            else
            {
                ViewBag.Type = "user-profile-new";
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            Address address= BusinessHandlerAddress.GetAddress(id);
            Configuration conf_district = BusinessHandlerConfigurations.GetConfigByKey("DISTRICT_SRILANKA");
            Configuration conf_countries = BusinessHandlerConfigurations.GetConfigByKey("COUNTRIES");
            ViewBag.District = JsonConvert.DeserializeObject<List<string>>(conf_district.Value);
            ViewBag.Countries = JsonConvert.DeserializeObject<List<string>>(conf_countries.Value);
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                ViewBag.Type = Request.QueryString["type"];
            }
            else
            {
                ViewBag.Type = "user-profile-new";
            }
            return View(address);
        }

        [HttpPost]
        public JsonResult  Add(Address model)
        {
            JsonResult jr = new JsonResult();
            if(model.Country!= "Sri Lanka")
            {
                model.District = "All";
            }
            if (model.Id>0)
            {
                jr.Data = BusinessHandlerAddress.Edit(model);
            }
            else
            {
                jr.Data = BusinessHandlerAddress.Add(model);
            }
            
            return jr;
        }
    }
}