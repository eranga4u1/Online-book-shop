using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerCharts
    {
        internal static Charts GetCharts()
        {
            try
            {
                Charts charts = new Charts();
                //using (var ctx = new ApplicationDbContext())
                //{
                //    charts.TotalActiveUsers = ctx.Users.Count();
                //    charts.TotalShippedOrders =ctx.Orders.Where(x=> x.DeliveryStatus==)
                //}
                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
//select count(*) from AspNetUsers as TotalActiveUsers where TotalActiveUsers.isDeleted = 0;
//select count(*) from Orders as TotalShippedOrders where TotalShippedOrders.DeliveryStatus > 1;

//select sum(Carts.AmountAfterDiscount) from Orders as TotalEarnings
//                                      join Carts on TotalEarnings.CartID = Carts.Id
//where TotalEarnings.DeliveryStatus > 1;


//select* from DeliverStatus