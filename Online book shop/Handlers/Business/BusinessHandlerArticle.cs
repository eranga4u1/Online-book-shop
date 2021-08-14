using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerArticle
    {
        public static Article Add(Article article)
        {
            article.CreatedDate = DateTime.UtcNow;
            article.UpdatedDate = DateTime.UtcNow;
            article.isDeleted = false;
            article.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            article.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerArticle.Add(article);
        }

        public static List<Article> GetArticles(bool OnlyNotDelete=false)
        {
            return DBHandlerArticle.GetArticles(OnlyNotDelete);
        }
        internal static Article GetArticlesById(int id)
        {
            Article article= DBHandlerArticle.GetArticlesById(id);
            if (article != null)
            {
                article.MainImage = BusinessHandlerMedia.Get(article.MainPictureMediaId);
                article.CoverImage = BusinessHandlerMedia.Get(article.CoverPictureMediaId);
            }
            return article;
        }

        internal static Article Update(Article article)
        {
            article.UpdatedDate = DateTime.UtcNow;
            article.isDeleted = false;
            article.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerArticle.Update(article);
        }
        internal static bool Show(int id)
        {
            return DBHandlerArticle.ShowHide(id,false);
        }
        internal static bool Hide(int id)
        {
            return DBHandlerArticle.ShowHide(id, true);
        }
    }
}