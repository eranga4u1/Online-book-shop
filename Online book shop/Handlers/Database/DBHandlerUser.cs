using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerUser
    {
        internal static ApplicationUser Update(ApplicationUser user)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var dbUser=ctx.Users.Where(x => x.Id == user.Id).FirstOrDefault();// .Add(publisher);
                    if(dbUser != null)
                    {
                        dbUser.FirstName = user.FirstName;
                        dbUser.LastName = user.LastName;
                        dbUser.Address = user.Address;
                        dbUser.ContactNumber = user.ContactNumber;
                        dbUser.Email = user.Email;
                        dbUser.UpdatedDate = DateTime.UtcNow;
                        dbUser.UpdatedBy = dbUser.Id;
                        dbUser.NickName = user.NickName;
                        dbUser.Birthday = user.Birthday;
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return dbUser;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}