using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerArticle
    {
        public static Article Add(Article article)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Articles.Add(article);
                    if (ctx.SaveChanges() > 0)
                    {
                        return article;
                    }
                    //if (ctx.SaveChanges() > 0 && BusinessHandlerConfigurations.LUCENE_AUTO_UPDATE)
                    //{
                    //    SearchHandler.UpdateSearchIndex(book, ObjectTypes.Book);
                    //}
                }
                return article;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Article GetArticlesById(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Articles.Where(x=> x.Id==id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<Article> GetArticles(bool OnlyNotDelete)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if (OnlyNotDelete)
                    {
                        return ctx.Articles.ToList().Where(x=> !x.isDeleted).OrderByDescending(x=>x.CreatedDate).ToList();
                    }
                    else
                    {
                        return ctx.Articles.OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static bool ShowHide(int id,bool option)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Article Dbarticle = ctx.Articles.Where(x => x.Id == id).FirstOrDefault();
                    if (Dbarticle != null)
                    {
                        Dbarticle.isDeleted = option;
                        Dbarticle.UpdatedDate = DateTime.UtcNow;
                        Dbarticle.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        if (ctx.SaveChanges() > 0) {
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
        internal static Article Update(Article article)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Article Dbarticle= ctx.Articles.Where(x => x.Id == article.Id).FirstOrDefault();
                    if(Dbarticle != null)
                    {
                        Dbarticle.Title = article.Title;
                        Dbarticle.Summary = article.Summary;
                        Dbarticle.Url = article.Url;
                        Dbarticle.Content = article.Content;
                        Dbarticle.WrittenBy = article.WrittenBy;
                        Dbarticle.MainPictureMediaId = article.MainPictureMediaId==0? Dbarticle.MainPictureMediaId: article.MainPictureMediaId;
                        Dbarticle.CoverPictureMediaId = article.CoverPictureMediaId==0? Dbarticle.CoverPictureMediaId: article.CoverPictureMediaId;
                        Dbarticle.UpdatedBy = article.UpdatedBy;
                        Dbarticle.UpdatedDate = article.UpdatedDate;
                        ctx.SaveChanges();
                    }
                    return Dbarticle;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}