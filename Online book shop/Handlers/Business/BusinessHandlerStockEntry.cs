using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerStockEntry
    {
        public static StockEntry Update(int bookId,int numberOfBooks,int bookPropertyId, StockEntryOperation operation)
        {
            StockEntry stockEntry = new StockEntry
            {
                BookId=bookId,
                BookPrpertyId=bookPropertyId,
                NumberOfBook=numberOfBooks,
                Operation=operation.ToString(),
                CreatedDate=DateTime.UtcNow,
                CreatedBy= BusinessHandlerAuthor.GetLoginUserId(),
            };
           return DBHandlerStockEntry.Update(stockEntry);
        }
        public static int GetUpdatedNumberOfbooks()
        {
            return 0;
        }

        internal static bool Update(List<Cart_Book> items,string Details)
        {
            List<StockEntry> list = new List<StockEntry>();
            foreach (Cart_Book b in items)
            {
                StockEntry stockEntry = new StockEntry
                {
                    BookId = b.BookId,
                    BookPrpertyId = b.BookPropertyId,
                    NumberOfBook = b.NumberOfItems,
                    Operation = StockEntryOperation.Out.ToString(),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = BusinessHandlerAuthor.GetLoginUserId(),
                };
                BusinessHandlerBookProperties.UpdateNumberOfBooks(b.BookId, b.BookPropertyId,-(b.NumberOfItems));
                list.Add(stockEntry);
            }
            return DBHandlerStockEntry.Update(list);
        }
    }
}