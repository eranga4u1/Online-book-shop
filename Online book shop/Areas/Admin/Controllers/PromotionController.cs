using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PromotionController : Controller
    {
        // GET: Admin/Promotion
        public ActionResult Index()
        {
            ViewBag.Promotions = BusinessHandlerPromotion.Get();
            return View();
        }
        public ActionResult Create()
        {
            var books = BusinessHandlerBook.GetAllBooksWithPropertyAsNewOne(); //BusinessHandlerBook.GetAllBooks(true).Select(x=> new DataObjVM { Id=x.Id,Name=x.Title,ObjType=0}).ToList();
            var authors = BusinessHandlerAuthor.GetAuthors().Select(x => new DataObjVM { Id = x.Id, Name = x.Name, ObjType = 1 }).ToList();
            var publishers = BusinessHandlerPublisher.GetPublishers().Select(x => new DataObjVM { Id = x.Id, Name = x.Name, ObjType = 2 }).ToList();
            var categories = BusinessHandlerCategory.GetCategories().Select(x => new DataObjVM { Id = x.Id, Name = x.CategoryName, ObjType = 3 }).ToList();
            publishers.AddRange(categories);
            books.AddRange(authors);
            books.AddRange(publishers);
            ViewBag.list = books;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Promotion model)
        {
            var image = Request.Files["MainPicture"];
            if (image != null && image.ContentLength > 0)
            {
                Media profilePic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (profilePic != null)
                {
                    model.PromotionMediaId = profilePic.Id;
                }
            }
            BusinessHandlerPromotion.Add(model);
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var books = BusinessHandlerBook.GetAllBooks(true).Select(x => new DataObjVM { Id = x.Id, Name = x.Title, ObjType = 0 }).ToList();
            var authors = BusinessHandlerAuthor.GetAuthors().Select(x => new DataObjVM { Id = x.Id, Name = x.Name, ObjType = 1 }).ToList();
            var publishers = BusinessHandlerPublisher.GetPublishers().Select(x => new DataObjVM { Id = x.Id, Name = x.Name, ObjType = 2 }).ToList();
            var categories = BusinessHandlerCategory.GetCategories().Select(x => new DataObjVM { Id = x.Id, Name = x.CategoryName, ObjType = 3 }).ToList();
            publishers.AddRange(categories);
            books.AddRange(authors);
            books.AddRange(publishers);
            ViewBag.list = books;
            ViewBag.Promotion = BusinessHandlerPromotion.Get(id);
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Promotion model)
        {
            var image = Request.Files["MainPicture"];
            if (image != null && image.ContentLength > 0)
            {
                Media profilePic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (profilePic != null)
                {
                    model.PromotionMediaId = profilePic.Id;
                }
            }
            BusinessHandlerPromotion.Update(model);
            return RedirectToAction("Index");
        }
        public ActionResult HideItem(int id)
        {
            BusinessHandlerPromotion.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerPromotion.Show(id);
            return RedirectToAction("Index");
        }
        public ActionResult BulkPromotion()
        {
            var books = BusinessHandlerBook.GetAllBooksWithPropertyAsNewOne();
            ViewBag.Items = books;
            return View();
        }
    }
}