using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerUtility
    {
        public static double GetTimeOffset()
        {
            double offset = 0;
            string val = System.Configuration.ConfigurationManager.AppSettings["TimeOffSet"];
            if(val != null)
            {
                offset = Convert.ToDouble(val);
            }
            return offset;
        }
    }
}