using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerLanguage
    {
        public static List<Language> Get()
        {
            return DBHandlerLanguage.Get();
        }
    }
}