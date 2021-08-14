using Online_book_shop.Handlers.Database;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerRecentlyVisitedItems
    {
        public static bool Add(int itemId)
        {
            string loginUser = BusinessHandlerAuthor.GetLoginUserId();
            if (!string.IsNullOrEmpty(loginUser))
            {
                return DBHandlerRecentlyVisitedItems.AddRecentlyVisitedItem(loginUser, itemId);
            }
            else
            {
                return false;
            }
            
        }
        public static List<BookVMTile> Get()
        {
            string loginUser = BusinessHandlerAuthor.GetLoginUserId();
            if (!string.IsNullOrEmpty(loginUser))
            {
                return DBHandlerRecentlyVisitedItems.GetRecentViewsByUser(loginUser);
            }
            else
            {
                return null;
            }
               
        }
        public static IEnumerable<int> StringToIntList(string str)
        {
            if (String.IsNullOrEmpty(str))
                yield break;

            foreach (var s in str.Split(','))
            {
                int num;
                if (int.TryParse(s, out num))
                    yield return num;
            }
        }
        public static string GetStringRecentViews(string value,int newId)
        {
            string s = value;
            value.Replace(string.Format(",{0}", newId.ToString()),"").Replace(string.Format(",{0}", newId.ToString()), "");
            if (StringToIntList(value).Count() > 10)
            {
                s = string.Join(",", value.Split(',').Select(str => str.TrimStart('0')));
            }
            
            return string.Format(s + ",{0}", newId.ToString());

        }
    }
}