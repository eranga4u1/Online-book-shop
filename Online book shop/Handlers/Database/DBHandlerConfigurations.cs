using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerConfigurations
    {
        public static Online_book_shop.Models.Configuration Get()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Configurations.Where(b => b.Key.ToString().Trim() == "LUCENE_AUTO_UPDATE").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Online_book_shop.Models.Configuration AddOrUpdate(Online_book_shop.Models.Configuration config)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var exist = ctx.Configurations.Where(x => x.Key == config.Key).FirstOrDefault();
                    if (exist == null)
                    {
                        ctx.Configurations.Add(config);
                    }
                    else
                    {
                        exist.Value = config.Value;
                    }
                    
                    if (ctx.SaveChanges() > 0)
                    {
                        return config;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Online_book_shop.Models.Configuration Get(string Key)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Configurations.Where(b => b.Key.ToString().Trim() == Key).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //internal static bool UpdateBookStatus()
        //{
        //    try
        //    {
        //        using (var ctx = new ApplicationDbContext())
        //        {
        //            return ctx.Configurations.Where(b => b.Key.ToString().Trim() == Key).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}