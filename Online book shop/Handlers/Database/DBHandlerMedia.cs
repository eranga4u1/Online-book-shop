using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerMedia
    {
        public static Media SaveMedia(Media media)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Medias.Add(media);
                    ctx.SaveChanges();
                }
                return media;
            }
            catch(Exception ex)
            {
                return null;
            }      
        }

        public static Media Get(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                 return   ctx.Medias.Where(m=> m.Id==id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}