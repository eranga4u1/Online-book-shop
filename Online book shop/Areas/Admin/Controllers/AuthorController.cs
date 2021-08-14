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
    public class AuthorController : Controller
    {
        // GET: Admin/Author
        public ActionResult Index()
        {
            ViewBag.Authors = BusinessHandlerAuthor.GetAuthors();
            return View();
        }
        public ActionResult CreateAuthor()
        {
            return View();
        }
        public ActionResult CreateAuthorProfile(Author author)
        {
            var image = Request.Files["ImageFile"];
            var coverImg = Request.Files["ImageFile_Cover"];
            if(image != null && image.ContentLength>0)
            {
               Media profilePic=  BusinessHandlerMedia.CreateNewMediaEntry(image,MediaCategory.ProfilePicture);
                if(profilePic != null)
                {
                    author.ProfilePictureMediaId = profilePic.Id;
                }
            }
            if (coverImg != null && coverImg.ContentLength > 0)
            {
                Media coverPic = BusinessHandlerMedia.CreateNewMediaEntry(coverImg, MediaCategory.CoverImage);
                if (coverPic != null)
                {
                    author.CoverPictureMediaId = coverPic.Id;
                }
            }

            author.CreatedDate = DateTime.Now;
            author.isDeleted = false;
            author.UpdatedDate = DateTime.Now;
            author.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            author.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            BusinessHandlerAuthor.SaveNewAuthor(author);
            return RedirectToAction("Index");
        }
        public ActionResult EditAuthor(int id)
        {
            ViewBag.Author = BusinessHandlerAuthor.GetAuthorById(id);
            return View();
        }
        public ActionResult UpdateAuthorProfile(Author author)
        {
            var image = Request.Files["ImageFile"];
            var coverImg = Request.Files["ImageFile_Cover"];
            if (image !=null && image.ContentLength>0)
            {
                Media profilePic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (profilePic != null)
                {
                    author.ProfilePictureMediaId = profilePic.Id;
                }
                else
                {
                    author.ProfilePictureMediaId = 0;
                }
            }
            if (coverImg != null && coverImg.ContentLength > 0)
            {
                Media coverPic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (coverPic != null)
                {
                    author.CoverPictureMediaId = coverPic.Id;
                }
                else
                {
                    author.ProfilePictureMediaId = 0;
                }
            }
            author.isDeleted = false;
            author.UpdatedDate = DateTime.Now;
            author.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            BusinessHandlerAuthor.UpdateAuthor(author);
            return RedirectToAction("Index");
        }
        public string GetAuthors()
        {
            return JsonConvert.SerializeObject(BusinessHandlerAuthor.GetAuthors().Select(x=> new Author {Id=x.Id,Name=x.Name}));
        }
        public ActionResult HideItem(int id)
        {
            BusinessHandlerAuthor.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerAuthor.Show(id);
            return RedirectToAction("Index");
        }
    }
}