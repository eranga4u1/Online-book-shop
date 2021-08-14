using Newtonsoft.Json;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerConfigurations
    {
        public static bool LUCENE_AUTO_UPDATE;
        public static void SetLUCENE_AUTO_UPDATE()
        {
            Online_book_shop.Models.Configuration c = DBHandlerConfigurations.Get();
            if (c != null)
            {
                LUCENE_AUTO_UPDATE = c.Value == "True" ? true : false;
            }
            else
            {
                LUCENE_AUTO_UPDATE = false;
            }

        }
        public static bool GetLUCENE_AUTO_UPDATE()
        {
            Online_book_shop.Models.Configuration c = DBHandlerConfigurations.Get();
            if (c != null)
            {
                return (c.Value == "True" ? true : false);
            }
            return false;
        }
        public static SocialNetworkURLs GetSocialMediaURLs()
        {
            var smedia = DBHandlerConfigurations.Get("SOCAIL_NETWORK_URLS");
            return (smedia != null ? JsonConvert.DeserializeObject<SocialNetworkURLs>(smedia.Value) : null);
        }

        public static Online_book_shop.Models.Configuration AddOrUpdate(Online_book_shop.Models.Configuration config)
        {
            return DBHandlerConfigurations.AddOrUpdate(config);
        }
        public static Online_book_shop.Models.Configuration GetConfigByKey(string key)
        {
            return DBHandlerConfigurations.Get(key);
        }
        //public static List<MenuVM> GetHomeNavigationMenus()
        //{
        //    var  menusConfig = DBHandlerConfigurations.Get("HOME_NAVIGATION");
        //    return  menusConfig != null ? JsonConvert.DeserializeObject<List<MenuVM>>(menusConfig.Value):null;
        //}
        public static List<MenuItem> GetHomeTopNavigationMenus()
        {
            var menusConfig = DBHandlerConfigurations.Get("HOME_TOP_NAVIGATION");
            return menusConfig != null ? JsonConvert.DeserializeObject<List<MenuItem>>(menusConfig.Value) : null;
        }
        //public static List<MenuVM> GetHomeFooterMenus()
        //{
        //    var menusConfig = DBHandlerConfigurations.Get("HOME_FOOTER_NAVIGATION");
        //    return menusConfig != null ? JsonConvert.DeserializeObject<List<MenuVM>>(menusConfig.Value) : null;
        //}
        public static List<MenuItem> GetHomeFooterNavigationMenus()
        {
            var menusConfig = DBHandlerConfigurations.Get("HOME_FOOTER_NAVIGATION");
            return menusConfig != null ? JsonConvert.DeserializeObject<List<MenuItem>>(menusConfig.Value) : null;
        }
        public static string GetHomeContactNumber()
        {
            var menusConfig = DBHandlerConfigurations.Get("CONTACT_NUMBERS");
            if (menusConfig != null)
            {
                var list = JsonConvert.DeserializeObject<List<ContactNumber>>(menusConfig.Value);
                ContactNumber c = list.Where(x => x.ShowsOnHomePage).FirstOrDefault();
                return c.NumberValue;
            }
            else
            {
                return "";
            }
        }
        public static string GetActiveMenu(string controllerName, List<MenuItem> menus)
        {
            if (controllerName.ToLower() == "home")
            {
                return "home";
            }
            else if (controllerName.ToLower() == "author")
            {
                return "author";
            }
            else if (controllerName.ToLower() == "authorprofile")
            {
                return "author";
            }
            else if (controllerName.ToLower() == "book")
            {
                return "books";
            }
            else if (controllerName.ToLower() == "bookprofile")
            {
                return "books";
            }
            else
            {
                string item = "home";
                foreach (MenuItem m in menus)
                {
                    int val = LevenshteinDistance.Compute(controllerName, m.text);
                    if (val < 5)
                    {
                        item = m.text.ToLower();
                    }
                    else if (val < 2)
                    {
                        return m.text.ToLower();
                    }
                }
                return item;
            }
        }

        public static InstantMessage GetInstantMessage()
        {

            Configuration instrant_message = BusinessHandlerConfigurations.GetConfigByKey("INSTANT_MESSAGE");
            if (instrant_message != null && !string.IsNullOrEmpty(instrant_message.Value))
            {
                return JsonConvert.DeserializeObject<InstantMessage>(instrant_message.Value);
            }
            return null;
        }
        //public static bool UpdateBookStatus()
        //{
        //    return DBHandlerConfigurations.UpdateBookStatus();
        //}

        public static List<DeliveryMethod> GetDeliveryMethods()
        {
            List<DeliveryMethod> Obj_delivery_types = new List<DeliveryMethod>();
            Configuration delivery_types = BusinessHandlerConfigurations.GetConfigByKey("DELIVERY_TYPES");
            if (delivery_types != null && !string.IsNullOrEmpty(delivery_types.Value))
            {
                Obj_delivery_types = JsonConvert.DeserializeObject<List<DeliveryMethod>>(delivery_types.Value);
            }
            return Obj_delivery_types;
        }

        public static bool DeliveryMethodstatusById(List<DeliveryMethod> list, int Id)
        {
          var item=  list.Where(x => x.Id == Id).FirstOrDefault();
            if(item != null)
            {
                return item.isEnable;
            }
            return false;
        }
        public static double GetTimeOffSet()
        {
            Configuration clientTimeOffSet = BusinessHandlerConfigurations.GetConfigByKey("TIME_OFFSET");
            if (clientTimeOffSet != null && !string.IsNullOrEmpty(clientTimeOffSet.Value))
            {
                return JsonConvert.DeserializeObject<double>(clientTimeOffSet.Value);
            }
            else
            {
                BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "TIME_OFFSET", Value = JsonConvert.SerializeObject(clientTimeOffSet) });
                return 5.5;
            }
        }
    }
}