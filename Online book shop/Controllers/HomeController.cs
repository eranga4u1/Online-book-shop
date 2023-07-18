using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Email;
using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using reCAPTCHA.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Online_book_shop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           List<BookVMTile> preorder_books = BusinessHandlerBook.GetPreOrderBooksForView();
           List<BookVMTile> latest_books = BusinessHandlerBook.GetLatestBooksForView();
            List<BookVMTile> book_packs = BusinessHandlerBook.GetBookPacksForView();
            List<BookVMTile> best_selling_books = BusinessHandlerBook.GetBestSellingBooksForView(); //set to get latest book this should change later
            // var preOrderBooks = books.Where(x => x.SaleType == (int)SaleType.PreOrder);
            ViewBag.PreOrderBooks = preorder_books.Where(b=> !b.isDeleted).OrderByDescending(x=> x.CreatedDate).ToList();
            ViewBag.LatestBooks = latest_books.Where(b => !b.isDeleted).OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.BestSellingBooks = best_selling_books.Where(b => !b.isDeleted).OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.HomeBanners = BusinessHandlerMedia.GetHomeBanners();
            List<Article> RecentArticles = BusinessHandlerArticle.GetArticles(true);
            ViewBag.Articles = (RecentArticles != null && RecentArticles.Count > 0) ? RecentArticles.Take(3).ToList() : null;
            ViewBag.LatestPromotion = BusinessHandlerPromotion.GetLatestPromotion();
            ViewBag.RecentlyViewItems = BusinessHandlerRecentlyVisitedItems.Get();
            ViewBag.book_packs = book_packs;
            //HTMLHelper.Test();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //public ActionResult Contact()
        //{
        //    Configuration config = BusinessHandlerConfigurations.GetConfigByKey("CONTACTUS-PAGE");
        //    if (config != null)
        //    {
        //        ViewBag.contactVM = JsonConvert.DeserializeObject<ContactVM>(config.Value);
        //    }
        //    else
        //    {
        //        ContactVM contactVM = new ContactVM
        //        {
        //            CompanyName= "Muses Publishing House - Pvt Ltd",
        //            AddressLine1 = "34/4, katuwalamulla",
        //            AddressLine2 = "11020 Ganemulla,",
        //            AddressLine3 = "Sri Lanka",
        //            Email1 = "support@musesbooks.com",
        //            Email2 = "mareting@musesbooks.com",
        //            Email3 = "publisher@musesbooks.com",
        //            Phone1 = "077 824 5459",
        //            Phone2 = "",
        //            Phone3 = "",
        //            Description = "საბეჭდი და ტიპოგრაფიული ინდუსტრიის უშინაარსო ტექსტია. იგი სტანდარტად 1500-იანი წლებიდან იქცა, როდესაც უცნობმა მბეჭდავმა ამწყობ დაზგაზე წიგნის საცდელი ეგზემპლარი დაბეჭდა. მისი ტექსტი არამარტო 5 საუკუნის მანძილზე შემორჩა, არამედ მან დღემდე, ელექტრონული ტიპოგრაფიის დრომდეც უცვლელად მოაღწია. განსაკუთრებული პოპულარობა მას 1960-იან წლებში გამოსულმა Letraset-ის ცნობილმა ტრაფარეტებმა მოუტანა, უფრო მოგვიანებით კი — Aldus PageMaker-ის ტიპის საგამომცემლო პროგრამებმა, რომლებშიც Lorem Ipsum-ის სხვადასხვა ვერსიები იყო ჩაშენებული.",
        //        };
        //        BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "CONTACTUS-PAGE", Value =JsonConvert.SerializeObject(contactVM) });

        //        ViewBag.contactVM = contactVM;
        //    }
        //    return View();
        //}

        [HttpPost]
        [CaptchaValidator]
        public ActionResult SendContactUsEmail(ContactUsEmail model, bool captchaValid)
        {
            var blockedEmailAddress = System.Configuration.ConfigurationManager.AppSettings["EmailBlockList"];
            if (!string.IsNullOrEmpty(blockedEmailAddress)) {
            var addresses= blockedEmailAddress.Split(',');
                if (addresses.Contains(model.Email))
                {
                    return Redirect("/");
                }
            }
            if (ModelState.IsValid)
            {
                string[] para = { model.FirstName + " " + model.LastName, model.Email, model.contactnumber, model.message };
                EmailHandler.Email(EmailHandler.SetEmailParameter("ContactUs.html", para), "contact@musesbooks.com", "webmaster@musespublishers.com", "Contact us Email from " + model.FirstName + " " + model.LastName,"MusesBooks.com : Contact Us Email");
                return Redirect("/");
            }
            else
            {
                return Redirect("/contact");
            }
            
        }
    }
}