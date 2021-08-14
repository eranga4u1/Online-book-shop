using Microsoft.Ajax.Utilities;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static Online_book_shop.Handlers.Search.LuceneService;

namespace Online_book_shop.Handlers.Search
{
    public class SearchHandler
    {
        public static bool ClearAndRecreateSearchIndex()
        {
            LuceneSearch.ClearLuceneIndex();
            CreateSearchIndexFromDB();
            return true;
        }
        public static bool CreateSearchIndexFromDB()
        {
            try
            {
                List<SearchData> data = new List<SearchData>();
                List<Book> BookList = BusinessHandlerBook.GetAllBooks(false);
                List<Author> AuthorList = BusinessHandlerAuthor.GetAuthors();
                List<Publisher> PublisherList = BusinessHandlerPublisher.GetPublishers();
                List<Category> CategoryList = BusinessHandlerCategory.GetCategories();
                foreach (Book book in BookList)
                {
                    data.Add(new SearchData
                    {
                        Id = int.Parse(((int)ObjectTypes.Book).ToString() + book.Id.ToString()),
                        ObjectType = (int)ObjectTypes.Book,
                        objectId = book.Id,
                        Title = book.Title !=null? book.Title:"",
                        Description = book.Description!=null? book.Description : "",
                        LocalTitle=book.LocalTitle !=null? book.LocalTitle:"",
                        ISBN=book.ISBN
                    });
                }
                foreach (Author author in AuthorList)
                {
                    data.Add(new SearchData
                    {
                        Id = int.Parse(((int)ObjectTypes.Author).ToString() + author.Id.ToString()),
                        ObjectType = (int)ObjectTypes.Author,
                        objectId = author.Id,
                        Title = author.Name !=null? author.Name:"",
                        LocalTitle=author.LocalName !=null? author.LocalName:"",
                        Description = author.Description !=null? author.Description : ""
                    });
                }
                foreach (Publisher publisher in PublisherList)
                {
                    data.Add(new SearchData
                    {
                        Id = int.Parse(((int)ObjectTypes.Publisher).ToString() + publisher.Id.ToString()),
                        ObjectType = (int)ObjectTypes.Publisher,
                        objectId = publisher.Id,
                        Title = publisher.Name!=null? publisher.Name:"",
                        LocalTitle=publisher.LocalName!=null? publisher.LocalName:"",
                        Description = publisher.Description!=null? publisher.Description : ""
                    });
                }
                foreach (Category category in CategoryList)
                {
                    data.Add(new SearchData
                    {
                        Id = int.Parse(((int)ObjectTypes.Category).ToString() + category.Id.ToString()),
                        ObjectType = (int)ObjectTypes.Category,
                        objectId = category.Id,
                        Title = category.CategoryName!=null? category.CategoryName :"",
                        LocalTitle=category.LocalCategoryName!=null? category.LocalCategoryName : "",
                        Description = category.CategoryDescription!=null? category.CategoryDescription : ""
                    });
                }
                LuceneSearch.AddUpdateLuceneIndex(data);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }          
        }

        internal static List<SearchData> GetSearchSuggestionBySP(string term)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    SqlParameter param1 = new SqlParameter("@SearchTerm", term);
                    var result= context.Database.SqlQuery<List<Book>>("proc_search @SearchTerm", param1);
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static bool UpdateSearchIndex(object item, ObjectTypes type)
        {
            try
            {
                if (type == ObjectTypes.Book)
                {
                    Book book = (Book)item;
                    LuceneSearch.ClearLuceneIndexRecord(int.Parse(((int)ObjectTypes.Book).ToString() + book.Id.ToString()));
                    LuceneSearch.AddUpdateLuceneIndex(new SearchData
                    {
                        Id = int.Parse(((int)ObjectTypes.Book).ToString() + book.Id.ToString()),
                        ObjectType = (int)ObjectTypes.Book,
                        objectId = book.Id,
                        Title = book.Title,
                        LocalTitle=book.LocalTitle,
                        Description = book.Description,
                        ISBN= book.ISBN
                    });
                    return true;
                }
                else if(type == ObjectTypes.Author)
                {
                    Author author = (Author)item;
                    LuceneSearch.ClearLuceneIndexRecord(int.Parse(((int)ObjectTypes.Author).ToString() + author.Id.ToString()));
                    LuceneSearch.AddUpdateLuceneIndex(new SearchData
                    {
                        Id = int.Parse(((int)ObjectTypes.Author).ToString() + author.Id.ToString()),
                        ObjectType = (int)ObjectTypes.Author,
                        objectId = author.Id,
                        Title = author.Name,
                        LocalTitle=author.LocalName,
                        Description = author.Description
                    });
                    return true;
                }
                else if(type == ObjectTypes.Publisher)
                {
                    Publisher publisher = (Publisher)item;
                    LuceneSearch.ClearLuceneIndexRecord(int.Parse(((int)ObjectTypes.Publisher).ToString() + publisher.Id.ToString()));
                    LuceneSearch.AddUpdateLuceneIndex(new SearchData
                    {
                        Id = int.Parse(((int)ObjectTypes.Publisher).ToString() + publisher.Id.ToString()),
                        ObjectType = (int)ObjectTypes.Publisher,
                        objectId = publisher.Id,
                        Title = publisher.Name,
                        LocalTitle=publisher.LocalName,
                        Description = publisher.Description
                    });
                    return true;
                }
                else if(type== ObjectTypes.Category)
                {
                    Category category = (Category)item;
                    LuceneSearch.ClearLuceneIndexRecord(int.Parse(((int)ObjectTypes.Category).ToString() + category.Id.ToString()));
                    LuceneSearch.AddUpdateLuceneIndex(new SearchData
                    {
                        Id = int.Parse(((int)ObjectTypes.Category).ToString() + category.Id.ToString()),
                        ObjectType = (int)ObjectTypes.Category,
                        objectId = category.Id,
                        Title = category.CategoryName,
                        LocalTitle=category.LocalCategoryName,
                        Description = category.CategoryDescription
                    });
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;//
            }
        }

        public static List<SearchData> Search(string term)
        {
            var results = LuceneSearch.SearchDefault(term);
            return results !=null?results.ToList():null;
        }
        public static List<SearchData> Search(string term, string fieldName = "")
        {
            var results = LuceneSearch.Search(term,fieldName);
            return results != null ? results.ToList() : null;
        }
        public static List<SearchData> GetAllIndexRecords()
        {
            var result = LuceneSearch.GetAllIndexRecords();
            if(result != null)
            {
                return result.ToList();
            }
            else
            {
                return null;
            }
        }
        public static List<SearchData> GetSearchSuggestion(string searchTerm)
        {
            var result = LuceneSearch.GetAllIndexRecords();
            if (result != null)
            {
             var   Rows = (from u in result
                           where    (u.Id.ToString().Contains(searchTerm)) ||
                                    (u.Title !=null && u.Title.ToLower().Contains(searchTerm.ToLower())) || 
                                    (u.ISBN !=null && u.ISBN.Contains(searchTerm)) ||
                                    (u.LocalTitle != null && (u.LocalTitle.ToLower().Contains(searchTerm.ToLower())))
                        select u).ToList();
                return Rows.DistinctBy(m => new { m.ObjectType, m.objectId }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}