using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        // GET: Admin/Book
        public ActionResult Index()
        {
            if(Request.QueryString["hidden"] != null && Request.QueryString["hidden"] == "true")
            {
                ViewBag.Books = BusinessHandlerBook.GetAllBooks(true);
            }
            else
            {
                ViewBag.Books = BusinessHandlerBook.GetAllBooks(false);
            }
            
            return View();
        }
        
        public ActionResult AddBook()
        {
            List<string> Obj_book_property_types = new List<string>();
            Configuration book_property_types = BusinessHandlerConfigurations.GetConfigByKey("BOOK_PROPERTY_TYPES");
            if (book_property_types != null && !string.IsNullOrEmpty(book_property_types.Value))
            {
                Obj_book_property_types = JsonConvert.DeserializeObject<List<string>>(book_property_types.Value);
            }
            ViewBag.Authors = BusinessHandlerAuthor.GetAuthors();
            ViewBag.Publishers = BusinessHandlerPublisher.GetPublishers();
            ViewBag.Categories = BusinessHandlerCategory.GetCategories();
            ViewBag.Languages = BusinessHandlerLanguage.Get();
            ViewBag.SaleStatus = BusinessHandlerSaleStatus.GetAllActiveSaleStatus();
            var books = BusinessHandlerBook.GetAllBooksWithPropertyAsNewOneForBookPack(); //BusinessHandlerBook.GetAllBooks(true).Select(x=> new DataObjVM { Id=x.Id,Name=x.Title,ObjType=0}).ToList();    
            ViewBag.list = books;
            ViewBag.book_property_types = Obj_book_property_types;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddBook(BookVM bookvm)
        {
            if(bookvm != null)
            {

                Book book = new Book();
                book.Title = bookvm.Title;
                book.ISBN = bookvm.ISBN; ;
                book.Description =bookvm.Description;
                book.AuthorId =bookvm.AuthorId;
                book.PublisherId = bookvm.ItemType==(int)ItemType.Book?bookvm.PublisherId:0;
                book.LocalTitle = bookvm.LocalTitle;
                book.Ratings = bookvm.Ratings;
                book.SaleType = bookvm.SaleType;
                book.RelaseDate = bookvm.PreReleaseEndDate;//) ? Convert.ToDateTime(bookvm.PreReleaseEndDate) : DateTime.UtcNow;
                book.YoutubeUrl = bookvm.YoutubeUrl;
                book.MaximumItemPerOrder = bookvm.MaximumItemPerOrder == 0 ? 10000 : bookvm.MaximumItemPerOrder;
                book.ItemType = bookvm.ItemType;
                book.AvailableUntil = bookvm.AvailableUntil;
                book =BusinessHandlerBook.Add(book);
          
                int FrontCoverMediaId=0, BackCoverMediaId=0, FreeReadPDFMediaId=0;
                if (book != null)
                {
                    var frontCover = Request.Files["FrontCoverMedia"];
                    var backCover = Request.Files["BackCoverMedia"];
                    var readPDF = Request.Files["FreeReadPDFMedia"];
                    if (frontCover.ContentLength > 0)
                    {
                        Media frontCoverPic = BusinessHandlerMedia.CreateNewMediaEntry(frontCover, MediaCategory.BookFrontCover);
                        if (frontCoverPic != null)
                        {
                            FrontCoverMediaId = frontCoverPic.Id;
                        }
                    }
                    if (backCover.ContentLength > 0)
                    {
                        Media backCoverPic = BusinessHandlerMedia.CreateNewMediaEntry(backCover, MediaCategory.BookBackCover);
                        if (backCoverPic != null)
                        {
                            BackCoverMediaId = backCoverPic.Id;
                        }
                    }
                    if (readPDF.ContentLength > 0)
                    {
                        Media PDF = BusinessHandlerMedia.CreateNewMediaEntry(readPDF, MediaCategory.ReadablePDF);
                        if (PDF != null)
                        {
                            FreeReadPDFMediaId = PDF.Id;
                        }
                    }
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        List<ItemMedia> mediaList = new List<ItemMedia>();
                        for (int i = 0; i < Request.Files.Count; i++)
                        {

                            HttpPostedFileBase file = Request.Files[i];

                            if (((file.FileName != frontCover.FileName) && (file.ContentLength != frontCover.ContentLength)) && ((file.FileName != backCover.FileName) && (file.ContentLength != backCover.ContentLength)) && ((file.FileName != readPDF.FileName) && (file.ContentLength != readPDF.ContentLength)))
                            {
                                Media media = BusinessHandlerMedia.CreateNewMediaEntry(file, MediaCategory.CoverImage);
                                ItemMedia itemMedia = new ItemMedia { MediaId = media.Id, ObjectId = book.Id, ObjectType = (int)ObjectTypes.Book};
                                mediaList.Add(itemMedia);

                            }

                        }
                        if (mediaList != null)
                        {
                            BusinessHandlerBook.AddItemPack(mediaList);
                        }

                    }
                    if(bookvm.ItemType== (int)ItemType.Book)
                    {
                        bool isPropertyAdded = false;
                        if (bookvm.BookProperties != null && bookvm.BookProperties.Count > 0)
                        {
                            foreach (var bp in bookvm.BookProperties)
                            {
                                if (bp.Price > 0 && bp.NumberOfPages != 0)
                                {
                                    BookProperties bookProperties = new BookProperties();
                                    bookProperties.BookId = book.Id;
                                    bookProperties.NumberOfPages = bp.NumberOfPages;
                                    bookProperties.NumberOfCopies = bp.NumberOfCopies;
                                    bookProperties.LanguageId = bookvm.LanguageId;
                                    bookProperties.Price = bp.Price;
                                    bookProperties.WeightByGrams = bp.WeightByGrams;
                                    bookProperties.FreeReadPDFMediaId = FreeReadPDFMediaId;
                                    bookProperties.BackCoverMediaId = BackCoverMediaId;
                                    bookProperties.FrontCoverMediaId = FrontCoverMediaId;
                                    bookProperties.Title = bp.Title;
                                    bookProperties.Description = bp.Description;
                                    BusinessHandlerBookProperties.Add(bookProperties);
                                    isPropertyAdded = true;
                                    //if (bp.DiscountValue > 0)
                                    //{
                                    Promotion promotion = new Promotion
                                    {
                                        PromotionTitle = "Promotion for " + bookvm.Title + " - " + book.Id,
                                        PromotionDescription = "",
                                        PromotionTypesFor = (int)PromotionTypesFor.Book,
                                        PromotionMethods = bp.PromotionMethods,
                                        ObjectType = (int)ObjectTypes.Book,
                                        ObjectId = book.Id,
                                        DiscountValue = bp.DiscountValue,
                                        OtherParameters = "{BookPropertyId:" + bookProperties.Id + "}",
                                        EndDate = DateTime.Today.AddYears(5),
                                        StartDate = DateTime.Today.AddDays(-1)
                                    };
                                    BusinessHandlerPromotion.Add(promotion);
                                    //}
                                }
                            }
                        }
                        if (!isPropertyAdded)
                        {
                            BusinessHandlerBookProperties.AddDemoProperty(book.Id);
                        }
                    }else if(bookvm.ItemType == (int)ItemType.BookPack)
                    {
                        decimal weight = 0;
                        decimal price = 0;
                        if (!String.IsNullOrEmpty(bookvm.SelectedBooks))
                        {
                            List<ItemPack_Item> itemPack_ItemList = new List<ItemPack_Item>();
                            string[] items = bookvm.SelectedBooks.Split(',');
                            foreach (string s in items)
                            {
                                string[] dictionary = s.Split('-');
                                int bookId = Convert.ToInt32(dictionary[0]);
                                int propertyId = Convert.ToInt32(dictionary[1]);
                                var Obj = new ItemPack_Item { ItemId = bookId, ItemPackId = book.Id, ItemPropertyId = propertyId, NumberOfItems = 1,isDeleted=false };
                                itemPack_ItemList.Add(Obj);
                                BookProperties b = BusinessHandlerBookProperties.GetById(propertyId);
                                BusinessHandlerBook.UpdateBookStockAddBookPackItem(book.Id, propertyId, bookvm.NumberOfCopies);
                                weight = weight + (b != null ? b.WeightByGrams : 0);
                                price = price + (b != null ? b.Price : 0);
                            }
                            if (itemPack_ItemList.Count > 0)
                            {
                                BusinessHandlerBook.AddItemPack_Book(itemPack_ItemList);
                            }

                        }
                        BookProperties bookProperties = new BookProperties();
                        bookProperties.BookId = book.Id;
                        bookProperties.NumberOfPages = 0;
                        bookProperties.NumberOfCopies = bookvm.NumberOfCopies;
                        bookProperties.LanguageId = bookvm.LanguageId;
                        bookProperties.Price = price;//bookvm.ItemPrice;
                        bookProperties.WeightByGrams = weight;
                        bookProperties.FreeReadPDFMediaId = FreeReadPDFMediaId;
                        bookProperties.BackCoverMediaId = BackCoverMediaId;
                        bookProperties.FrontCoverMediaId = FrontCoverMediaId;
                        bookProperties.Title = bookvm.Title;
                        bookProperties.Description = bookvm.Description;
                        BusinessHandlerBookProperties.Add(bookProperties);
                        Promotion promotion = new Promotion
                        {
                            PromotionTitle = "Promotion for " + bookvm.Title + " - " + book.Id,
                            PromotionDescription = "",
                            PromotionTypesFor = (int)PromotionTypesFor.Book,
                            PromotionMethods = bookvm.BookPackDiscountType,
                            ObjectType = (int)ObjectTypes.Book,
                            ObjectId = book.Id,
                            DiscountValue = bookvm.BookPackDiscountValue,
                            OtherParameters = "{BookPropertyId:" + bookProperties.Id + "}",
                            EndDate = DateTime.Today.AddYears(5),
                            StartDate = DateTime.Today.AddDays(-1)
                        };
                        BusinessHandlerPromotion.Add(promotion);
                    }
                   
                   
                    //BusinessHandlerBookCategory.DeletebyBookId(book.Id);
                    if(!string.IsNullOrEmpty(bookvm.Categories))
                    {
                        var catIds = bookvm.Categories.Split(',');
                        foreach(var id in catIds)
                        {
                            Book_Category book_Category = new Book_Category {BookId= book.Id, CategoryId=int.Parse(id) };
                            BusinessHandlerBookCategory.SaveCategory(book_Category);
                        }
                    }
                    if (bookvm.OtherAthors !=null && bookvm.OtherAthors.Length > 0)
                    {
                        var  other_authors = bookvm.OtherAthors.Distinct();
                        foreach (var a in other_authors)
                        {
                            if(a != bookvm.AuthorId)
                            {
                                BusinessHandlerAuthor.AddMultipleAuthor(book.Id,a);
                            }
                        }
                    }                                       
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult edit(int id)
        {
           Book book= BusinessHandlerBook.Get(id);
            BookVM bookvm = new BookVM();
            if (book != null)
            {
                List<BookProperties> bookProperties = BusinessHandlerBookProperties.GetByBookId(book.Id);
                List<Book_Category> bookCategories = BusinessHandlerBookCategory.GetByBookId(book.Id);
                bookvm.Id =book.Id;
                bookvm.Title =book.Title;
                bookvm.ISBN =book.ISBN;
                bookvm.Description =book.Description;
                bookvm.AuthorId =book.AuthorId;
                bookvm.PublisherId = book.PublisherId;
                bookvm.LocalTitle = book.LocalTitle;
                bookvm.Ratings = book.Ratings;
                bookvm.SaleType = book.SaleType;
                bookvm.MaximumItemPerOrder = book.MaximumItemPerOrder;
                bookvm.PreReleaseEndDate = book.RelaseDate;// !=null?book.PreReleaseEndDate.ToString("yyyy-MM-dd"):book.CreatedDate.ToString("yyyy-MM-dd");
                bookvm.Categories= string.Join(",", bookCategories.Select(x=> x.CategoryId));
                bookvm.OtherAthors = BusinessHandlerAuthor.GetmultipleAuthors(book.Id).Select(x=> x.Id).ToArray();
                bookvm.YoutubeUrl = book.YoutubeUrl;
                bookvm.ItemType = book.ItemType;
                bookvm.AvailableUntil = book.AvailableUntil;

                List<BookPropertyVM> bplist = new List<BookPropertyVM>();
                foreach (var b in bookProperties)
                {
                    Promotion promotion = BusinessHandlerPromotion.Get(book.Id, b.Id);
                    BookPropertyVM bp = new BookPropertyVM();
                    bp.NumberOfCopies = b.NumberOfCopies;
                    bp.NumberOfPages = b.NumberOfPages;
                    bp.Price = b.Price;
                    bp.WeightByGrams = b.WeightByGrams;
                    bp.Id = b.Id;
                    if(promotion != null)
                    {
                        if (book.ItemType == (int)ItemType.Book)
                        {
                            bp.DiscountValue = promotion.DiscountValue;
                            bp.PromotionMethods = promotion.PromotionMethods;
                        }else if(book.ItemType== (int)ItemType.BookPack)
                        {
                            bookvm.BookPackDiscountValue = promotion.DiscountValue;
                            bookvm.BookPackDiscountType =promotion.PromotionMethods;
                        }
                        
                    }
                    if (bookvm.FrontCover == null)
                    {
                        bookvm.FrontCover = BusinessHandlerMedia.Get(b.FrontCoverMediaId);
                    }
                    if (bookvm.BackCover == null)
                    {
                        bookvm.BackCover = BusinessHandlerMedia.Get(b.BackCoverMediaId);
                    }
                    if (bookvm.ReadPDF == null)
                    {
                        bookvm.ReadPDF = BusinessHandlerMedia.Get(b.FreeReadPDFMediaId);
                    }
                    if (bookvm.LanguageId == 0)
                    {
                        bookvm.LanguageId = b.LanguageId;
                    }
                    bp.Title = b.Title;
                    bp.Description = b.Description;
                    bplist.Add(bp);
                  

                }
                var books = BusinessHandlerBook.GetAllBooksWithPropertyAsNewOne(); //BusinessHandlerBook.GetAllBooks(true).Select(x=> new DataObjVM { Id=x.Id,Name=x.Title,ObjType=0}).ToList();    
                ViewBag.list = books;

                bookvm.BookProperties = bplist;
            }
            List<string> Obj_book_property_types = new List<string>();
            Configuration book_property_types = BusinessHandlerConfigurations.GetConfigByKey("BOOK_PROPERTY_TYPES");
            if (book_property_types != null && !string.IsNullOrEmpty(book_property_types.Value))
            {
                Obj_book_property_types = JsonConvert.DeserializeObject<List<string>>(book_property_types.Value);
            }

            ViewBag.Authors = BusinessHandlerAuthor.GetAuthors();
            ViewBag.Categories = BusinessHandlerCategory.GetCategories();
            ViewBag.Publishers= BusinessHandlerPublisher.GetPublishers();
            ViewBag.Languages = BusinessHandlerLanguage.Get();
            ViewBag.SaleStatus = BusinessHandlerSaleStatus.GetAllActiveSaleStatus();
            ViewBag.book_property_types = Obj_book_property_types;
            ViewBag.selectedBooks = BusinessHandlerBook.GetBookPackItemByBookPackId(id);

            return View(bookvm);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(BookVM bookVM)
        {
            if(bookVM != null)
            {
                Book book = BusinessHandlerBook.Get(bookVM.Id);
                List<BookProperties> bookProperties = BusinessHandlerBookProperties.GetByBookId(bookVM.Id);
               
                if (book != null)
                {
                    book.Id= bookVM.Id;
                    book.Title= bookVM.Title;
                    book.ISBN= bookVM.ISBN;
                    book.Description= bookVM.Description;
                    book.PublisherId = bookVM.ItemType == (int)ItemType.Book ? bookVM.PublisherId : 0;
                    book.AuthorId= bookVM.AuthorId;
                    book.LocalTitle = bookVM.LocalTitle;
                    book.Ratings = bookVM.Ratings;
                    book.SaleType = bookVM.SaleType;
                    book.RelaseDate = bookVM.PreReleaseEndDate;//) ? Convert.ToDateTime(bookVM.PreReleaseEndDate) : DateTime.Today.AddDays(-1);
                    book.YoutubeUrl = bookVM.YoutubeUrl;
                    book.MaximumItemPerOrder = bookVM.MaximumItemPerOrder;
                    book.AvailableUntil = bookVM.AvailableUntil;
                    BusinessHandlerBook.Put(book);
                    int FrontCoverMediaId = 0, BackCoverMediaId = 0, FreeReadPDFMediaId = 0;
                    if (book != null)
                    {
                        var frontCover = Request.Files["FrontCoverMedia"];
                        var backCover = Request.Files["BackCoverMedia"];
                        var readPDF = Request.Files["FreeReadPDFMedia"];

                        if (frontCover.ContentLength > 0)
                        {
                            Media frontCoverPic = BusinessHandlerMedia.CreateNewMediaEntry(frontCover, MediaCategory.BookFrontCover);
                            if (frontCoverPic != null)
                            {
                                FrontCoverMediaId = frontCoverPic.Id;
                            }
                        }
                        if (backCover.ContentLength > 0)
                        {
                            Media backCoverPic = BusinessHandlerMedia.CreateNewMediaEntry(backCover, MediaCategory.BookBackCover);
                            if (backCoverPic != null)
                            {
                                BackCoverMediaId = backCoverPic.Id;
                            }
                        }
                        if (readPDF.ContentLength > 0)
                        {
                            Media PDF = BusinessHandlerMedia.CreateNewMediaEntry(readPDF, MediaCategory.ReadablePDF);
                            if (PDF != null)
                            {
                                FreeReadPDFMediaId = PDF.Id;
                            }
                        }
                        if (Request.Files != null && Request.Files.Count > 0)
                        {
                            List<ItemMedia> mediaList = new List<ItemMedia>();
                            for (int i = 0; i < Request.Files.Count; i++)
                            {

                                HttpPostedFileBase file = Request.Files[i];

                                if (((file.FileName != frontCover.FileName) && (file.ContentLength != frontCover.ContentLength)) && ((file.FileName != backCover.FileName) && (file.ContentLength != backCover.ContentLength)) && ((file.FileName != readPDF.FileName) && (file.ContentLength != readPDF.ContentLength)))
                                {
                                    Media media = BusinessHandlerMedia.CreateNewMediaEntry(file, MediaCategory.CoverImage);
                                    ItemMedia itemMedia = new ItemMedia { MediaId = media.Id, ObjectId = book.Id, ObjectType = (int)ObjectTypes.Book };
                                    mediaList.Add(itemMedia);

                                }

                            }
                            if (mediaList != null)
                            {
                                BusinessHandlerBook.AddItemPack(mediaList);
                            }

                        }
                        bool isPropertyAdded = false;
                        if (bookVM.ItemType == (int)ItemType.Book)
                        {
                            foreach (var bp in bookVM.BookProperties)
                            {

                                BookProperties bookPropertyDb = BusinessHandlerBookProperties.GetById(bp.Id);
                                if (bookPropertyDb != null)
                                {
                                    bookPropertyDb.BookId = book.Id;
                                    bookPropertyDb.NumberOfPages = bp.NumberOfPages;
                                    bookPropertyDb.NumberOfCopies = bp.NumberOfCopies;
                                    bookPropertyDb.LanguageId = bookVM.LanguageId;
                                    bookPropertyDb.Price = bp.Price;
                                    bookPropertyDb.WeightByGrams = bp.WeightByGrams;
                                    bookPropertyDb.FreeReadPDFMediaId = FreeReadPDFMediaId != 0 ? FreeReadPDFMediaId : bookPropertyDb.FreeReadPDFMediaId;
                                    bookPropertyDb.BackCoverMediaId = BackCoverMediaId != 0 ? BackCoverMediaId : bookPropertyDb.BackCoverMediaId;
                                    bookPropertyDb.FrontCoverMediaId = FrontCoverMediaId != 0 ? FrontCoverMediaId : bookPropertyDb.FrontCoverMediaId;
                                    bookPropertyDb.Title = bp.Title;
                                    bookPropertyDb.Description = bp.Description;
                                    BusinessHandlerBookProperties.Put(bookPropertyDb);
                                    isPropertyAdded = true;
                                    //if (bp.DiscountValue > 0)
                                    //{
                                    Promotion promotion = BusinessHandlerPromotion.Get(book.Id, bp.Id);
                                    if (promotion == null)
                                    {
                                        promotion = new Promotion
                                        {
                                            PromotionTitle = "Promotion for " + book.Title + " - " + book.Id,
                                            PromotionDescription = "",
                                            PromotionTypesFor = (int)PromotionTypesFor.Book,
                                            PromotionMethods = bp.PromotionMethods,
                                            ObjectType = (int)ObjectTypes.Book,
                                            ObjectId = book.Id,
                                            DiscountValue = bp.DiscountValue,
                                            OtherParameters = "{BookPropertyId:" + bookPropertyDb.Id + "}",
                                            EndDate = DateTime.Today.AddYears(5),
                                            StartDate = DateTime.Today.AddDays(-1)
                                        };
                                        BusinessHandlerPromotion.Add(promotion);
                                    }
                                    else
                                    {
                                        //Promotion promotion = new Promotion
                                        //{
                                        promotion.PromotionTitle = "Promotion for " + book.Title + " - " + book.Id;
                                        promotion.PromotionDescription = "";
                                        promotion.PromotionTypesFor = (int)PromotionTypesFor.Book;
                                        promotion.PromotionMethods = bp.PromotionMethods;
                                        promotion.ObjectType = (int)ObjectTypes.Book;
                                        promotion.ObjectId = book.Id;
                                        promotion.DiscountValue = bp.DiscountValue;
                                        promotion.OtherParameters = "{BookPropertyId:" + bookPropertyDb.Id + "}";
                                        bookPropertyDb.FreeReadPDFMediaId = FreeReadPDFMediaId != 0 ? FreeReadPDFMediaId : bookPropertyDb.FreeReadPDFMediaId;
                                        bookPropertyDb.BackCoverMediaId = BackCoverMediaId != 0 ? BackCoverMediaId : bookPropertyDb.BackCoverMediaId;
                                        bookPropertyDb.FrontCoverMediaId = FrontCoverMediaId != 0 ? FrontCoverMediaId : bookPropertyDb.FrontCoverMediaId;
                                        promotion.EndDate = DateTime.Today.AddYears(5);
                                        promotion.StartDate = DateTime.Today.AddDays(-1);
                                        BusinessHandlerPromotion.Update(promotion);
                                        //};
                                    }

                                    //}                                                                      

                                }
                                else
                                {
                                    if (bp.Price > 0 && bp.NumberOfPages != 0)
                                    {
                                        BookProperties bookPropertyNew = new BookProperties();
                                        bookPropertyNew.BookId = book.Id;
                                        bookPropertyNew.NumberOfPages = bp.NumberOfPages;
                                        bookPropertyNew.NumberOfCopies = bp.NumberOfCopies;
                                        bookPropertyNew.LanguageId = bookVM.LanguageId;
                                        bookPropertyNew.Price = bp.Price;
                                        bookPropertyNew.WeightByGrams = bp.WeightByGrams;
                                        bookPropertyNew.FreeReadPDFMediaId = FreeReadPDFMediaId;
                                        bookPropertyNew.BackCoverMediaId = BackCoverMediaId;
                                        bookPropertyNew.FrontCoverMediaId = FrontCoverMediaId;
                                        bookPropertyNew.Title = bp.Title;
                                        bookPropertyNew.Description = bp.Description;
                                        BusinessHandlerBookProperties.Add(bookPropertyNew);
                                        if (bp.DiscountValue > 0)
                                        {
                                            // BusinessHandlerPromotion.Get(book.Id, bookPropertyNew.Id);
                                            Promotion promotion = new Promotion
                                            {
                                                PromotionTitle = "Promotion for " + book.Title + " - " + book.Id,
                                                PromotionDescription = "",
                                                PromotionTypesFor = (int)PromotionTypesFor.Book,
                                                PromotionMethods = bp.PromotionMethods,
                                                ObjectType = (int)ObjectTypes.Book,
                                                ObjectId = book.Id,
                                                DiscountValue = bp.DiscountValue,
                                                OtherParameters = "{BookPropertyId:" + bookPropertyNew.Id + "}",
                                                EndDate = DateTime.Today.AddYears(5),
                                                StartDate = DateTime.Today.AddDays(-1)
                                            };
                                            BusinessHandlerPromotion.Add(promotion);
                                        }
                                    }


                                }

                            }
                        }
                        else
                        {
                            decimal weight = 0;
                            decimal price = 0;
                            if (!String.IsNullOrEmpty(bookVM.SelectedBooks))
                            {
                                List<ItemPack_Item> itemPack_ItemList = new List<ItemPack_Item>();
                                string[] items = bookVM.SelectedBooks.Split(',');
                                foreach (string s in items)
                                {
                                    string[] dictionary = s.Split('-');
                                    int bookId = Convert.ToInt32(dictionary[0]);
                                    int propertyId = Convert.ToInt32(dictionary[1]);
                                    var Obj = new ItemPack_Item { ItemId = bookId, ItemPackId = book.Id, ItemPropertyId = propertyId, NumberOfItems = 1, isDeleted = false };
                                    itemPack_ItemList.Add(Obj);
                                    BookProperties b = BusinessHandlerBookProperties.GetById(propertyId);
                                    BusinessHandlerBook.UpdateBookStockAddBookPackItem(book.Id, propertyId, bookVM.NumberOfCopies);
                                    weight = weight + (b != null ? b.WeightByGrams : 0);
                                    price = price+ (b != null ? b.Price : 0);
                                }
                                if (itemPack_ItemList.Count > 0)
                                {
                                    BusinessHandlerBook.UpdateBookPackItem(book.Id, itemPack_ItemList);
                                }

                            }
                            BookProperties bookPropertyDb = BusinessHandlerBookProperties.GetById(bookVM.bookPack_bookPropertyId);
                            bookPropertyDb.BookId = book.Id;
                            bookPropertyDb.NumberOfPages = 0;
                            bookPropertyDb.NumberOfCopies = bookVM.NumberOfCopies;
                            bookPropertyDb.LanguageId = bookVM.LanguageId;
                            bookPropertyDb.Price = price;//bookVM.ItemPrice;
                            bookPropertyDb.WeightByGrams = weight;
                            bookPropertyDb.FrontCoverMediaId = FrontCoverMediaId != 0 ? FrontCoverMediaId : bookPropertyDb.FrontCoverMediaId;

                            bookPropertyDb.FreeReadPDFMediaId = FreeReadPDFMediaId;
                            bookPropertyDb.BackCoverMediaId = BackCoverMediaId;
                            bookPropertyDb.FrontCoverMediaId = FrontCoverMediaId;
                            bookPropertyDb.Title = bookVM.Title;
                            bookPropertyDb.Description = bookVM.Description;
                            BusinessHandlerBookProperties.Put(bookPropertyDb);
                            Promotion promotion = BusinessHandlerPromotion.Get(book.Id, bookVM.bookPack_bookPropertyId);
                            if (promotion == null)
                            {
                                promotion = new Promotion
                                {
                                    PromotionTitle = "Promotion for " + book.Title + " - " + book.Id,
                                    PromotionDescription = "",
                                    PromotionTypesFor = (int)PromotionTypesFor.Book,
                                    PromotionMethods = bookVM.BookPackDiscountType,
                                    ObjectType = (int)ObjectTypes.Book,
                                    ObjectId = book.Id,
                                    DiscountValue = bookVM.BookPackDiscountValue,
                                    OtherParameters = "{BookPropertyId:" + bookPropertyDb.Id + "}",
                                    EndDate = DateTime.Today.AddYears(5),
                                    StartDate = DateTime.Today.AddDays(-1)
                                };
                                BusinessHandlerPromotion.Add(promotion);
                            }
                            else
                            {
                                //Promotion promotion = new Promotion
                                //{
                                promotion.PromotionTitle = "Promotion for " + book.Title + " - " + book.Id;
                                promotion.PromotionDescription = "";
                                promotion.PromotionTypesFor = (int)PromotionTypesFor.Book;
                                promotion.PromotionMethods = bookVM.BookPackDiscountType;
                                promotion.ObjectType = (int)ObjectTypes.Book;
                                promotion.ObjectId = book.Id;
                                promotion.DiscountValue = bookVM.BookPackDiscountValue;
                                promotion.OtherParameters = "{BookPropertyId:" + bookPropertyDb.Id + "}";
                                promotion.EndDate = DateTime.Today.AddYears(5);
                                promotion.StartDate = DateTime.Today.AddDays(-1);
                                BusinessHandlerPromotion.Update(promotion);
                                //};
                            }
                        }
                           
                        if (!string.IsNullOrEmpty(bookVM.Categories))
                        {
                            BusinessHandlerBookCategory.RemoveExsistngMap(book.Id);
                            var catIds = bookVM.Categories.Split(',');
                            foreach (var id in catIds)
                            {
                                Book_Category book_Category = new Book_Category { BookId = book.Id, CategoryId = int.Parse(id) };
                                BusinessHandlerBookCategory.SaveCategory(book_Category);
                            }
                        }
                        if (bookVM.OtherAthors != null && bookVM.OtherAthors.Length > 0)
                        {
                            var other_authors = bookVM.OtherAthors.Distinct();
                            foreach (var a in other_authors)
                            {
                                if (a != bookVM.AuthorId)
                                {
                                    BusinessHandlerAuthor.AddMultipleAuthor(bookVM.Id, a);
                                }
                            }
                        }

                    }
                }

            }
            return RedirectToAction("Index");
        }
        public ActionResult HideItem(int id)
        {
            BusinessHandlerBook.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult HideBookPack(int id)
        {
            BusinessHandlerBook.HideBookPack(id);
            return RedirectToAction("BookPacks");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerBook.Show(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowBookPack(int id)
        {
            BusinessHandlerBook.ShowBookPack(id);
            return RedirectToAction("BookPacks");
        }
        

        public ActionResult AddMultipleAuthors()
        {
            ViewBag.Authors = BusinessHandlerAuthor.GetAuthors();
            return View();
        }
        public ActionResult BookPacks()
        {

            ViewBag.ItemPacks = (!string.IsNullOrEmpty(Request.QueryString["hidden"]) && Request.QueryString["hidden"] == "true") ? BusinessHandlerBook.GetAllItemPack(true) : BusinessHandlerBook.GetAllItemPack();
            return View();
        }
        public ActionResult AddBookPack()
        {
            var books = BusinessHandlerBook.GetAllBooksWithPropertyAsNewOne(); //BusinessHandlerBook.GetAllBooks(true).Select(x=> new DataObjVM { Id=x.Id,Name=x.Title,ObjType=0}).ToList();    
            ViewBag.list = books;
            return View();
        }
        [HttpPost]
        public ActionResult AddBookPack(ItemPack model)
        {                     
            var Images = Request.Files["image_files"];
            var frontCover = Request.Files["FrontCoverMedia"];
            
            if (frontCover.ContentLength > 0)
            {
                Media frontCoverPic = BusinessHandlerMedia.CreateNewMediaEntry(frontCover, MediaCategory.BookFrontCover);
                if (frontCoverPic != null)
                {
                    model.FrontCoverMediaId = frontCoverPic.Id;
                }
            }
            model= BusinessHandlerBook.AddItemPack(model);
            if(Request.Files !=null && Request.Files.Count > 0)
            {
                List<ItemMedia> mediaList = new List<ItemMedia>();
                for (int i= 0; i < Request.Files.Count; i++){
                    
                    HttpPostedFileBase file = Request.Files[i];
                    
                    if ((file.FileName != frontCover.FileName) && (file.ContentLength !=frontCover.ContentLength))
                    {
                        Media media = BusinessHandlerMedia.CreateNewMediaEntry(file, MediaCategory.CoverImage);
                        ItemMedia itemMedia = new ItemMedia { MediaId = media.Id, ObjectId = model.Id, ObjectType = (int)ObjectTypes.ItemPack };
                        mediaList.Add(itemMedia);
                 
                    }
                       
                }
                if(mediaList != null)
                {
                    BusinessHandlerBook.AddItemPack(mediaList);
                }
           
            }
            if (!String.IsNullOrEmpty(model.SelectedBooks)){
                List<ItemPack_Item> itemPack_ItemList = new List<ItemPack_Item>();
                string[] items = model.SelectedBooks.Split(',');
                foreach(string s in items)
                {
                   string[] dictionary= s.Split('-');
                    int bookId = Convert.ToInt32 (dictionary[0]);
                    int propertyId = Convert.ToInt32(dictionary[1]);
                    var Obj = new ItemPack_Item { ItemId = bookId, ItemPackId = model.Id, ItemPropertyId = propertyId,NumberOfItems=(1*model.NumberOfPacks) };
                    itemPack_ItemList.Add(Obj);
                    //BusinessHandlerBook.AddBookToBookPack(bookId, propertyId, model.Id, (1 * model.NumberOfPacks));
                }
                if (itemPack_ItemList.Count > 0)
                {
                    BusinessHandlerBook.AddItemPack_Book(itemPack_ItemList);
                }

            }

            return RedirectToAction("BookPacks");
        }

        public ActionResult EditBookPack(int id)
        {
            ViewBag.model = BusinessHandlerBook.GetItemPack(id);
            ViewBag.selectedBooks = BusinessHandlerBook.GetBookForPack(id);
            ViewBag.multipleImages = BusinessHandlerBook.GetOtherMedia(id,ObjectTypes.ItemPack);
            var books = BusinessHandlerBook.GetAllBooksWithPropertyAsNewOne(); //BusinessHandlerBook.GetAllBooks(true).Select(x=> new DataObjVM { Id=x.Id,Name=x.Title,ObjType=0}).ToList();    
            ViewBag.list = books;
            return View();
        }
        [HttpPost]
        public ActionResult EditBookPack(ItemPack model)
        {
            var Images = Request.Files["image_files"];
            var frontCover = Request.Files["FrontCoverMedia"];

            if (frontCover.ContentLength > 0)
            {
                Media frontCoverPic = BusinessHandlerMedia.CreateNewMediaEntry(frontCover, MediaCategory.BookFrontCover);
                if (frontCoverPic != null)
                {
                    model.FrontCoverMediaId = frontCoverPic.Id;
                }
            }
            model = BusinessHandlerBook.UpdateItemPack(model);
            if (Request.Files != null && Request.Files.Count > 0)
            {
                List<ItemMedia> mediaList = new List<ItemMedia>();
                for (int i = 0; i < Request.Files.Count; i++)
                {

                    HttpPostedFileBase file = Request.Files[i];

                    if ((file.FileName != frontCover.FileName) && (file.ContentLength != frontCover.ContentLength))
                    {
                        Media media = BusinessHandlerMedia.CreateNewMediaEntry(file, MediaCategory.CoverImage);
                        ItemMedia itemMedia = new ItemMedia { MediaId = media.Id, ObjectId = model.Id, ObjectType = (int)ObjectTypes.ItemPack };
                        mediaList.Add(itemMedia);

                    }

                }
                if (mediaList != null)
                {
                    BusinessHandlerBook.AddItemPack(mediaList);
                }

            }
            if (!String.IsNullOrEmpty(model.SelectedBooks))
            {
                List<ItemPack_Item> itemPack_ItemList = new List<ItemPack_Item>();
                string[] items = model.SelectedBooks.Split(',');
                foreach (string s in items)
                {
                    string[] dictionary = s.Split('-');
                    int bookId = Convert.ToInt32(dictionary[0]);
                    int propertyId = Convert.ToInt32(dictionary[1]);
                    var Obj = new ItemPack_Item { ItemId = bookId, ItemPackId = model.Id, ItemPropertyId = propertyId, NumberOfItems = (1 * model.NumberOfPacks) };
                    itemPack_ItemList.Add(Obj);
                    //BusinessHandlerBook.AddBookToBookPack(bookId, propertyId, model.Id, (1 * model.NumberOfPacks));
                }
                if (itemPack_ItemList.Count > 0)
                {
                    BusinessHandlerBook.AddItemPack_Book(itemPack_ItemList);
                }

            }


            return RedirectToAction("BookPacks");
        }
        [HttpPost]
        public JsonResult RemoveMediaItem(ItemMedia model)
        {
            return BusinessHandlerBook.RemoveMediaItem(model);
        }

        public JsonResult RemoveMultipleAuthor(Book_Author model)
        {
            JsonResult jr = new JsonResult();
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (BusinessHandlerAuthor.RemoveAuthorFromBook(model))
            {
                result.Add("success", "true");
            }
            else
            {
                result.Add("success", "false");
            }

            return jr;
        }

        public JsonResult GetBookValidationForBookPack(BookPackItemVM model)
        {
            JsonResult jr = new JsonResult();
            Book _book = BusinessHandlerBook.Get(model.BookId);
            BookProperties _bookProperty = BusinessHandlerBookProperties.GetById(model.PropertyId);
            SaleStatus preOrder =BusinessHandlerSaleStatus.GetSaleStatusByTitle("pre_order");
            SaleStatus NormalSaleType = BusinessHandlerSaleStatus.GetSaleStatusByTitle("normal_sale");
            //SaleStatus LimitedStockType = BusinessHandlerSaleStatus.GetSaleStatusByTitle("limited_stock");
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (_book !=null && _bookProperty != null)
            {
                if(_book.SaleType != preOrder.Id && _book.SaleType != NormalSaleType.Id)
                {
                    result.Add("message", "Can't Update. Book is not on Pre order or normal sale state");
                    result.Add("success", "failed");
                }else if (_bookProperty.NumberOfCopies<model.NumberOfBookPackItems)
                {
                    result.Add("message", "Can't Update. Not enough remaining items");
                    result.Add("success", "failed");
                }
                else
                {
                    result.Add("message", "Added successfully");
                    result.Add("success", "pass");
                }
            }
            else
            {
                result.Add("message", "Can't Update. Book is missing");
                result.Add("success", "failed");
            }
            jr.Data = result;
            return jr;
        }
    }
}