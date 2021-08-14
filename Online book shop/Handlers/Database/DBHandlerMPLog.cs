using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerMPLog
    {
        internal static MPLog Log(MPLog mPLog)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.MPLogs.Add(mPLog);
                    if (ctx.SaveChanges() > 0)
                    {
                        return mPLog;
                    }
                }
            }
            catch(Exception ex)
            {
               
            }
            return null;
        }
    }
}