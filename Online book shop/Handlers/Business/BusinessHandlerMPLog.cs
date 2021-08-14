using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerMPLog
    {
        public static MPLog Log(LogType type,string message,string controller, string action, string para=null)
        {
            MPLog mPLog = new MPLog
            {
                Type = (int)type,
                Message = message,
                Controller = controller,
                Action = action,
                Para = para,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = BusinessHandlerAuthor.GetLoginUserId()
            };
            return DBHandlerMPLog.Log(mPLog);
        }
    }
}