using Online_book_shop.Handlers.Database;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerCharts
    {
        public static Charts GetChartsData()
        {
            return DBHandlerCharts.GetCharts();
        }
    }
}