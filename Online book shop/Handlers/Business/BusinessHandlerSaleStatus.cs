using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerSaleStatus
    {
        public static List<SaleStatus> GetAllActiveSaleStatus()
        {
            return DBHandlerSaleStatus.GetAllActiveSaleStatus();
        }
        public static SaleStatus GetSaleStatus(int Id)
        {
            return DBHandlerSaleStatus.GetSaleStatus(Id);

        }
        public static SaleStatus GetSaleStatusByTitle(string Title)
        {
            return DBHandlerSaleStatus.GetSaleStatusByTitle(Title);
        }

        internal static bool Add(SaleStatus model)
        {
            return DBHandlerSaleStatus.Add(model);
        }

        internal static bool Update(SaleStatus model)
        {
            return DBHandlerSaleStatus.Update(model);
        }

        internal static bool IsAssigned(int id)
        {
            return DBHandlerSaleStatus.IsAssigned(id);
        }

        internal static bool Delete(int id)
        {
            return DBHandlerSaleStatus.Delete(id);
        }

        internal static SaleStatus GetBookSaleStatusOnGivenDate(int bookId, DateTime date)
        {
            return null;//DBHandlerSaleStatus.GetBookSaleStatusOnGivenDate(bookId, date);
        }
    }
}