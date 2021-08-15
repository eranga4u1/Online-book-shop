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
    public class AuthorProfileController : Controller
    {

        // GET: Author
        public ActionResult Index(string id)
        {
            int page = 1;
            if(Request.QueryString["page"] != null)
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }
            int authorId;
            Author author = new Author();
            bool isNumeric = int.TryParse(id, out authorId);
            if (!isNumeric)
            {
                authorId = BusinessHandlerAuthor.GetAuthorIdByFriendlyName(id);
                author = BusinessHandlerAuthor.GetAuthorById(authorId);
            }
            else
            {
                author = BusinessHandlerAuthor.GetAuthorById(authorId);
                if(author != null)
                {
                    if (page == 1)
                    {
                        return Redirect(string.Format("/authors/{0}", author.FriendlyName));
                    }
                    else
                    {
                        return Redirect(string.Format("/authors/{0}?page={1}", author.FriendlyName,page));
                    }
                   
                }
                
            }

           // BookVMTile book = BusinessHandlerBook.GetSearchedBookForView(bookId);
            
            ViewBag.Author = author;
            List<BookVMTile> books= BusinessHandlerBook.GetBooksByAuthor(authorId, page);
            //List<BookVMTile> multi_author_books = BusinessHandlerBook.GetMultiAuthorBooks(id);
            //if(multi_author_books != null && books !=null)
            //{
            //    var books2= books.Union(multi_author_books).ToList();
            //    ViewBag.TotalNumberOfBooks = books2 != null ? books2.Count : 0;
            //    ViewBag.Books = books2.Skip(12 * page).Take(12).ToList();
            //}
            //else
            //{
                ViewBag.TotalNumberOfBooks = books != null ? books.Count : 0;
                ViewBag.Books = books.Skip(52 * (page-1)).Take(52).ToList();
            //}
           
            ViewBag.ActiveClass = "Author";
            try
            {
                ViewBag.MetaValues = new MetaData
                {
                    Description = Regex.Replace(author.Description, "<.*?>", String.Empty),
                    Title = string.Format("{0} {1}", !string.IsNullOrEmpty(author.LocalName) ? author.LocalName : "", !string.IsNullOrEmpty(author.Name) ? author.Name : ""),
                    Keywords = author.LocalName + "," + author.Name ,//+ "," + book.AuthorName + "," + book.LocalAuthorName,
                    Image = "https://musespublishers.com/Content/UploadFiles/Images/ProfilePicture/" + author.ProfileImage.FileName,
                    Type = "article"
                };
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "Index", "Book");
            }
            return View();
        }
        public ActionResult Collection()
        {
            int page = 1;
            int itemPerPage = 12;
            if (Request.QueryString["page"] != null)
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }
            List<Author> authors = BusinessHandlerAuthor.GetAuthors();
            ViewBag.TotalAuthors = authors.Count;
            if (authors != null)
            {
                ViewBag.authors = authors.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();

            }
            else
            {
                ViewBag.authors = null;
            }
            ViewBag.TotalNumberOfItems = authors != null ? authors.Count : 0;
            ViewBag.ActiveClass = "Author";
            return View();
        }
    }
}