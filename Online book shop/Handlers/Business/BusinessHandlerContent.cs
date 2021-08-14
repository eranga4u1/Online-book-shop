using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerContent
    {
        internal static Content PostContent(Content model)
        {
            model.CreatedDate = DateTime.UtcNow;
            model.UpdatedDate = DateTime.UtcNow;
            model.isDeleted = false;
            model.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            model.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return   DBHandlerContent.PostContent(model);
        }

        internal static Content GetById(int Id)
        {
            return DBHandlerContent.GetById(Id);
        }
        internal static Content GetByUrlPart(string url)
        {
            return DBHandlerContent.GetByUrlPart(url);
        }
        internal static List<Content> GetAll()
        {
            return DBHandlerContent.Get();
        }
        internal static Content Put(Content model)
        {
            model.UpdatedDate = DateTime.UtcNow;
            model.isDeleted = false;
            model.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerContent.PutContent(model);
        }

        internal static bool Hide(int id)
        {
         return DBHandlerContent.ShowHide(id, true);
        }

        internal static bool Show(int id)
        {
            return DBHandlerContent.ShowHide(id, false);
        }
    }
}