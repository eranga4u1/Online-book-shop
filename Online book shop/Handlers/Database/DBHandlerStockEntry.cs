using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerStockEntry
    {
        public static StockEntry Update(StockEntry stockEntry)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.StockEntries.Add(stockEntry);
                    ctx.SaveChanges();
                }
                return stockEntry;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool Update(List<StockEntry> items)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.StockEntries.AddRange(items);
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}