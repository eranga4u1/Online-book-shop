using Online_book_shop.Handlers.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class UtilityController : Controller
    {
        // GET: Utility
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Thumbnail(string filename)
        {
            try
            {
                var img = new WebImage(filename)
                             .Resize(130, 180, false, true);
                return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
            }
            catch
            {
                return null;
                    
            }
         
        }
        public ActionResult HomeBanner(string filename)
        {
            try
            {
                var img = new WebImage(filename)
                            .Resize(1440, 498, false, true);
                return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
            }
            catch
            {
                return null;
            }
        
        }

        public ActionResult ResizeImage(string filename,int width,int height)
        {
            try
            {
                var img = new WebImage(filename)
                            .Resize(width, height, false, true);
                return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
            }
            catch
            {
                return null;
            }

        }
    }
}