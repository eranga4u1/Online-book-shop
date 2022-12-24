using iTextSharp.text.pdf.parser;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static iTextSharp.text.pdf.AcroFields;

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

        internal static PageResults GetBookStats(FilterByDate model, int page)
        {
            PageResults p = new PageResults();
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
                p.CurrentPage= page;
                p.NumberOfPages = (int) Math.Ceiling(Convert.ToDouble(result.Count/100));
                p.Results= result != null ? result.Skip(100 * (page - 1)).Take(100).ToList() : null;
                return p;
            }
            catch (Exception ex)
            {
                return p;
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

        internal static StockVM GetStockVM()
        {
            StockVM vm = new StockVM();
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var books = from a in ctx.Books
                                join b in ctx.BookProperties on a.Id equals b.BookId where !a.isDeleted && a.ItemType==(int)ItemType.Book
                                select new BookCountVM { BookId = a.Id, Count = b.NumberOfCopies };

                   var filteredBooks= books.GroupBy(d => d.BookId)
                                            .Select(
                                            g => new BookCountVM
                                            {
                                             BookId = g.Key,
                                             Count = g.Sum(s => s.Count)
                                            });
                if(filteredBooks != null)
                    {
                        vm.TotalItem= filteredBooks.Count();
                        vm.Available = filteredBooks.Where(x => x.Count > 0).Count();
                        vm.OutOfStock = filteredBooks.Where(x => x.Count <= 0).Count();
                        vm.RedLine = filteredBooks.Where(x => x.Count>0 && x.Count<5).Count();
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return vm;
        }
        internal static List<BookCountVM> GetBookStockDetails(int authorId,int publisherId,int stocktype, int page, int itemsperpage)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var books = from a in ctx.Books
                                join b in ctx.BookProperties on a.Id equals b.BookId
                                where !a.isDeleted && a.ItemType == (int)ItemType.Book
                                select new BookCountVM { 
                                    BookId = a.Id, 
                                    Count = b.NumberOfCopies,
                                    BookName=a.Title,
                                    BookPropertyName=b.Title,
                                    BookPropertyId=b.Id,
                                    AuthorId=a.AuthorId,
                                    PublisherId=a.PublisherId };
                    List<BookCountVM> filtered = new List<BookCountVM>();
                    if(authorId>0 && publisherId>0)
                    {
                        filtered = books.Where(x => x.AuthorId == authorId && x.PublisherId == publisherId).ToList();
                       
                    }else if (authorId > 0)
                    {
                        filtered = books.Where(x => x.AuthorId == authorId).ToList();

                    }
                    else if (publisherId > 0)
                    {
                        filtered = books.Where(x => x.PublisherId == publisherId).ToList();

                    }
                    else
                    {
                        filtered = books.ToList();
                    }
                    if (filtered.Count > 0)
                    {
                        if (stocktype == 0)
                        {
                            return filtered.Where(x => x.Count >= 0).Skip((page-1)* itemsperpage).Take(itemsperpage).ToList();
                        }
                        else if (stocktype == 1)
                        {
                            return filtered.Where(x => x.Count > 0).Skip((page - 1) * itemsperpage).Take(itemsperpage).ToList();
                        }
                        else if (stocktype == 2)
                        {
                            return filtered.Where(x => x.Count > 0 && x.Count < 5).Skip((page - 1) * itemsperpage).Take(itemsperpage).ToList();
                        }
                        else if (stocktype == 3)
                        {
                            return filtered.Where(x => x.Count < 1).Skip((page - 1) * itemsperpage).Take(itemsperpage).ToList();
                        }
                    }
                    return filtered.OrderBy(x=> x.BookName).ToList();
                }
            }
            catch(Exception ex)
            {

            }
            return null;
        }

        internal static bool UpdateStock(List<Book_Property_Amount> arr)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    foreach (var item in arr)
                    {
                        var s = ctx.BookProperties.Where(x => x.Id == item.BookPropertyId && x.BookId == item.BookId).FirstOrDefault();
                        if(s != null)
                        {
                            s.NumberOfCopies = s.NumberOfCopies + item.Amount;
                            BusinessHandlerStockEntry.Update(item.BookId, item.Amount, item.BookPropertyId, StockEntryOperation.In_Admin_Updated);
                        }
                    }
                    ctx.SaveChanges();
                    return true;
                }
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}