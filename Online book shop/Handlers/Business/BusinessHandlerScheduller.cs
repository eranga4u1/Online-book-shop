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
        internal static bool ReleaseOldBookPack()
        {
          var packs= DBHandlerBook.GetInactivatedBookPacks();
            if(packs !=null && packs.Count > 0)
            {
                foreach(int i in packs)
                {
                    DBHandlerBook.ReleaseBookPack(i);
                }
            }

            return true;
        }
    }
}