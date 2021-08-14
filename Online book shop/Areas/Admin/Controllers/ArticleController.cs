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
    public class ArticleController : Controller
    {
        // GET: Admin/Article
        public ActionResult Index()
        {
            ViewBag.Articles = BusinessHandlerArticle.GetArticles();
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult CreateArticle(Article article)
        {
            var image = Request.Files["MainPicture"];
            var coverImg = Request.Files["CoverPicture"];
            if (image != null && image.ContentLength > 0)
            {
                Media profilePic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (profilePic != null)
                {
                    article.MainPictureMediaId = profilePic.Id;
                }
            }
            if (coverImg != null && coverImg.ContentLength > 0)
            {
                Media coverPic = BusinessHandlerMedia.CreateNewMediaEntry(coverImg, MediaCategory.CoverImage);
                if (coverPic != null)
                {
                    article.CoverPictureMediaId = coverPic.Id;
                }
            }

            BusinessHandlerArticle.Add(article);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Article article= BusinessHandlerArticle.GetArticlesById(id);
            ViewBag.Article = article;
            return View();
        }
        [ValidateInput(false)]
        public ActionResult EditArticle(Article article)
        {
            var image = Request.Files["MainPicture"];
            var coverImg = Request.Files["CoverPicture"];
            if (image != null && image.ContentLength > 0)
            {
                Media profilePic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (profilePic != null)
                {
                    article.MainPictureMediaId = profilePic.Id;
                }
                else
                {
                    article.MainPictureMediaId = 0;
                }
            }
            if (coverImg != null && coverImg.ContentLength > 0)
            {
                Media coverPic = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.ProfilePicture);
                if (coverPic != null)
                {
                    article.CoverPictureMediaId = coverPic.Id;
                }
                else
                {
                    article.CoverPictureMediaId = 0;
                }
            }
            BusinessHandlerArticle.Update(article);
            return RedirectToAction("Index");
        }
        public ActionResult HideItem(int id)
        {
            BusinessHandlerArticle.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerArticle.Show(id);
            return RedirectToAction("Index");
        }
    }
}