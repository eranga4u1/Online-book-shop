using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerBookCategory
    {
        internal static Book_Category Add(Book_Category category)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Obj = ctx.Book_Categories.Where(x => x.BookId == category.BookId && x.CategoryId == category.CategoryId).FirstOrDefault();
                    if(Obj != null)
                    {
                        Obj.isDeleted = false;
                    }
                    else
                    {
                        ctx.Book_Categories.Add(category);                        
                    }
                    ctx.SaveChanges();

                }
                return category;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static bool RemoveExsistngMap(int bookId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var Obj = ctx.Book_Categories.Where(x => x.BookId == bookId);
                if(Obj !=null && (Obj.Count() > 0))
                {
                    foreach(var item in Obj)
                    {
                        item.isDeleted = true;
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        internal static List<Book_Category> GetByBookId(int bookId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var list = ctx.Book_Categories.Where(x => x.BookId == bookId && !x.isDeleted);
                    return list!=null?list.ToList():null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static bool DeletebyBookId(int bookId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var list = ctx.Book_Categories.Where(x => x.BookId == bookId);
                    if (list !=null)
                    {
                        ctx.Book_Categories.RemoveRange(list);
                        if (ctx.SaveChanges() > 0) {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}