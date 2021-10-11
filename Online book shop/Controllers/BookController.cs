using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class BookProfileController : Controller
    {
        // GET: Book
        public ActionResult Index(string id)
        {
            int bookId;
            bool isNumeric = int.TryParse(id, out bookId);
            if (!isNumeric)
            {
                bookId = BusinessHandlerBook.GetIdFromFriendlyName(id);
            }
           
            BookVMTile book= BusinessHandlerBook.GetSearchedBookForView(bookId);
            if(book !=null && (book.Property==null || book.Property.Count < 1)){
                if(book.Property == null)
                {
                    book.Property = new List<BookProperties>();
                }
                book.Property.Add(BusinessHandlerBookProperties.AddDemoProperty(book.Id));
            }
            
            List<ReviewVM> reviews = BusinessHandlerUserReviews.GetReviewsForBook(bookId);
            int RecentRate = BusinessHandlerUserReviews.GetRecentReviewForBook(bookId);
            List<BookVMTile> Authorsbooks = BusinessHandlerBook.GetBooksByAuthor(book.AuthorId);
            List<BookVMTile> Suggestionbooks = book.Categories !=null?BusinessHandlerBook.GetBooksByCategories(book.Categories.Select(x=> x.Id).ToList()):null;

            ViewBag.book = book;
            ViewBag.Authorsbooks = Authorsbooks.Where(b => b.Id != bookId) != null? Authorsbooks.Where(b => b.Id != bookId).ToList():null;
            ViewBag.Suggestionbooks = Suggestionbooks.Where(b => b.Id != bookId) != null ? Suggestionbooks.Where(b => b.Id != bookId).ToList() : null; 
            ViewBag.Reviews = reviews;
            ViewBag.RecentRate = RecentRate;
            if (book.publisher !=null && book.publisher.Id == 9)
            {
                ViewBag.MethodName = "muses";
            }
            try
            {
                ViewBag.MetaValues = new MetaData
                {
                    Description = Regex.Replace(book.Description, "<.*?>", String.Empty),
                    Title = string.Format("{0} {1}", !string.IsNullOrEmpty(book.LocalBookName) ? book.LocalBookName : "", !string.IsNullOrEmpty(book.BookName) ? book.BookName : ""),
                    Keywords = (!string.IsNullOrEmpty(book.LocalBookName) ? book.LocalBookName : "")+ "," + (!string.IsNullOrEmpty(book.BookName) ? book.BookName : "") + "," + (!string.IsNullOrEmpty(book.AuthorName) ? book.AuthorName : "") + "," + (!string.IsNullOrEmpty(book.LocalAuthorName) ? book.LocalAuthorName : ""),
                    Image = "https://musespublishers.com/Content/UploadFiles/Images/BookFrontCover/" + (!string.IsNullOrEmpty(book.FrontCover.FileName) ? book.FrontCover.FileName : ""),
                    Type="product"
                };
                BusinessHandlerRecentlyVisitedItems.Add(bookId);
            }
            catch(Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "Index", "Book");
            }
           
            return View();
        }
        public ActionResult Collection()
        {
            int page = 0;
            if (Request.QueryString["page"] != null)
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }
            List<BookVMTile> books = BusinessHandlerBook.GetAllBooksForView();
            List<Category> categories = BusinessHandlerCategory.GetCategories();
            ViewBag.TotalNumberOfBooks = books != null ? books.Count : 0;
            ViewBag.Books = books;//.Skip(25 * page).Take(25).ToList();
            ViewBag.Categories = categories;
            return View();
        }

        public static Dictionary<int,int> GetStock(int bookId)
        {
            return BusinessHandlerBook.GetStock(bookId);
        }
        public ActionResult BookPacks()
        {
            ViewBag.AllActiveBookPacks= BusinessHandlerBook.GetAllActiveBookPacks();
            return View();
        }
    }
}