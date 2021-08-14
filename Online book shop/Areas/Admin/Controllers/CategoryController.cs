using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
          ViewBag.Categories=BusinessHandlerCategory.GetCategories();
            return View();
        }
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            if(category != null)
            {
                BusinessHandlerCategory.SaveCategory(category);
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditCategory(int id)
        {
          ViewBag.Category=  BusinessHandlerCategory.GetCategory(id);
            return View();
        }
        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            BusinessHandlerCategory.UpdateCategory(category);
            return RedirectToAction("Index");
        }
        public ActionResult HideItem(int id)
        {
            BusinessHandlerCategory.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerCategory.Show(id);
            return RedirectToAction("Index");
        }
    }
}