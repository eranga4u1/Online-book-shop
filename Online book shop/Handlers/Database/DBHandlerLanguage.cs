using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerLanguage
    {
        public static List<Language> Get()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                 return   ctx.Languages.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}