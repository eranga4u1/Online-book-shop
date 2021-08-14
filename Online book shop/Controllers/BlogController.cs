using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index()
        {
            int page = 0;
            int itemPerPage = 12;
            if (Request.QueryString["page"] != null)
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }
            List<Article> blogs = BusinessHandlerArticle.GetArticles(true);
            if (blogs !=null)
            {
              ViewBag.blogs=  blogs.Skip(page * itemPerPage).Take(itemPerPage).ToList();
                
            }
            else
            {
                ViewBag.blogs = null;
            }
            ViewBag.TotalNumberOfItems = blogs != null ? blogs.Count : 0;
            return View();
        }
        public ActionResult Blog(int id)
        {
            ViewBag.page = BusinessHandlerArticle.GetArticlesById(id);
            return View();
        }
    }
}