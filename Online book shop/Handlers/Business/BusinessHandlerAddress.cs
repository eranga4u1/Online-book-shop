using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerAddress
    {
        public static Address Add(Address model)
        {
            model.CreatedDate = DateTime.UtcNow;
            model.UpdatedDate = DateTime.UtcNow;
            model.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            model.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            model.isDeleted = false;
            model.UserId= BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerAddress.Add(model);
        }
        public static List<Address> GetAddresses()
        {
            return DBHandlerAddress.GetAddresses(BusinessHandlerAuthor.GetLoginUserId());
        }
        public static Address GetAddress(int id)
        {
            return DBHandlerAddress.GetAddress(id);
        }

        internal static bool remove(int model)
        {
            return DBHandlerAddress.RemoveAddress(model);
        }
        internal static bool setDefault(int model)
        {
            return DBHandlerAddress.SetDefault(model,BusinessHandlerAuthor.GetLoginUserId());
        }

        internal static Address Edit(Address model)
        {
            model.UpdatedDate = DateTime.UtcNow;           
            model.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            model.isDeleted = false;
            model.UserId = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerAddress.Update(model);
        }
    }
}