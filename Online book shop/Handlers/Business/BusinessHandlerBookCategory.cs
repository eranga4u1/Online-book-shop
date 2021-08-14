using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerBookCategory
    {
        public static Book_Category SaveCategory(Book_Category category)
        {
            category.CreatedDate = DateTime.UtcNow;
            category.isDeleted = false;
            category.UpdatedDate = DateTime.UtcNow;
            category.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            category.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerBookCategory.Add(category);
        }

        internal static List<Book_Category> GetByBookId(int bookId)
        {
          return  DBHandlerBookCategory.GetByBookId(bookId);
        }

        internal static bool  DeletebyBookId(int bookId)
        {
            return DBHandlerBookCategory.DeletebyBookId(bookId);
        }
        internal static bool RemoveExsistngMap(int bookId)
        {
            return DBHandlerBookCategory.RemoveExsistngMap(bookId);
        }
    }
}