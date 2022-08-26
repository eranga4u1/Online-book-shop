using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Search;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerBook
    {
        public static Book Add(Book book)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Books.Add(book);
                    if(ctx.SaveChanges()>0 && BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                    {
                        SearchHandler.UpdateSearchIndex(book,ObjectTypes.Book);
                    }
                }
                return book;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static int GetIdFromFriendlyName(string name)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Obj= ctx.Books.Where(x => x.FriendlyName == name).FirstOrDefault();
                    if(Obj != null)
                    {
                        return Obj.Id;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return 0;
        }

        internal static List<BookVMTile> GetBooksByCategory(int id, int page)
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                List<int> categoryIds = new List<int>() { id};

                using (var ctx = new ApplicationDbContext())
                {
                    var selectedBooks = from bc in ctx.Book_Categories where  !bc.isDeleted
                                        join cid in categoryIds
                                         on bc.CategoryId equals cid
                                        join bk in ctx.Books
                                             on bc.BookId equals bk.Id
                                        select bk;
                    var preOrderBooks = from a in (selectedBooks)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType=a.SaleType,
                                            Url=a.FriendlyName,
                                            CreatedDate=a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                            c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    return preOrderBooks.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<BookVMTile> GetAllActiveBookPacks()
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    DateTime slTime = DateTime.UtcNow.AddHours(5.5);
                    var selectedBooks = ctx.Books.Where(x => !x.isDeleted && x.ItemType == (int)ItemType.BookPack && (x.AvailableUntil >= slTime)).ToList();
                    var Bookpacks = from a in (from q in selectedBooks
                                               orderby q.CreatedDate descending
                                                   select q)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType = a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate = a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                        c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    var returnList = Bookpacks.Where(x => !x.isDeleted).ToList();
                    return returnList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Dictionary<int, int> GetStock(int bookId)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    List<BookProperties> bp = ctx.BookProperties.Where(x => x.BookId == bookId).ToList();
                    if(bp != null)
                    {
                        foreach(BookProperties p in bp)
                        {
                            dic.Add(p.BookId, p.NumberOfCopies);
                        }
                        return dic;
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

        internal static List<DataObjVM> GetAllBooksWithPropertyAsNewOneForBookPack()
        {
            try
            {
                SaleStatus preOrder = BusinessHandlerSaleStatus.GetSaleStatusByTitle("pre_order");
                SaleStatus NormalSaleType = BusinessHandlerSaleStatus.GetSaleStatusByTitle("normal_sale");
                //SaleStatus LimitedStockType = BusinessHandlerSaleStatus.GetSaleStatusByTitle("limited_stock");

                using (var ctx = new ApplicationDbContext())
                {

                    var books = from a in ctx.BookProperties
                                join
                                b in ctx.Books.Where(x=> (x.SaleType == preOrder.Id || x.SaleType == NormalSaleType.Id) && x.ItemType==(int)ItemType.Book) on a.BookId equals b.Id
                                select (new DataObjVM
                                {
                                    Id = b.Id,
                                    Name = b.Title + " : " + a.Title,
                                    ObjType = 0,
                                    OtherPara = "{\"BookPropertyId\":" + a.Id + "}",
                                    BookAuthorId = b.AuthorId,
                                    BookPublisherId = b.PublisherId
                                });
                    var result = books.ToList();


                    if (result != null)
                    {
                        return result;

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

        internal static bool AddBookToBookPack(int itemId, int itemPropertyId,int itemPackId, int numberOfitems)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    int numberOfRemainingBooks = 0;
                    var bookProperties = ctx.BookProperties.Where(x => x.BookId == itemId && x.Id == itemPropertyId && !x.isDeleted).FirstOrDefault();
                    if(bookProperties != null)
                    {
                        numberOfRemainingBooks = bookProperties.NumberOfCopies;
                        if(numberOfRemainingBooks > 0 && (numberOfRemainingBooks> numberOfitems || numberOfRemainingBooks == numberOfitems))
                        {
                           if(DBHandlerBookProperties.UpdateNumberOfBooks(itemId, itemPropertyId, (numberOfitems * (-1))))
                            {
                                ItemPack_Item itemPack_Item = new ItemPack_Item { ItemId = itemId, ItemPropertyId = itemPropertyId, NumberOfItems = numberOfitems };
                                ctx.ItemPack_Items.Add(itemPack_Item);
                                if (ctx.SaveChanges() > 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                               
                            }
                        }
                    }
                }
                return false;
            }catch(Exception ex)
            {
                return false;
            }
        }

        internal static ItemPack UpdateItemPack(ItemPack model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ItemPack dbBook = ctx.ItemPacks.Where(b => b.Id == model.Id).FirstOrDefault();
                   // dbBook.ISBN = book.ISBN;
                    dbBook.Title = model.Title;
                    dbBook.UpdatedBy = model.UpdatedBy;
                    dbBook.UpdatedDate = model.UpdatedDate;
                    //dbBook.PublisherId = book.PublisherId;
                    //dbBook.AuthorId = book.AuthorId;
                    dbBook.LocalTitle = model.LocalTitle;
                    //dbBook.Ratings = book.Ratings;
                    dbBook.SaleType = model.SaleType;
                    //dbBook.RelaseDate = model.RelaseDate;
                    dbBook.Description = model.Description;
                    //dbBook.FriendlyName = book.FriendlyName;
                    dbBook.YoutubeUrl = model.YoutubeUrl;
                    //dbBook.MaximumItemPerOrder = book.MaximumItemPerOrder;
                    ctx.SaveChanges();
                    //if (BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                    //{
                    //    SearchHandler.UpdateSearchIndex(book, ObjectTypes.Book);
                    //}
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<DataObjVM> GetBookPackItemByBookPackId(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    
                    var list = ctx.ItemPack_Items.Where(x => x.ItemPackId == id && !x.isDeleted);
                    var books = from a in ctx.BookProperties
                                join
                                b in ctx.Books on a.BookId equals b.Id
                                select (new DataObjVM
                                {
                                    Id = b.Id,
                                    Name = b.Title + " : " + a.Title,
                                    ObjType = 0,
                                    OtherPara = "{\"BookPropertyId\":" + a.Id + "}",
                                    BookAuthorId = b.AuthorId,
                                    BookPublisherId = b.PublisherId
                                });
                    var result = books.Where(x=> list.Any(y=> y.ItemId==x.Id && "{\"BookPropertyId\":" + y.ItemPropertyId + "}"==x.OtherPara));


                    if (result != null)
                    {
                        return result.ToList();

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

        internal static bool ReleaseBookPack(int bookPackId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                  var book=  ctx.Books.Where(x => x.Id == bookPackId).FirstOrDefault();
                    if(book != null)
                    {

                        book.isDeleted = true;
                        book.UpdatedDate = DateTime.UtcNow;
                    }
                    var items=  ctx.ItemPack_Items.Where(x=> x.ItemPackId==bookPackId);
                    var bookproperty = ctx.BookProperties.Where(x => x.BookId == bookPackId).FirstOrDefault();
                    if(items != null && bookproperty !=null)
                    {
                        foreach(var i in items)
                        {
                            i.isDeleted = true;
                            i.DeletedDate = DateTime.UtcNow;
                            BusinessHandlerBook.UpdateBookStockAddBookPackItem(book.Id, i.ItemPropertyId, bookproperty.NumberOfCopies);
                            DBHandlerBookProperties.UpdateNumberOfBooks(book.Id, i.ItemPropertyId, bookproperty.NumberOfCopies);
                        }

                    }

                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static bool UpdateBookStockAddBookPackItem(int bookId, int propertyId, int numberOfBookPacks)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var bookProperty = ctx.BookProperties.Where(x => x.BookId == bookId && x.Id == propertyId).FirstOrDefault();
                    if(bookProperty != null && (bookProperty.NumberOfCopies <= numberOfBookPacks))
                    {
                        bookProperty.NumberOfCopies = bookProperty.NumberOfCopies - numberOfBookPacks;
                        bookProperty.UpdatedDate = DateTime.UtcNow;
                        bookProperty.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        if (ctx.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static List<ItemMedia> GetOtherMedia(int id, ObjectTypes type)
        {
            try
            {
                 using (var ctx = new ApplicationDbContext())
                {
                    return ctx.ItemMedias.Where(x => x.ObjectId == id && x.ObjectType==(int)type && !x.IsDeleted).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool isPreOrderDissabled(int bookId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Book book = ctx.Books.Where(x => x.Id == bookId).FirstOrDefault();
                    SaleStatus pre_order= ctx.SaleStatus.Where(x => x.Title == "pre_order").FirstOrDefault();
                    if (book != null && pre_order !=null)
                    {
                               if(book.SaleType== pre_order.Id)
                        {
                            if (DateTime.UtcNow > book.RelaseDate)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
  
            }
            return false;
        }

        internal static bool isOrderLimitExceed(int bookId, int numberOfItem)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Obj = ctx.Books.Where(x => x.Id == bookId).FirstOrDefault();
                    if (Obj != null)
                    {
                        if (Obj.MaximumItemPerOrder == 0)
                        {
                            return false;
                        }
                        else if (Obj.MaximumItemPerOrder>=numberOfItem)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        };
                    }
                    
                }

            }
            catch (Exception ex)
            {
               
            }
            return true;
        }

        internal static bool RemoveMediaItem(ItemMedia model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Obj = ctx.ItemMedias.Where(x => x.Id == model.Id).FirstOrDefault();
                    if(Obj != null)
                    {
                        Obj.IsDeleted = true;
                        
                    }
                   if(ctx.SaveChanges() > 0){ 
                        return true; 
                    }else {
                        return false;
                    };
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static List<ItemPack_Item> GetBookForPack(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.ItemPack_Items.Where(x => x.ItemPackId== id).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static ItemPack GetItemPack(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.ItemPacks.Where(x => x.Id == id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool AddItemMedia(List<ItemMedia> mediaList)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.ItemMedias.AddRange(mediaList);
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static bool AddItemMedia(ItemMedia itemMedia)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.ItemMedias.Add(itemMedia);
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static List<ItemPack> GetAllItemPack()
        {
            List<ItemPack> list = new List<ItemPack>();
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    list = ctx.ItemPacks.Where(x => !x.isDeleted).ToList();
                }

            }catch(Exception ex)
            {

            }
            return list;
        }

        internal static List<BookVMTile> GetBestSellingBooksForView()
        {
            try
            {

                List<BookVMTile> list = new List<BookVMTile>();
                DateTime fromDate = DateTime.UtcNow.AddDays(-7);

                using (var ctx = new ApplicationDbContext())
                {
                    var result = ctx.StockEntries.Where(x=> x.Operation=="Out" && x.CreatedDate>=fromDate).GroupBy(x => x.BookId).Select(g => new {
                                        BookId =g.Select(x=>x.BookId).FirstOrDefault(),
                                        Total = g.Sum(x => x.NumberOfBook)
                                    }).OrderByDescending(x=> x.Total).Take(12).Select(i=> i.BookId).ToList();

                  var bookResults = from a in (from q in ctx.Books where  result.Any(y=> y==q.Id) && !q.isDeleted && q.ItemType == (int)ItemType.Book
                                                 orderby q.CreatedDate descending
                                                   select q )
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted=b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType = a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate=a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                        c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    //List<BookVMTile> filtered = new List<BookVMTile>();
                    //foreach(var i in result.ToList())
                    //{
                    //  var selected=  preOrderBooks.Where(p => p.Id == i.BookId && !p.isDeleted).FirstOrDefault();
                    //    if(selected != null)
                    //    {
                    //        filtered.Add(selected);
                    //    }
                    //}
                    var test = bookResults.ToList<BookVMTile>();
                    return bookResults !=null?bookResults.ToList<BookVMTile>():null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<DataObjVM> GetAllBooksWithPropertyAsNewOne()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {

                    var books = from a in ctx.BookProperties
                                join
                                b in ctx.Books on a.BookId equals b.Id select (new DataObjVM
                                {
                                    Id = b.Id,
                                    Name = b.Title + " : " + a.Title,
                                    ObjType = 0,
                                    OtherPara = "{\"BookPropertyId\":"+ a.Id + "}",
                                    BookAuthorId=b.AuthorId,
                                    BookPublisherId=b.PublisherId
                                }) ;
                    var result = books.ToList();

                   
                    if (result != null)
                    {
                        return result;

                    }
                    else
                    {
                        return null;
                    }

                }
            }catch(Exception ex)
            {
                return null;
            }
        }

        internal static List<BookVMTile> GetBooksByPublisher(int publisherId, int pageId)
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    var preOrderBooks = from a in (from q in ctx.Books
                                                   where q.PublisherId== publisherId && !q.isDeleted && q.ItemType == (int)ItemType.Book
                                                   select q)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType=a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate=a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                            c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    return preOrderBooks.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<BookVMTile> GetBooksByCategories(List<int> categoryIds)
        {

            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    var selectedBooks = from bc in ctx.Book_Categories where !bc.isDeleted 
                                        join cid in categoryIds
                                         on bc.CategoryId equals cid
                                        join bk in ctx.Books 
                                             on bc.BookId equals bk.Id
                                             where !bk.isDeleted && bk.ItemType == (int)ItemType.Book
                                        select bk;

                    var preOrderBooks = from a in  (selectedBooks.Distinct()) 
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType = a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate=a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                            c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    return preOrderBooks.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<BookVMTile> GetBooksByAuthor(int authorId, int pageId)
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    List<int> bkIDlist = new List<int>();
                    bkIDlist = ctx.Book_Authors.Where(x=> x.AuthorId==authorId).Select(x=> x.BookId).ToList();
                    var books = ctx.Books.Where(q => ((q.AuthorId == authorId) || bkIDlist.Contains(q.Id)) && !q.isDeleted).ToList();
                    var preOrderBooks = from a in books
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType=a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate=a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                            c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    return preOrderBooks.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Book> GetAllBooks(bool withDeleted)
        {
            List<Book> results = new List<Book>();
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if (withDeleted)
                    {
                        results = ctx.Books.OrderBy(book => book.Title).ToList();
                    }
                    else
                    {
                        results = ctx.Books.Where(b=> !b.isDeleted).OrderBy(book => book.Title).ToList();
                    }
                    if (results != null)
                    {
                        foreach (var b in results)
                        {
                            var authors=ctx.Authors.Where(a => a.Id == b.AuthorId).FirstOrDefault();

                            b.AuthorName = authors != null ? authors.Name : "";
                        }
                    }
                    return results;
                }
               
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PageResults GetAllBooksAsPageResults(bool withDeleted, int page,int numberOfPages)
        {
            List<Book> results = new List<Book>();
            PageResults p = new PageResults();
            int numberOfItem = 0;
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if (withDeleted)
                    {
                        results = ctx.Books.OrderBy(book => book.Title).Skip(100 * (page - 1)).Take(100).ToList();
                        numberOfItem = numberOfPages == 0?ctx.Books.OrderBy(book => book.Title).Count():0;
                    }
                    else
                    {
                        results = ctx.Books.Where(b => !b.isDeleted).OrderBy(book => book.Title).Skip(100 * (page - 1)).Take(100).ToList();
                        numberOfItem = numberOfPages == 0?ctx.Books.Where(b => !b.isDeleted).OrderBy(book => book.Title).Count():0;
                    }
                    if (results != null)
                    {                       
                        p.CurrentPage = page;
                        p.NumberOfPages = numberOfPages==0?(int)Math.Ceiling(Convert.ToDouble(numberOfItem / 100)): numberOfPages;
                        p.Results = results;//!= null ? results.Skip(20 * (page - 1)).Take(20).ToList() : null;
                        if(p.Results != null)
                        {
                            var allAuthors = ctx.Authors;
                            foreach (var b in (List<Book>)p.Results)
                            {
                                var authors = allAuthors.Where(a => a.Id == b.AuthorId).FirstOrDefault();

                                b.AuthorName = authors != null ? authors.Name : "";
                            }
                        }
                        
                    }

                    return p;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static Book Put(Book book)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Book dbBook=  ctx.Books.Where(b=> b.Id==book.Id).FirstOrDefault();
                    dbBook.ISBN = book.ISBN;
                    dbBook.Title = book.Title;
                    dbBook.UpdatedBy = book.UpdatedBy;
                    dbBook.UpdatedDate = book.UpdatedDate;
                    dbBook.PublisherId = book.PublisherId;
                    dbBook.AuthorId = book.AuthorId;
                    dbBook.LocalTitle = book.LocalTitle;
                    dbBook.Ratings = book.Ratings;
                    dbBook.SaleType = book.SaleType;
                    dbBook.RelaseDate = book.RelaseDate;
                    dbBook.Description = book.Description;
                    dbBook.FriendlyName = book.FriendlyName;
                    dbBook.YoutubeUrl = book.YoutubeUrl;
                    dbBook.MaximumItemPerOrder = book.MaximumItemPerOrder;
                    dbBook.AvailableUntil = book.AvailableUntil;
                    ctx.SaveChanges();
                    if (BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                    {
                        SearchHandler.UpdateSearchIndex(book, ObjectTypes.Book);
                    }
                }
                return book;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Media GetBookFrontCover(int bookId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var bookProperty = ctx.BookProperties.Where(x => x.BookId == bookId).FirstOrDefault();
                    if(bookProperty != null)
                    {
                        return ctx.Medias.Where(x => x.Id == bookProperty.FrontCoverMediaId).FirstOrDefault();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<BookVMTile> GetPreOrderBooksForView()
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    int preOrderId = ctx.SaleStatus.Where(s => s.Title == "pre_order").FirstOrDefault().Id;
                    var preOrderBooks = from a in (from q in ctx.Books
                                                // where (q.SaleType == preOrderId && !q.isDeleted && q.ItemType == (int)ItemType.Book) select q)
                                                   where (q.SaleType == preOrderId && !q.isDeleted )
                                                   select q)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName=a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName=b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating =a.Ratings,
                                            SaleType=a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate= a.CreatedDate,
                                            ItemType=a.ItemType,
                                            Property =ctx.BookProperties.Where(x=> x.BookId==a.Id).ToList(),
                                            Categories= (from r in (from t in ctx.Book_Categories
                                                         where t.BookId == a.Id && !t.isDeleted select t)                                                         join
                                                             c in ctx.Categories on r.CategoryId equals c.Id
                                                         select c).ToList(),
                                            FrontCover= (from x in ( from s in ctx.BookProperties
                                                         where s.BookId == a.Id select s)
                                                         join
                                                         y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                         select y).ToList().FirstOrDefault()
                                        };
                    return preOrderBooks.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static List<BookVMTile> GetLatestBooksForView()
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    SaleStatus saleStatus = BusinessHandlerSaleStatus.GetSaleStatusByTitle("pre_order");
                    
                    var preOrderBooks = from a in (from q in ctx.Books
                                                   where !q.isDeleted && q.SaleType !=saleStatus.Id && q.ItemType == (int)ItemType.Book
                                                   orderby q.CreatedDate descending 
                                                   select q).Take(10)
                    join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType = a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate=a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)join
                                                            c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    return preOrderBooks.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static BookVMTile GetSearchedBookForView(int bookId)
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    var preOrderBooks = from a in (from q in ctx.Books
                                                  where q.Id== bookId && !q.isDeleted
                                                   select q)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            ISBN=a.ISBN,
                                            SaleType=a.SaleType,
                                            AuthorId=a.AuthorId,
                                            RelaseDate=a.RelaseDate,
                                            Description=a.Description,
                                            YoutubeUrl=a.YoutubeUrl,
                                            MaximumItemPerOrder=a.MaximumItemPerOrder,
                                            Url = a.FriendlyName,
                                            CreatedDate=a.CreatedDate,
                                            publisher =ctx.Publishers.Where(x=> x.Id==a.PublisherId).FirstOrDefault(),
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            ItemType=a.ItemType,
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                        c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).Distinct().ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault(),
                                            BackCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.BackCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    return preOrderBooks.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Book Get(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                  return  ctx.Books.Where(b=> b.Id==id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static bool ShowHide(int id, bool option)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Book book = ctx.Books.Where(x => x.Id == id).FirstOrDefault();
                    if (book != null)
                    {
                        book.isDeleted = option;
                        book.UpdatedDate = DateTime.UtcNow;
                        book.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        if (ctx.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal static bool ShowHideBookPack(int id, bool option)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ItemPack book = ctx.ItemPacks.Where(x => x.Id == id).FirstOrDefault();
                    if (book != null)
                    {
                        book.isDeleted = option;
                        book.UpdatedDate = DateTime.UtcNow;
                        book.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        if (ctx.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal static List<BookVMTile> GetAllBooksForView()
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    var preOrderBooks = from a in (from q in ctx.Books where !q.isDeleted && q.ItemType==(int)ItemType.Book
                                                   orderby q.CreatedDate descending
                                                   select q )//.Skip(page* itemForPage).Take(itemForPage)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType=a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate= a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                        c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    var returnList=preOrderBooks.Where(x => !x.isDeleted).ToList();
                    return returnList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static int GetTotalNumberOfBooks()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   return ctx.Books.Where(b => !b.isDeleted && b.ItemType == (int)ItemType.Book).Count();//.ToList();
                }
            }
            catch(Exception ex)
            {

            }
            return 0;
        }

        internal static List<BookVMTile> GetPageBooksForView(int page, int itemPerPage)
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    var selectedBooks = ctx.Books.Where(b => !b.isDeleted && b.ItemType == (int)ItemType.Book).OrderByDescending(x => x.CreatedDate).Skip(itemPerPage * (page - 1)).Take(itemPerPage).ToList();
                    var preOrderBooks = from a in selectedBooks
                                            //(from q in ctx.Books
                                            //       where !q.isDeleted
                                            //       orderby q.CreatedDate descending
                                            //       select q)//.Skip(page* itemForPage).Take(itemForPage)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType = a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate = a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                        c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    var returnList = preOrderBooks.Where(x => !x.isDeleted).ToList();
                    return returnList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static ItemPack AddItemPack(ItemPack model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.ItemPacks.Add(model);
                    if(ctx.SaveChanges() > 0){
                        return model;
                    }
                
                }
            }catch(Exception ex)
            {

            }
            return null;
        }

        internal static bool AddItemPack_Book(List<ItemPack_Item> list)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.ItemPack_Items.AddRange(list);
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }

                }
            }
            catch(Exception ex)
            {

            }
            return false;
        }
        internal static List<ItemPack> GetAllItemPack(bool withDeleted=false)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return withDeleted? ctx.ItemPacks.ToList(): ctx.ItemPacks.Where(x => !x.isDeleted).ToList();                 
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<BookVMTile> GetMultiAuthorBooks(int authorId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var books = ctx.Book_Authors.Where(y => y.AuthorId == authorId).Select(x => x.BookId);
                    var selectedbooks = ctx.Books.Where(p => books.All(p2 => p2 == p.Id));
                    var multiAuthorBooks = from a in (from q in selectedbooks
                                                   where (q.AuthorId == authorId
                                                   ) && !q.isDeleted
                                                   select q)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType = a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate = a.CreatedDate,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                            c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    return multiAuthorBooks.ToList();


                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool ChangePreOrderScheduller()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var ObjPreOrder= ctx.SaleStatus.Where(x => !x.isDeleted && x.Title == "pre_order").FirstOrDefault();
                    var Objin_2_weeks = ctx.SaleStatus.Where(x => !x.isDeleted && x.Title == "in_2_weeks").FirstOrDefault();
                    if (ObjPreOrder != null)
                    {
                        List<Book> books=ctx.Books.Where(x => !x.isDeleted && x.SaleType == ObjPreOrder.Id && x.RelaseDate <= DateTime.UtcNow).ToList();
                        if(books != null)
                        {
                            foreach(var b in books)
                            {
                                b.SaleType = Objin_2_weeks != null ? Objin_2_weeks.Id : b.SaleType;
                                b.UpdatedDate = DateTime.UtcNow;
                                b.UpdatedBy = "Sheduller";
                            }

                            ctx.SaveChanges();
                            return true;
                        }
                    }
                   
                }
                
            }catch(Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerBooks", MethodBase.GetCurrentMethod().Name);
            }
            return false;
        }

        internal static BookVMTile GetBookTileByBookId(int bookId)
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    var preOrderBooks = from a in (from q in ctx.Books
                                                   where q.Id == bookId
                                                   select q)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName = b.Name,
                                            LocalAuthorName = b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            ISBN = a.ISBN,
                                            SaleType = a.SaleType,
                                            AuthorId = a.AuthorId,
                                            RelaseDate = a.RelaseDate,
                                            Description = a.Description,
                                            YoutubeUrl = a.YoutubeUrl,
                                            Url = a.FriendlyName,
                                            CreatedDate = a.CreatedDate,
                                            MaximumItemPerOrder=a.MaximumItemPerOrder,
                                            publisher = ctx.Publishers.Where(x => x.Id == a.PublisherId).FirstOrDefault(),
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                        c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).Distinct().ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault(),
                                            BackCover = (from x in (from s in ctx.BookProperties
                                                                    where s.BookId == a.Id
                                                                    select s)
                                                         join
                                                         y in ctx.Medias on x.BackCoverMediaId equals y.Id
                                                         select y).ToList().FirstOrDefault()
                                        };
                    return preOrderBooks.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<BookVMTile> GetBookPacksForView()
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    SaleStatus saleStatus = BusinessHandlerSaleStatus.GetSaleStatusByTitle("pre_order");
                    var date = DateTime.Now.AddHours(5.5);
                    var BookPacks = from a in (from q in ctx.Books
                                                   where !q.isDeleted && q.SaleType != saleStatus.Id &&  q.ItemType == (int)ItemType.BookPack && (q.AvailableUntil>= date)
                                               orderby q.CreatedDate descending
                                                   select q)
                                        join b in ctx.Authors on a.AuthorId equals b.Id
                                        select new BookVMTile
                                        {
                                            Id = a.Id,
                                            BookName = a.Title,
                                            LocalBookName = a.LocalTitle,
                                            AuthorName ="", //b.Name,
                                            LocalAuthorName ="", //b.LocalName,
                                            isDeleted = b.isDeleted,
                                            Rating = a.Ratings,
                                            SaleType = a.SaleType,
                                            Url = a.FriendlyName,
                                            CreatedDate = a.CreatedDate,
                                            ItemType=a.ItemType,
                                            Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                            Categories = (from r in (from t in ctx.Book_Categories
                                                                     where t.BookId == a.Id && !t.isDeleted
                                                                     select t)
                                                          join
                                        c in ctx.Categories on r.CategoryId equals c.Id
                                                          select c).ToList(),
                                            FrontCover = (from x in (from s in ctx.BookProperties
                                                                     where s.BookId == a.Id
                                                                     select s)
                                                          join
                                                          y in ctx.Medias on x.FrontCoverMediaId equals y.Id
                                                          select y).ToList().FirstOrDefault()
                                        };
                    return BookPacks.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<int> GetInactivatedBookPacks()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    DateTime date = DateTime.UtcNow.AddHours(5.5);
                    return ctx.Books.Where(x=> x.ItemType==(int)ItemType.BookPack && !x.isDeleted && x.RelaseDate< date).Select(x=> x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool UpdateBookPackItem(int bookPackId, List<ItemPack_Item> newitems)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var items=ctx.ItemPack_Items.Where(x => x.ItemPackId == bookPackId && !x.isDeleted);
                    if(items !=null )
                    {
                        foreach (var item in items.ToList())
                        {
                            item.isDeleted = true;
                            item.DeletedDate = DateTime.UtcNow;
                        }
                    }                       
                    ctx.ItemPack_Items.AddRange(newitems);
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }
    }
}