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
    public class ContentController : Controller
    {
        // GET: Admin/Content
        public ActionResult Index()
        {
            ViewBag.AllContent= BusinessHandlerContent.GetAll();
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Content model)
        {
            if(model != null)
            {
                BusinessHandlerContent.PostContent(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ContentPage = BusinessHandlerContent.GetById(id);
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Content model)
        {
            BusinessHandlerContent.Put(model);
            return RedirectToAction("Index");
        }

        public ActionResult HideItem(int id)
        {
            BusinessHandlerContent.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerContent.Show(id);
            return RedirectToAction("Index");
        }

    }
}