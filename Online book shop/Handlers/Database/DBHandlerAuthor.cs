using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Search;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerAuthor
    {
        public static Author SaveAuthor(Author author)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Authors.Add(author);
                    if (ctx.SaveChanges() > 0 && BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                    {
                        SearchHandler.UpdateSearchIndex(author, ObjectTypes.Author);
                    }
                }
                return author;
            }
            catch(Exception ex)
            {
                return null;
            }

        }
        public static List<Author> GetAuthors()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var authors = ctx.Authors.OrderBy(author => author.Name).ToList();
                    foreach(var a in authors)
                    {
                        a.ProfileImage = ctx.Medias.Where(media => media.Id == a.ProfilePictureMediaId).FirstOrDefault();
                    }
                    return authors;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static int GetAuthorIdByFriendlyName(string id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var author = ctx.Authors.Where(a => a.FriendlyName.Trim() == id.Trim()).FirstOrDefault();
                    if (author != null)
                    {
                        return author.Id;
                    }
               
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.ToString(), "GetAuthorIdByFriendlyName", "DBHandlerAuthor");
            }
            return 0;
        }

        internal static List<Author> GetmultipleAuthors(int bookId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    List<int> mulAuthors = ctx.Book_Authors.Where(x => x.BookId == bookId && !x.isDeleted).Select(y => y.AuthorId).ToList();
                    List<Author> author = ctx.Authors.Where(a => mulAuthors.Any(y=> y==a.Id)).ToList();
                    
                    if (author != null)
                    {
                        foreach (Author a in author)
                        {
                            a.ProfileImage = ctx.Medias.Where(media => media.Id == a.ProfilePictureMediaId).FirstOrDefault();
                            a.CoverImage = ctx.Medias.Where(media => media.Id == a.CoverPictureMediaId).FirstOrDefault();
                        }
                  
                    }
                    return author;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool RemoveAuthorFromBook(Book_Author book_Author)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   var Obj= ctx.Book_Authors.Where(x => x.BookId == book_Author.BookId && x.AuthorId == book_Author.AuthorId ).FirstOrDefault();
                    if(Obj != null){
                        Obj.isDeleted = true;
                        if (ctx.SaveChanges()>0)
                        {
                            return true;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        internal static void AddMultipleAuthor(int id, int a)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Obj = ctx.Book_Authors.Where(x => x.BookId == id && x.AuthorId == a).FirstOrDefault();
                    if(Obj !=null)
                    {
                        Obj.isDeleted = false;
                    }
                    else
                    {
                        ctx.Book_Authors.Add(
                   new Book_Author
                   {
                       BookId = id,
                       AuthorId = a,
                       CreatedDate = DateTime.Today,
                       UpdatedDate = DateTime.Today,
                       isDeleted = false,
                       CreatedBy = BusinessHandlerAuthor.GetLoginUserId(),
                       UpdatedBy = BusinessHandlerAuthor.GetLoginUserId()
                   }
                   );
                    }
               
                    ctx.SaveChanges();                
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static Author UpdateAuthor(Author author)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Author a = ctx.Authors.Where(x => x.Id == author.Id).FirstOrDefault();
                    if(a != null)
                    {
                        a.ContactNumber = author.ContactNumber;
                        a.Description = author.Description;
                        a.Email = author.Email;
                        a.ExternalURL = author.ExternalURL;
                        a.Name = author.Name;
                        a.LocalName = author.LocalName;
                        if (author.ProfilePictureMediaId > 0)
                        {
                            a.ProfilePictureMediaId = author.ProfilePictureMediaId;
                        }
                        a.UpdatedBy = author.UpdatedBy;
                        a.UpdatedDate = author.UpdatedDate;
                        a.FriendlyName = author.FriendlyName;
                        ctx.SaveChanges();
                        if (BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                        {
                            SearchHandler.UpdateSearchIndex(a, ObjectTypes.Author);
                        }
                    }
                 
                }
              
                return author;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Author GetAuthor(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var author = ctx.Authors.Where(a=> a.Id==id).FirstOrDefault();
                    if(author != null)
                    {
                        author.ProfileImage = ctx.Medias.Where(media => media.Id == author.ProfilePictureMediaId).FirstOrDefault();
                        author.CoverImage = ctx.Medias.Where(media => media.Id == author.CoverPictureMediaId).FirstOrDefault();
                    }
                    return author;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool ShowHide(int id, bool option)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Author author = ctx.Authors.Where(x => x.Id == id).FirstOrDefault();
                    if (author != null)
                    {
                        author.isDeleted = option;
                        author.UpdatedDate = DateTime.UtcNow;
                        author.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        if (ctx.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}