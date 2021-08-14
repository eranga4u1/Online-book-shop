using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerBookProperties
    {
        public static BookProperties Add(BookProperties bookProperties)
        {
            bookProperties.CreatedDate = DateTime.UtcNow;
            bookProperties.UpdatedDate = DateTime.UtcNow;
            bookProperties.isDeleted = false;
            bookProperties.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            bookProperties.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerBookProperties.Add(bookProperties);
        }

        public static BookProperties AddDemoProperty(int bookId)
        {
            BookProperties Obj = new BookProperties
            {
                BookId = bookId,
                NumberOfPages = 0,
                NumberOfCopies = 0,
                LanguageId = 0,
                Price = 0,
                WeightByGrams = 0,
                FreeReadPDFMediaId = 0,
                BackCoverMediaId = 0,
                FrontCoverMediaId = 0,
                Title = "Sample Description",
                Description = "Update this fields with actual data"
            };
            return BusinessHandlerBookProperties.Add(Obj);
        }
        public static List<BookProperties> GetByBookId(int id)
        {
            return DBHandlerBookProperties.GetBookPropertyByBookId(id);
        }
        public static BookProperties GetById(int id)
        {
            return DBHandlerBookProperties.GetBookPropertyById(id);
        }
        public static BookProperties Put(BookProperties bookProperties)
        {
            return DBHandlerBookProperties.Put(bookProperties);
        }
        public static bool UpdateNumberOfBooks(int bookId, int propertyId, int count)
        {
            return DBHandlerBookProperties.UpdateNumberOfBooks(bookId, propertyId, count);

        }
    }
}