using ImageMagick;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Online_book_shop.Handlers.Helper
{
    public class HTMLHelper
    {
        public static bool isImageExist(string path)
        {
            bool result = false;
            try
            {
                var absolutePath = System.Web.HttpContext.Current.Server.MapPath("~/" + path);
                if (System.IO.File.Exists(absolutePath))
                {
                    result = true;
                }
            }
            catch(Exception ex)
            {

            }
         
            return result;
        }
        public static void Test()
        {
            using (var image = new MagickImage("D:\\Project_2020\\Online Book Store\\Online book shop\\Online book shop\\Content\\images\\book4.jpg"))
            {
                var size = new MagickGeometry(100, 100);
                // This will resize the image to a fixed size without maintaining the aspect ratio.
                // Normally an image will be resized to fit inside the specified size.
                size.IgnoreAspectRatio = true;

                image.Resize(size);

                // Save the result
                image.Write("D:\\Project_2020\\Online Book Store\\Online book shop\\Online book shop\\Content\\images\\" + "Snakeware.100x100.png");
            }
        }

        public static Pager GetPager(int currentPage, int numberOfItems, int itemPerPage=12)
        {
            int NumberOfPages = 0;
            if (currentPage == 0)
            {
                currentPage = 1;
            }
            Pager _pager = new Pager();
            _pager.Current = new PagerItem { isEnable = true, LinkPageId = currentPage };
            _pager.Prev= new PagerItem { isEnable = true, LinkPageId = (currentPage-1) };
            _pager.Next = new PagerItem { isEnable = true, LinkPageId = (currentPage + 1) };
            _pager.LinkedPages = new List<int>();

            //int AppNumberOfPages = numberOfItems / itemPerPage;

            //if(AppNumberOfPages* itemPerPage== numberOfItems)
            //{
            //    //No need next page
            //    NumberOfPages = AppNumberOfPages;
            //}else if (AppNumberOfPages * itemPerPage > numberOfItems)
            //{
            //    //No need next page
            //    NumberOfPages = AppNumberOfPages;
            //}
            //else if(AppNumberOfPages * itemPerPage < numberOfItems)
            //{
            //    //need next page
            //    NumberOfPages = (AppNumberOfPages+1);
            //}

             NumberOfPages = (int)Math.Ceiling(Convert.ToDecimal(numberOfItems) / Convert.ToDecimal(itemPerPage));

            if (currentPage == 1)
            {
                _pager.Prev.isEnable = false;
                _pager.Prev.LinkPageId = 0;
                if (NumberOfPages == 1)
                {
                    _pager.Next.isEnable = false;
                    _pager.Next.LinkPageId = 0;
                }              
            }
            else
            {
                if(currentPage== NumberOfPages)
                {
                    _pager.Next.isEnable = false;
                    _pager.Next.LinkPageId = 0;
                }
            }
            _pager.NumberOfPage = NumberOfPages;
           // _pager.LinkedPages.Add(currentPage);

            //current Page --
            for (int i = currentPage; (i >0 && (0> (currentPage-5) || 0 == (currentPage - 5))); i--)
            {
                _pager.LinkedPages.Add(i);
            }

            int remaining = 10 - _pager.LinkedPages.Count;
            ////current Page ++
            for (int i= (currentPage+1); (i<= NumberOfPages && i< (currentPage+ remaining)); i++)
            {
                _pager.LinkedPages.Add(i);
            }

            return _pager;
        }

        public static string TruncateLongString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return str.Length>maxLength?str.Substring(0, Math.Min(str.Length, maxLength))+"...": str;
        }
        public static BookProperties GetBookProperties(List<BookProperties> list)
        {
            return list.Where(p => p.NumberOfCopies > 0).OrderBy(p => p.Price).FirstOrDefault();
        }
        public static string GenarateRandomRef(int number)
        {
            //double pow_ab = number - 11; //Math.Pow(number%10000, 3);
            // double val= number + Math.Pow(number, 2);
                string year = DateTime.Now.Year.ToString();
            return year + "-IN-" + number.ToString() + "-" + GetLetter() + GetTimestamp();
        }
        public static String GetTimestamp()
        {
            return DateTime.Now.ToString("ddMMHHmm");
        }
        static Random _random = new Random();
        public static string GetLetter()
        {
            int num = _random.Next(0, 26); // Zero to 25
            char let = (char)('a' + num);
            return let.ToString().ToUpper();
        }
        public static bool isCartEnabled(int sale_type, int remaining_copies)
        {
            SaleStatus saleStatus = BusinessHandlerSaleStatus.GetSaleStatus(sale_type);
            if(saleStatus != null)
            {
                //if (saleStatus.Title == "pre_order" && saleStatus.isAddToCartEnables)
                //{
                //    return true;
                //}
                //else 
                if (saleStatus.isAddToCartEnables && remaining_copies > 0)
                {
                    return true;
                }
                
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static string GetNoStockMessage(int sale_type)
        {
            SaleStatus saleStatus = BusinessHandlerSaleStatus.GetSaleStatus(sale_type);
            if(saleStatus != null)
            {
                return saleStatus.DisplayText;
            }
            return "Out of stock";
        }
        public static string RemoveSpecialCharacters(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                StringBuilder sb = new StringBuilder();
                str = str.Replace(" ", "-").Replace(".", "-").ToLower();
                foreach (char c in str)
                {
                    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '-')
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString();
            }
            else
            {
                return null;
            }
            
        }

        public static string GetUrl(string friendlyName, int id)
        {
            int bookId;
            bool isNumeric = int.TryParse(friendlyName, out bookId);
            if (isNumeric)
            {
                return id.ToString();
            }
            else if (!string.IsNullOrEmpty(friendlyName))
            {
                return friendlyName;
            }
            return id.ToString();
        }

        public static Cart GetCart(int cartId)
        {
            return BusinessHandlerShopingCart.GetById(cartId);
            //if(_cart != null)
            //{
            //   return _cart.Items.Count();
            //}
            //else
            //{
            //    return 0;
            //}
        }
    }

}