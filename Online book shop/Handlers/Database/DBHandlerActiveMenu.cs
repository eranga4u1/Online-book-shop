using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Models;
namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerActiveMenu
    {
          public static int GetActiveMenuByController(string controller,string method)
            {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {

                    var Options = ctx.ActiveMenus.Where(x => x.ControllerName.Trim().ToLower() == controller);
                    if(Options !=null && Options.Count()>0 && string.IsNullOrEmpty(method))
                    {
                        return Options.FirstOrDefault().ActiveMenuOrderNo;
                    }
                    else if(Options != null && Options.Count() > 0 && !string.IsNullOrEmpty(method))
                    {
                      var Option= Options.Where(y => y.MethodName == method.ToLower()).FirstOrDefault();
                        if(Option != null)
                        {
                            return Option.ActiveMenuOrderNo;
                        }
                        else
                        {
                          return  Options.FirstOrDefault().ActiveMenuOrderNo;
                        }
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}