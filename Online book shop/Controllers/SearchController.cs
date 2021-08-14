using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Search;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static Online_book_shop.Handlers.Search.LuceneService;

namespace Online_book_shop.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        public bool ClearAndRecreateSearchIndex()
        {
           return SearchHandler.ClearAndRecreateSearchIndex();
        }
        public ActionResult Find(string search, int page = 1, int itemPerPage = 20)
        {
            List<SearchData> searchDatas = SearchHandler.GetSearchSuggestion(search);//SearchHandler.Search(search);
            if (searchDatas != null)
            {
                searchDatas = searchDatas.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
            }
            List<BookVMTile> searchedBooks = new List<BookVMTile>();
            List<Author> searchedAuthors = new List<Author>();
            List<Publisher> searchedPublishers = new List<Publisher>();
            List<Category> searchedCategories = new List<Category>();
            foreach (SearchData searchData in searchDatas)
            {
                if (searchData.ObjectType == (int)ObjectTypes.Book)
                {
                    BookVMTile foundBook = BusinessHandlerBook.GetBookTileByBookId(searchData.objectId);
                    if (foundBook != null)
                    {
                        searchedBooks.Add(foundBook);
                    }
                }
                else if (searchData.ObjectType == (int)ObjectTypes.Author)
                {
                    Author foundAuthor = BusinessHandlerAuthor.GetAuthorById(searchData.objectId);
                    if (foundAuthor != null)
                    {
                        searchedAuthors.Add(foundAuthor);
                    }
                }
                else if (searchData.ObjectType == (int)ObjectTypes.Publisher)
                {
                    Publisher foundPublisher = BusinessHandlerPublisher.GetPublisherById(searchData.objectId);
                    if (foundPublisher != null)
                    {
                        searchedPublishers.Add(foundPublisher);
                    }
                }
                else if (searchData.ObjectType == (int)ObjectTypes.Category)
                {
                    Category foundCategory = BusinessHandlerCategory.GetCategory(searchData.objectId);
                    if (foundCategory != null)
                    {
                        searchedCategories.Add(foundCategory);
                    }
                }

            }
            ViewBag.SearchedBooks = searchedBooks;//BusinessHandlerBook.GetBestSellingBooksForView();
            ViewBag.SearchedAuthors = searchedAuthors;//BusinessHandlerAuthor.GetAuthors(); //
            ViewBag.SearchedPublishers = searchedPublishers; //BusinessHandlerPublisher.GetPublishers();// 
            ViewBag.SearchedCategories = searchedCategories; //BusinessHandlerCategory.GetCategories(); 
            return View();
        }
        public ActionResult Search(string search,int page=1,int itemPerPage=20)
        {
           List<SearchData> searchDatas= SearchHandler.GetSearchSuggestion(search);//SearchHandler.Search(search);
            if (searchDatas != null)
            {
                searchDatas = searchDatas.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
            }
            List<BookVMTile> searchedBooks = new List<BookVMTile>();
            List<Author> searchedAuthors = new List<Author>();
            List<Publisher> searchedPublishers = new List<Publisher>();
            List<Category> searchedCategories = new List<Category>();
            foreach (SearchData searchData in searchDatas)
            {
                if (searchData.ObjectType== (int)ObjectTypes.Book)
                {
                    BookVMTile foundBook= BusinessHandlerBook.GetBookTileByBookId(searchData.objectId);
                   if(foundBook != null) {
                        searchedBooks.Add(foundBook);
                    }                      
                }
                else if (searchData.ObjectType == (int)ObjectTypes.Author)
                {
                    Author foundAuthor = BusinessHandlerAuthor.GetAuthorById(searchData.objectId);
                    if(foundAuthor != null)
                    {
                        searchedAuthors.Add(foundAuthor);
                    }
                }else if (searchData.ObjectType == (int)ObjectTypes.Publisher)
                {
                    Publisher foundPublisher = BusinessHandlerPublisher.GetPublisherById(searchData.objectId);
                    if (foundPublisher != null)
                    {
                        searchedPublishers.Add(foundPublisher);
                    }
                }
                else if (searchData.ObjectType == (int)ObjectTypes.Category)
                {
                    Category foundCategory = BusinessHandlerCategory.GetCategory(searchData.objectId);
                    if (foundCategory != null)
                    {
                        searchedCategories.Add(foundCategory);
                    }
                }

            }
            ViewBag.SearchedBooks = searchedBooks;//BusinessHandlerBook.GetBestSellingBooksForView();
            ViewBag.SearchedAuthors = searchedAuthors;//BusinessHandlerAuthor.GetAuthors(); //
            ViewBag.SearchedPublishers =searchedPublishers; //BusinessHandlerPublisher.GetPublishers();// 
            ViewBag.SearchedCategories =searchedCategories; //BusinessHandlerCategory.GetCategories(); 
            return View();
        }
        public string SearchByField(string search,string field)
        {
           var bb= SearchHandler.Search(search,field);
            return "done";
        }
        //[HttpPost]
        public string GetSearchSuggestion(string term)
        {
            try
            {
                List<SearchData> searchDatas = new List<SearchData>();
                //if (Regex.IsMatch(term, "^[a-zA-Z0-9]*$"))
                //{
                //    searchDatas = SearchHandler.GetSearchSuggestionBySP(term);
                //}
                //else
                //{
                searchDatas = SearchHandler.GetSearchSuggestion(term);

                if (searchDatas != null)
                {
                    searchDatas = searchDatas.Take(10).ToList();
                }
                //List<BookVMTile> searchedBooks = new List<BookVMTile>();
                //List<Author> searchedAuthors = new List<Author>();
                //List<Publisher> searchedPublishers = new List<Publisher>();
                //List<Category> searchedCategories = new List<Category>();
                foreach (SearchData searchData in searchDatas)
                {
                    if (searchData.ObjectType == (int)ObjectTypes.Book)
                    {
                        Book book = BusinessHandlerBook.Get(searchData.objectId);
                        if (book != null && !book.isDeleted)
                        {
                            searchData.URL = "/book/" + searchData.objectId;
                        }
                        else
                        {
                            searchData.URL = null;
                        }
                    }
                    else if (searchData.ObjectType == (int)ObjectTypes.Author)
                    {
                        Author author = BusinessHandlerAuthor.GetAuthorById(searchData.objectId);
                        if (author != null && !author.isDeleted)
                        {
                            searchData.URL = "/authors/" + searchData.objectId;
                        }
                        else
                        {
                            searchData.URL = null;
                        }

                    }
                    //else if (searchData.ObjectType == (int)ObjectTypes.Publisher)
                    //{
                    //    Publisher pub = BusinessHandlerPublisher.GetPublisherById(searchData.objectId);
                    //    if (pub != null && !pub.isDeleted)
                    //    {
                    //        searchData.URL = "/publisher/" + searchData.objectId;
                    //    }
                    //    else
                    //    {
                    //        searchData.URL = null;
                    //    }

                    //}
                    //else if (searchData.ObjectType == (int)ObjectTypes.Category)
                    //{
                    //    Category cat = BusinessHandlerCategory.GetCategory(searchData.objectId);
                    //    if (cat != null && !cat.isDeleted)
                    //    {
                    //        searchData.URL = "/category/" + searchData.objectId;
                    //    }
                    //    else
                    //    {
                    //        searchData.URL = null;
                    //    }

                    //}

                }
                // }

                return JsonConvert.SerializeObject(searchDatas.Where(x => x.URL != null).ToList());
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}