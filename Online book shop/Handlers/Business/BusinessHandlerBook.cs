using Online_book_shop.Handlers.Database;
using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerBook
    {
        public static Book Add(Book book)
        {
            book.CreatedDate =DateTime.UtcNow;
            book.UpdatedDate = DateTime.UtcNow;
            book.isDeleted =false;
            book.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            book.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            book.FriendlyName = HTMLHelper.RemoveSpecialCharacters(book.Title.Trim());
            return DBHandlerBook.Add(book);
        }

        internal static List<BookVMTile> GetBookPacksForView()
        {
            return DBHandlerBook.GetBookPacksForView();
        }

        public static int GetIdFromFriendlyName(string name)
        {
            return DBHandlerBook.GetIdFromFriendlyName(name);
        }

        internal static List<BookVMTile> GetBooksByCategory(int id, int page)
        {
            return DBHandlerBook.GetBooksByCategory(id,page);
        }

        internal static List<BookVMTile> GetBestSellingBooksForView()
        {
            return DBHandlerBook.GetBestSellingBooksForView();
        }

        public static List<Book> GetAllBooks(bool withDeleted)
        {
            return DBHandlerBook.GetAllBooks(withDeleted);
        }
        public static Book Get(int id)
        {
            return DBHandlerBook.Get(id);
        }
        public static Book Put(Book book)
        {
            book.UpdatedDate = DateTime.UtcNow;
            book.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            book.FriendlyName = HTMLHelper.RemoveSpecialCharacters(book.Title.Trim());
            return DBHandlerBook.Put(book);
        }

        public static BookPrice GetBookPriceInfo(int bookId, int bookPropId )
        {
            BookPrice bookPrice = new BookPrice();
            BookProperties bookProperties = BusinessHandlerBookProperties.GetById(bookPropId);
            if(bookProperties != null)
            {
                bookPrice.UnitPrice = bookProperties.Price;// Convert.ToDecimal(345.50);
                bookPrice.Discount = BusinessDiscountHandler.GetDiscountAmountForBook(bookId,bookPropId,bookPrice.UnitPrice);// Convert.ToDecimal(25.20);
                bookPrice.PriceAfterDiscount = Convert.ToDecimal(bookPrice.UnitPrice - bookPrice.Discount);
                bookPrice.ItemInStock = bookProperties.NumberOfCopies;
            }        
            return bookPrice;
        }

        internal static Dictionary<int, int> GetStock(int bookId)
        {
            return DBHandlerBook.GetStock(bookId);
        }

        public static List<BookVMTile> GetPreOrderBooksForView()
        {
            return DBHandlerBook.GetPreOrderBooksForView();
        }
        public static Media GetBookFrontCover(int bookId)
        {
            return DBHandlerBook.GetBookFrontCover(bookId);
        } 
        public static List<BookVMTile> GetLatestBooksForView()
        {
            return DBHandlerBook.GetLatestBooksForView();
        }
        public static BookVMTile GetSearchedBookForView(int bookId)
        {
            return DBHandlerBook.GetSearchedBookForView(bookId);
        }
        public static BookVMTile GetBookTileByBookId(int bookId)
        {
            return DBHandlerBook.GetBookTileByBookId(bookId);
        }
        internal static bool Show(int id)
        {
            return DBHandlerBook.ShowHide(id, false);
        }
        internal static bool Hide(int id)
        {
            return DBHandlerBook.ShowHide(id, true);
        }
        internal static bool ShowBookPack(int id)
        {
            return DBHandlerBook.ShowHideBookPack(id, false);
        }
        internal static bool HideBookPack(int id)
        {
            return DBHandlerBook.ShowHideBookPack(id, true);
        }

        internal static List<BookVMTile> GetBooksByCategories(List<int> categoryId)
        {
            return DBHandlerBook.GetBooksByCategories(categoryId);
        }

        internal static List<BookVMTile> GetBooksByAuthor(int Authord,int pageId=0)
        {
            return DBHandlerBook.GetBooksByAuthor(Authord, pageId);
        }
        internal static List<BookVMTile> GetMultiAuthorBooks(int Authord)
        {
            return DBHandlerBook.GetMultiAuthorBooks(Authord);
        }

        internal static List<BookVMTile> GetBooksByPublisher(int publisherId, int pageId = 0)
        {
            return DBHandlerBook.GetBooksByPublisher(publisherId, pageId);
        }

        internal static List<BookVMTile> GetAllBooksForView()
        {
            return DBHandlerBook.GetAllBooksForView();
        }
        internal static List<DataObjVM> GetAllBooksWithPropertyAsNewOne()
        {
            return DBHandlerBook.GetAllBooksWithPropertyAsNewOne();
        }

        internal static List<ItemPack> GetAllItemPack(bool withDeleted = false)
        {
            return DBHandlerBook.GetAllItemPack(withDeleted);
        }
        internal static bool AddBookToBookPack(int bookId, int bookPropertyId,int itemPackId, int numberOfBooks=1)
        {
            return DBHandlerBook.AddBookToBookPack(bookId, bookPropertyId, itemPackId, numberOfBooks);
        }
        internal static ItemPack AddItemPack(ItemPack model)
        {

            model.UpdatedDate = DateTime.UtcNow;
            model.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            model.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            model.CreatedDate = DateTime.UtcNow;
            return DBHandlerBook.AddItemPack(model);
        }

        internal static bool AddItemMedia(ItemMedia itemMedia)
        {
            return DBHandlerBook.AddItemMedia(itemMedia);
        }

        internal static bool AddItemPack(List<ItemMedia> mediaList)
        {
           return DBHandlerBook.AddItemMedia(mediaList);
        }
        internal static bool AddItemPack_Book(List<ItemPack_Item> list)
        {
            return DBHandlerBook.AddItemPack_Book(list);
        }
        internal static ItemPack GetItemPack(int id)
        {
            return DBHandlerBook.GetItemPack(id);
        }

        internal static List<ItemPack_Item> GetBookForPack(int id)
        {
            return DBHandlerBook.GetBookForPack(id);
        }

        public static List<ItemMedia> GetOtherMedia(int id,ObjectTypes type)
        {
            return DBHandlerBook.GetOtherMedia(id,type);
        }

       
        public static JsonResult RemoveMediaItem(ItemMedia model)
        {
            JsonResult jr = new JsonResult();
            if(DBHandlerBook.RemoveMediaItem(model))
            {
                jr.Data = "success";
            }
            else
            {
                jr.Data = "failed";
            }
            return jr;
        }

        internal static ItemPack UpdateItemPack(ItemPack model)
        {
            model.UpdatedDate = DateTime.UtcNow;

            model.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();

            return DBHandlerBook.UpdateItemPack(model);
        }

        internal static bool UpdateBookStatus()
        {
            try
            {
              return  DBHandlerBook.ChangePreOrderScheduller();
            }
            catch(Exception ex)
            {

            }
            return true;
        }

        internal static bool isPreOrderDissabled(int bookId)
        {
            try
            {
               return DBHandlerBook.isPreOrderDissabled(bookId);
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        internal static bool isOrderLimitExceed(int bookId, int numberOfItem)
        {
            return DBHandlerBook.isOrderLimitExceed(bookId, numberOfItem);
        }
        internal static StockEntry UpdateBookStockAddBookPackItem(int BookId , int PropertyId , int NumberOfBooks)
        {
            if(DBHandlerBook.UpdateBookStockAddBookPackItem(BookId, PropertyId, NumberOfBooks))
            {
              return  BusinessHandlerStockEntry.Update(BookId, NumberOfBooks, PropertyId, StockEntryOperation.Out_For_Book_Pack);
            }
            return null;
        }
    }
}