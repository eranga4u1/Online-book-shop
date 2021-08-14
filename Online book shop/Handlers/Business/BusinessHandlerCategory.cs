using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerCategory
    {
        public static Category SaveCategory(Category category)
        {
            category.CreatedDate = DateTime.UtcNow;
            category.isDeleted = false;
            category.UpdatedDate = DateTime.UtcNow;
            category.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            category.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerCategory.Add(category);
        }
        public static List<Category> GetCategories()
        {
            return DBHandlerCategory.GetCategories();
        }

        internal static Category GetCategory(int id)
        {
            return DBHandlerCategory.GetCategory(id);
        }

        internal static Category UpdateCategory(Category category)
        {
            return DBHandlerCategory.UpdateCategory(category); 
        }
        public static List<Category> GetCategoryByBookId(int bookId)
        {
            return DBHandlerCategory.GetCategoryByBookId(bookId);
        }
        internal static bool Show(int id)
        {
            return DBHandlerCategory.ShowHide(id, false);
        }
        internal static bool Hide(int id)
        {
            return DBHandlerCategory.ShowHide(id, true);
        }
    }
}