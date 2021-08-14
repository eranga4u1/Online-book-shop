using Newtonsoft.Json;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerMedia
    {
        public static Media CreateNewMediaEntry(HttpPostedFileBase file, MediaCategory mediaCategory)
        {
            Media media = new Media();
            string path = "";
            media.MediaCategory = (int)mediaCategory;
            if (mediaCategory == MediaCategory.ProfilePicture)
            {
                media.FileLocation = "/ProfilePicture/";
                path = "\\Content\\UploadFiles\\Images\\ProfilePicture";
            }
            else if (mediaCategory == MediaCategory.CoverImage)
            {
                media.FileLocation = "/CoverImage/";
                path = "\\Content\\UploadFiles\\Images\\CoverImage";

            }
            else if (mediaCategory == MediaCategory.BookFrontCover)
            {
                media.FileLocation = "/BookFrontCover/";
                path = "\\Content\\UploadFiles\\Images\\BookFrontCover";

            }
            else if (mediaCategory == MediaCategory.BookBackCover)
            {
                media.FileLocation = "/BookBackCover/";
                path = "\\Content\\UploadFiles\\Images\\BookBackCover";

            }
            else if (mediaCategory == MediaCategory.ReadablePDF)
            {
                media.FileLocation = "/PDF/";
                path = "\\Content\\UploadFiles\\PDF";
            }
            else
            {
                media.FileLocation = "/Uncategorise/";
                path = "\\Content\\UploadFiles\\PDF";

            }
            string fileName = saveFileInFolder(file, path);

            if (!string.IsNullOrEmpty(fileName))
            {
                media.FileName = fileName;
                media.FileExtention = Path.GetExtension(file.FileName);
                media.CreatedDate = DateTime.Now;
                media.UpdatedDate = DateTime.Now;
                media.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                media.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
                media.isDeleted = false;
                return BusinessHandlerMedia.SaveMedia(media);
            }
            else
            {
                return null;
            }
           

        }
        public static string saveFileInFolder(HttpPostedFileBase file, string fileLocation)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);// Path.GetFileName(file.FileName);
                file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(fileLocation), fileName));
                return fileName;
            }
            catch (Exception ex) { return null; }

        }

        public static Media SaveMedia(Media media)
        {
            return DBHandlerMedia.SaveMedia(media);          
        }
        public static Media Get(int id)
        {
            return DBHandlerMedia.Get(id);
        }
        public static List<Media> GetHomeBanners()
        {
            try
            {
                var Object = DBHandlerConfigurations.Get("HOME_BANNER_IMAGES");
                return (Object != null && !string.IsNullOrEmpty(Object.Value)) ? JsonConvert.DeserializeObject<List<Media>>(Object.Value) : null;
            }
            catch (Exception ex)
            {
                return null;
            }
      }

        public static string ApplyWaterMark(string sourceFile, string targetImage)
        {
            return write_watermark_text(sourceFile,"Muses Publishing House", targetImage);
        }
        public static string write_watermark_text(string sourceImage, string text, string targetImage)
        {
            try
            {

                //First we have to take the image from the file and here enable Embedded Color management
                //loaded this related image to the Image variable.
                Image img = Image.FromFile(sourceImage, true);

                //Then we need to get the image width and height.
                //We will use this values to set font size according to image size.
                //Also we are going to set the image starting location according to image dimensions.
                int width = img.Width;
                int height = img.Height;
                int font_size = (width > height ? height : width) / 12;

                //We set text starting location according to image size.
                //If we do not be carefull about this, the text can be overflow from image.
                //So we prevent this. 
                Point text_starting_point = new Point(0, (width/2));

                // Later we set the font of the text. I set the Comic Sans MS as font family and 
                //I set the font size according to image size dynamically.
                Font text_font = new Font("Comic Sans MS", font_size, FontStyle.Bold, GraphicsUnit.Pixel);

                //Also we set the text color and transparency of the text.
                //And we create the Brush to write the text with specified color and trancperancy.
                Color color = Color.FromArgb(100, 255, 0, 0);
                SolidBrush brush = new SolidBrush(color);

                //To write a text we create a Graphic component and load the above image inside.
                //Then we draw our string with specified font, color and transparency at specified location above.
                //After our string drawing we will dispose the image to reduce the memory usage.
                Graphics graphics = Graphics.FromImage(img);
                graphics.DrawString(text, text_font, brush, text_starting_point);
                graphics.Dispose();

                //After all we need to save our image to the target location.
                img.Save(targetImage);

               
                //And dispose the image to reduce memory usage of this component.
                img.Dispose();

                return targetImage;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }
    }
}