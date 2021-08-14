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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult  UpdateNickName(UserDataVM model)
        {
            JsonResult jr = new JsonResult();
            if (!string.IsNullOrEmpty(model.NickName))
            {
                ApplicationUser user = BusinessHandlerUser.GetApplicationUser();
                user.NickName = model.NickName;
                BusinessHandlerUser.Update(user);
            }
            return jr;
        }


        [Authorize]
        public ActionResult UpdateBirthday(UserDataVM model)
        {
            JsonResult jr = new JsonResult();
            if (!string.IsNullOrEmpty(model.Birthday))
            {
                DateTime d = DateTime.Parse(model.Birthday);
                ApplicationUser user = BusinessHandlerUser.GetApplicationUser();
                user.Birthday = d;
                BusinessHandlerUser.Update(user);
            }
            return jr;
        }



        [Authorize]
        public ActionResult UpdateEmail(UserDataVM model)
        {
            JsonResult jr = new JsonResult();
            if (!string.IsNullOrEmpty(model.Email))
            {
                ApplicationUser user = BusinessHandlerUser.GetApplicationUser();
                user.Email = model.Email;
                BusinessHandlerUser.Update(user);
            }
            return jr;
        }

        [Authorize]
        public ActionResult RemoveAddress(Address model)
        {
            JsonResult jr = new JsonResult();

            if (BusinessHandlerAddress.remove(model.Id))
            {
                jr.Data = "success";
            }
            else
            {
                jr.Data = "failed";
            }
              
        
            return jr;
        }

        [Authorize]
        public ActionResult SetDefault(Address model)
        {
            JsonResult jr = new JsonResult();

            if (BusinessHandlerAddress.setDefault(model.Id))
            {
                jr.Data = "success";
            }
            else
            {
                jr.Data = "failed";
            }


            return jr;
        }

    }
}