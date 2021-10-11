using Online_book_shop.Handlers.Business;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class BookPacksController : Controller
    {
        // GET: BookPack
        public ActionResult Index()
        {
            //int page = 1;
            List<BookVMTile> allActiveBookPacks= BusinessHandlerBook.GetAllActiveBookPacks();
            if(allActiveBookPacks !=null && allActiveBookPacks.Count > 0)
            {
                //if (Request.QueryString["page"] != null && Convert.ToInt32(Request.QueryString["page"]) > 1)
                //{
                //    page = Convert.ToInt32(Request.QueryString["page"]);
                //}
                ViewBag.AllActiveBookPacks = BusinessHandlerBook.GetAllActiveBookPacks();//.Skip((page-1) *52).Take(52);                
                ViewBag.Total = allActiveBookPacks.Count();
            }
            else
            {
                ViewBag.AllActiveBookPacks = new List<BookVMTile>();
                ViewBag.Total = 0;
            }
            
            return View();
        }
    }
}