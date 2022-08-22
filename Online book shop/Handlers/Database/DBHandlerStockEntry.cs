using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
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

        internal static List<BookStat> GetBookStats(FilterByDate model)
        {
            try
            {
                List<BookStat> result = new List<BookStat>();
                using (var ctx = new ApplicationDbContext())
                {
                    List<BookStat> entryList = (from a in ctx.StockEntries.Where(stockEntry => stockEntry.Operation == "Out" &&
                     (stockEntry.CreatedDate >= model.fromdate && stockEntry.CreatedDate <= model.todate))
                                    join b in ctx.Books on a.BookId equals b.Id
                                    select new BookStat { Name = b.Title, NumberOfBooks = a.NumberOfBook, BookId = b.Id }).ToList();
                                    if(entryList != null)
                                    {
                                        result = entryList
                                           .GroupBy(l => l.BookId)
                                           .Select(cl => new BookStat
                                           {
                                               BookId = cl.FirstOrDefault().BookId,
                                               NumberOfBooks = cl.Sum(c => c.NumberOfBooks),
                                               Name = cl.FirstOrDefault().Name,
                                           }).OrderByDescending(x=> x.NumberOfBooks).ToList();
                                    }                   
                }
                return result;
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