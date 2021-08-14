using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerRecentlyVisitedItems
    {
        internal static List<BookVMTile> GetRecentViewsByUser(string userId)
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    RecentyViewItems recentyViewItems= ctx.RecentyViewItems.Where(x => x.UserId == userId).FirstOrDefault();
                    if(recentyViewItems != null && !string.IsNullOrEmpty(recentyViewItems.RecentlyVisitedItems) )
                    {
                        IEnumerable<int> bookIds = BusinessHandlerRecentlyVisitedItems.StringToIntList(recentyViewItems.RecentlyVisitedItems);
                        if(bookIds !=null && bookIds.Count()>0)
                        {
                            var bookResults = from a in (from q in ctx.Books
                                                         where bookIds.Any(y => y == q.Id) && !q.isDeleted
                                                         orderby q.CreatedDate descending
                                                         select q)
                                              join b in ctx.Authors on a.AuthorId equals b.Id
                                              select new BookVMTile
                                              {
                                                  Id = a.Id,
                                                  BookName = a.Title,
                                                  LocalBookName = a.LocalTitle,
                                                  AuthorName = b.Name,
                                                  LocalAuthorName = b.LocalName,
                                                  isDeleted = b.isDeleted,
                                                  Rating = a.Ratings,
                                                  SaleType = a.SaleType,
                                                  Url = a.FriendlyName,
                                                  CreatedDate = a.CreatedDate,
                                                  Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                                  Categories = (from r in (from t in ctx.Book_Categories
                                                                           where t.BookId == a.Id && !t.isDeleted
                                                                           select t)
                                                                join
                                              c in ctx.Categories on r.CategoryId equals c.Id
                                                                select c).ToList(),
                                                  FrontCover = (from x in (from s in ctx.BookProperties
                                                                           where s.BookId == a.Id
                                                                           select s)
                                                                join
                                                                y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                                select y).ToList().FirstOrDefault()
                                              };
                            var test = bookResults.ToList<BookVMTile>();
                            return bookResults != null ? bookResults.ToList<BookVMTile>() : null;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerRecentlyVisitedItems", "GetRecentViewsByUser", "user:" + userId);
                
            }
            return null;
        }

        internal static bool AddRecentlyVisitedItem(string userId, int itemId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    RecentyViewItems recentyViewItems = ctx.RecentyViewItems.Where(x => x.UserId == userId).FirstOrDefault();
                    if(recentyViewItems !=null && !string.IsNullOrEmpty(recentyViewItems.RecentlyVisitedItems))
                    {
                        recentyViewItems.RecentlyVisitedItems = BusinessHandlerRecentlyVisitedItems.GetStringRecentViews(recentyViewItems.RecentlyVisitedItems, itemId);
                        recentyViewItems.UpdatedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        RecentyViewItems Obj = new RecentyViewItems
                        {
                            UserId = userId,
                            RecentlyVisitedItems = itemId.ToString(),
                            UpdatedDate = DateTime.UtcNow
                        };
                        ctx.RecentyViewItems.Add(Obj);                       
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }catch(Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerRecentlyVisitedItems", "AddRecentlyVisitedItem", "user:" + userId);

            }
            return false;
        }
    }
}