using Online_book_shop.Handlers.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    public class AdminUtilityController : Controller
    {
        // GET: Admin/AdminUtility
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ResizeImage(string filename, int width, int height)
        {
            try
            {
                var img = new WebImage(filename)
                            .Resize(width, height, false, true);
                return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
            }
            catch(Exception ex)
            {
                return null;
            }

        }
    }
}