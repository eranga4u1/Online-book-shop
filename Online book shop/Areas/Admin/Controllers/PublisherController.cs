using Newtonsoft.Json;
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
    public class PublisherController : Controller
    {
        // GET: Admin/Publisher
        public ActionResult Index()
        {
            ViewBag.Publishers =BusinessHandlerPublisher.GetPublishers();
            return View();
        }
        public ActionResult CreatePublisher()
        {
            return View();
        }
        public ActionResult CreatePublisherProfile(Publisher publisher)
        {
            var image = Request.Files["ImageFile"];
            var coverImg = Request.Files["ImageFile_Cover"];
            if (image != null && image.ContentLength>0)
            {
                Media profilePic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (profilePic != null)
                {
                    publisher.ProfilePictureMediaId = profilePic.Id;
                }
            }
            if (coverImg != null && coverImg.ContentLength > 0)
            {
                Media coverPic = BusinessHandlerMedia.CreateNewMediaEntry(coverImg, MediaCategory.CoverImage);
                if (coverPic != null)
                {
                    publisher.CoverPictureMediaId = coverPic.Id;
                }
            }
            publisher.CreatedDate = DateTime.Now;
            publisher.isDeleted = false;
            publisher.UpdatedDate = DateTime.Now;
            publisher.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            publisher.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            BusinessHandlerPublisher.SaveNewPublisher(publisher);
            return RedirectToAction("Index");
        }
        public ActionResult EditPublisher(int id)
        {
            ViewBag.Publisher = BusinessHandlerPublisher.GetPublisherById(id);
            return View();
        }
        public ActionResult UpdatePublisherProfile(Publisher publisher)
        {
            var image = Request.Files["ImageFile"];
            var coverImg = Request.Files["ImageFile_Cover"];
            if (image.ContentLength > 0)
            {
                Media profilePic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (profilePic != null)
                {
                    publisher.ProfilePictureMediaId = profilePic.Id;
                }
                else
                {
                    publisher.ProfilePictureMediaId = 0;
                }
            }
            if (coverImg != null && coverImg.ContentLength > 0)
            {
                Media coverPic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (coverPic != null)
                {
                    publisher.CoverPictureMediaId = coverPic.Id;
                }
                else
                {
                    publisher.ProfilePictureMediaId = 0;
                }
            }
            publisher.isDeleted = false;
            publisher.UpdatedDate = DateTime.Now;
            publisher.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            BusinessHandlerPublisher.UpdatePublisher(publisher);
            return RedirectToAction("Index");
        }
        public string GetPublishers()
        {
            return JsonConvert.SerializeObject(BusinessHandlerPublisher.GetPublishers().Select(p=>new Publisher { Id= p.Id, Name=p.Name}));
        }
        public ActionResult HideItem(int id)
        {
            BusinessHandlerPublisher.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerPublisher.Show(id);
            return RedirectToAction("Index");
        }
    }
}