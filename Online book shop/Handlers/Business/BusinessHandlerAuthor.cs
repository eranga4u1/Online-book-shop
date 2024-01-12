using Microsoft.AspNet.Identity;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerAuthor
    {
        public static string GetLoginUserId()
        {
            try
            {
                var User = HttpContext.Current.User;
                return User.Identity.GetUserId();
            }
            catch(Exception ex)
            {
                return "Annonymous";
            }
           
        }
        public static bool SaveNewAuthor(Author author)
        {
            author.FriendlyName = HTMLHelper.RemoveSpecialCharacters(author.Name.Trim());
            if (DBHandlerAuthor.SaveAuthor(author) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static int GetAuthorIdByFriendlyName(string id)
        {
           return DBHandlerAuthor.GetAuthorIdByFriendlyName(id);
        }

        public static List<Author> GetAuthors()
        {
          return DBHandlerAuthor.GetAuthors();
        }
        public static Author GetAuthorById(int id)
        {
            return DBHandlerAuthor.GetAuthor(id);
        }
        public static Author UpdateAuthor(Author author)
        {
            author.FriendlyName = HTMLHelper.RemoveSpecialCharacters(author.Name.Trim());
            return DBHandlerAuthor.UpdateAuthor(author);
        }
        internal static bool Show(int id)
        {
            return DBHandlerAuthor.ShowHide(id, false);
        }
        internal static bool Hide(int id)
        {
            return DBHandlerAuthor.ShowHide(id, true);
        }
        public static List<Author> GetmultipleAuthors(int bookId)
        {
            return DBHandlerAuthor.GetmultipleAuthors(bookId);
        }

        internal static void AddMultipleAuthor(int id, int a)
        {
            DBHandlerAuthor.AddMultipleAuthor( id,  a);
        }

        internal static bool RemoveAuthorFromBook(Book_Author model)
        {
          return  DBHandlerAuthor.RemoveAuthorFromBook(model);
        }
    }
}