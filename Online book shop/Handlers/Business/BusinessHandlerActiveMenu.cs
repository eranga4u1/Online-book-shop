using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Handlers.Database;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerActiveMenu
    {
        public static int GetActiveMenuByController(string controller,string method=null)
        {
            try
            {
                return DBHandlerActiveMenu.GetActiveMenuByController(controller, method);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}