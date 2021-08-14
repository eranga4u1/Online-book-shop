using Online_book_shop.Handlers.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerScheduller
    {
        internal static  bool UpdateOrderDescription()
        {
            return DBHandlerOrder.UpdateOrderDescription();
        } 
    }
}