using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
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
        internal static bool RevertPromotion()
        {
            var promotions = DBHandlerPromotion.GetExpiredPromotion();
            if(promotions != null && promotions.Count > 0)
            {
                foreach (Promotion p in promotions)
                {
                    DBHandlerPromotion.SetToDefaultPromotion(p);
                }
            }
            return true;
        }

        internal static bool BackUpDatabase()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var result = ctx.Database.SqlQuery<BookVMTile>("exec sp_BackUPDB").ToList();
                    if (result != null)
                    {
                        BusinessHandlerMPLog.Log(LogType.Message, "Backup done", "BackUpDatabase", "BusinessHandlerScheduller");
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Message, "DB BackUP Failed", "BackUpDatabase", "BusinessHandlerScheduller");
            }
            return true;
        }
    }
}