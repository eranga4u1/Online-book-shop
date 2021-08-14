using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerUser
    {
        internal static ApplicationUser GetApplicationUser()
        {
            return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        }
        internal static ApplicationUser Update(ApplicationUser user)
        {
           return DBHandlerUser.Update(user);
        }
        public static ApplicationUser GetUserById(string id)
        {
            return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(id);

        }
        public static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        public static List<string> GetLoginUserRoles()
        {
            // get the user manager from the owin context
            ApplicationUserManager userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            string userId = HttpContext.Current.User.Identity.GetUserId();

            // get user roles
            return userManager.GetRoles(userId).ToList();
        }
    }
}