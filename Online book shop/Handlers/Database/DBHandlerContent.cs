using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerContent
    {
        internal static Content PostContent(Content model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Content.Add(model);
                    if (ctx.SaveChanges() > 0)
                    {
                        return model;
                    }
                    //if (ctx.SaveChanges() > 0 && BusinessHandlerConfigurations.LUCENE_AUTO_UPDATE)
                    //{
                    //    SearchHandler.UpdateSearchIndex(book, ObjectTypes.Book);
                    //}
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Content GetByUrlPart(string url)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Content.Where(X => X.URLSubString.ToLower() == url.ToLower()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Content GetById(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Content.Where(X => X.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        internal static Content PutContent(Content model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var dbContent = ctx.Content.Where(X => X.Id == model.Id).FirstOrDefault();
                    if(dbContent != null)
                    {
                        dbContent.Title = model.Title;
                        dbContent.URLSubString = model.URLSubString;
                        dbContent.HTMLContent = model.HTMLContent;
                        dbContent.UpdatedBy = model.UpdatedBy;
                        dbContent.UpdatedDate = model.UpdatedDate;
                        ctx.SaveChanges();
                    }
                    return dbContent;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<Content> Get()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Content.ToList();
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
                    Content Dbarticle = ctx.Content.Where(x => x.Id == id).FirstOrDefault();
                    if (Dbarticle != null)
                    {
                        Dbarticle.isDeleted = option;
                        Dbarticle.UpdatedDate = DateTime.UtcNow;
                        Dbarticle.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
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