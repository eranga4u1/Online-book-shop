using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerStock
    {
        public static StockVM GetStockVM()
        {
            return DBHandlerStockEntry.GetStockVM();
        }
        public static List<BookCountVM> GetBookStockDetails(int authorId, int publisherId, int stocktype, int page, int itemsperpage)
        {
            return DBHandlerStockEntry.GetBookStockDetails(authorId, publisherId, stocktype, page, itemsperpage);
        }
    }
}