using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Search;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerCategory
    {
        public static Category Add(Category category)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Categories.Add(category);
                   if(ctx.SaveChanges() > 0 && BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                    {
                        SearchHandler.UpdateSearchIndex(category, ObjectTypes.Category);
                    }
                }
                return category;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<Category> GetCategories()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Categories = ctx.Categories.OrderBy(category => category.CategoryName).ToList();
                   
                    return Categories;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<Category> GetCategoryByBookId(int bookId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var list=from a in ctx.Book_Categories
                            where a.BookId == bookId
                            join
                                b in ctx.Categories on a.CategoryId equals b.Id
                            select b;
                   if( list!=null)
                        return list.ToList();
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Category UpdateCategory(Category category)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                  var category_db =  ctx.Categories.Where(c => c.Id == category.Id).FirstOrDefault();
                    if(category_db != null)
                    {
                        category_db.CategoryName = category.CategoryName;
                        category_db.CategoryDescription = category.CategoryDescription;
                        category_db.UpdatedDate = DateTime.Today;
                        category_db.UpdatedBy= BusinessHandlerAuthor.GetLoginUserId();
                        category_db.LocalCategoryName = category.LocalCategoryName;
                    }
                    ctx.SaveChanges();
                    if (BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                    {
                        SearchHandler.UpdateSearchIndex(category_db, ObjectTypes.Category);
                    }
                }
                return category;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Category GetCategory(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   return ctx.Categories.Where(c=> c.Id==id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static bool ShowHide(int id, bool option)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Category category = ctx.Categories.Where(x => x.Id == id).FirstOrDefault();
                    if (category != null)
                    {
                        category.isDeleted = option;
                        category.UpdatedDate = DateTime.UtcNow;
                        category.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        if (ctx.SaveChanges() > 0)
                        {
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