using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_book_shop.Handlers
{
    interface IPayment
    {
        bool CreatePaymentForCart(Cart cart);
    }
}
