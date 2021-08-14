using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerBookProperties
    {
        public static BookProperties Add(BookProperties bookProperties)
        {
                try
                {
                    using (var ctx = new ApplicationDbContext())
                    {
                        ctx.BookProperties.Add(bookProperties);
                        if (ctx.SaveChanges() > 0)
                        {
                            BusinessHandlerStockEntry.Update(bookProperties.BookId, bookProperties.NumberOfCopies, bookProperties.Id, StockEntryOperation.In_Admin_Updated);
                        }
                }
                    return bookProperties;
                }
                catch (Exception ex)
                {
                    return null;
                }
        }
        public static List<BookProperties> GetBookPropertyByBookId(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                  return  ctx.BookProperties.Where(bp=> bp.BookId==id).OrderBy(x=>x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static BookProperties GetBookPropertyById(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.BookProperties.Where(bp => bp.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static BookProperties Put(BookProperties bookProperties)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   BookProperties dbbookProperties= ctx.BookProperties.Where(bp=> bp.Id== bookProperties.Id).FirstOrDefault();
                    int dbNumberOfCopies = dbbookProperties.NumberOfCopies;
                    if (dbbookProperties != null)
                    {
                        dbbookProperties.NumberOfCopies = bookProperties.NumberOfCopies;
                        dbbookProperties.NumberOfPages = bookProperties.NumberOfPages;
                        dbbookProperties.UpdatedBy = bookProperties.UpdatedBy;
                        dbbookProperties.UpdatedDate = bookProperties.UpdatedDate;
                        dbbookProperties.LanguageId = bookProperties.LanguageId;
                        dbbookProperties.BackCoverMediaId = bookProperties.BackCoverMediaId;
                        dbbookProperties.BookId = bookProperties.BookId;
                        dbbookProperties.Price = bookProperties.Price;
                        dbbookProperties.WeightByGrams = bookProperties.WeightByGrams;
                        dbbookProperties.FreeReadPDFMediaId = bookProperties.FreeReadPDFMediaId;
                        dbbookProperties.FrontCoverMediaId = bookProperties.FrontCoverMediaId;
                        dbbookProperties.Title = bookProperties.Title;
                        dbbookProperties.Description = bookProperties.Description;
                    }
                    if (ctx.SaveChanges()>0) {
                        if (bookProperties.NumberOfCopies - dbNumberOfCopies < 0)
                        {
                            BusinessHandlerStockEntry.Update(dbbookProperties.BookId, (bookProperties.NumberOfCopies - dbNumberOfCopies) * -1, bookProperties.Id, StockEntryOperation.Out_Admin_Updated);
                        }
                        else
                        {
                            BusinessHandlerStockEntry.Update(dbbookProperties.BookId,  (bookProperties.NumberOfCopies - dbNumberOfCopies), bookProperties.Id, StockEntryOperation.In_Admin_Updated);
                        }
                    }
                }
                return bookProperties;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static bool UpdateNumberOfBooks(int bookId, int propertyId,int count)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var property=ctx.BookProperties.Where(bp => bp.BookId == bookId && bp.Id==propertyId).FirstOrDefault();
                    if(property != null)
                    {
                        property.NumberOfCopies = (property.NumberOfCopies + count);//Count may be + or -
                        property.UpdatedDate = DateTime.UtcNow;
                        ctx.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}