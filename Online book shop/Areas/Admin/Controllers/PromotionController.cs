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
        public ActionResult BulkPromotion(BulkPromotionVM model=null)
        {
            var books = BusinessHandlerBook.GetAllBooksWithPropertyAsNewOne();
            if (model == null || (model.PublisherId==0 && model.AuthorId==0))
            {
                ViewBag.Items = books;
            }
            else
            {
                if(model.AuthorId>0 && model.PublisherId > 0)
                {
                    ViewBag.Items = books.Where(x => x.BookAuthorId == model.AuthorId && x.BookPublisherId == model.PublisherId) !=null? books.Where(x => x.BookAuthorId == model.AuthorId && x.BookPublisherId == model.PublisherId).ToList():null;
                }
                else if(model.AuthorId > 0)
                {
                    ViewBag.Items = books.Where(x => x.BookAuthorId == model.AuthorId) !=null? books.Where(x => x.BookAuthorId == model.AuthorId).ToList():null;
                }else if (model.PublisherId > 0)
                {
                    ViewBag.Items = books.Where(x => x.BookPublisherId == model.PublisherId) !=null? books.Where(x => x.BookPublisherId == model.PublisherId).ToList():null;
                }
            }
            
            
            ViewBag.Authors = BusinessHandlerAuthor.GetAuthors();
            ViewBag.Publishers = BusinessHandlerPublisher.GetPublishers();
            ViewBag.FilterModel = model;
            return View();
        }

        public JsonResult AddBulkPromotion(VMBulkPromotion model)
        {
            if (!string.IsNullOrEmpty(model.SelectedItems))
            {
                model.StartDate = model.StartDate.AddHours(-12.5);
                model.EndDate = model.EndDate.AddHours(-12.5);
                List<Promotion> promoList = new List<Promotion>();
                string[] itemArray = model.SelectedItems.Split(',');
                foreach(string s in itemArray)
                {
                   string[] p = s.Split('/');
                    Promotion promo = new Promotion();
                    promo.ObjectId = Convert.ToInt32(p[0]);
                    promo.ObjectType = 0;
                    promo.OtherParameters ="{BookPropertyId:"+ p[1] + "}";
                    promo.PromotionTitle = "Applied Bulk Promotion";
                    promo.PromotionDescription = "Applied Bulk Promotion";
                    promo.PromotionTypesFor = 0;
                    promo.PromotionMethods = 1;
                    promo.DiscountValue = model.Percentage;
                    promo.StartDate = model.StartDate;
                    promo.EndDate = model.EndDate;
                    promo.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
                    promo.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                    promo.CreatedDate = DateTime.Today;
                    promo.UpdatedDate = DateTime.Today;
                    promoList.Add(promo);
                }
               if( BusinessHandlerPromotion.AddNewPromotions(promoList, 0))
                {
                    return new JsonResult { Data = "success" };
                }
            }
            return new JsonResult { Data = "fail" };
        }
    }
}