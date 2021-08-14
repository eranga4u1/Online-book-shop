using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerUserReviews
    {
        internal static UserReview Update(UserReview ur)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var item =ctx.UserReviews.Where(x => x.UserId == ur.UserId && ur.ObjectId == x.ObjectId && ur.ObjectType == x.ObjectType).FirstOrDefault();
                    if(item != null)
                    {
                        item.value = ur.value != 0 ? ur.value : item.value;
                        item.UserComment = !string.IsNullOrEmpty(ur.UserComment) ? ur.UserComment : item.UserComment;
                        item.CreatedDate = ur.CreatedDate;
                        item.isApproved = ur.isApproved;
                        item.isanonymous = ur.isanonymous;
                        item.isspolier = ur.isspolier;
                        var previous = ctx.UserReviews.Where(x => ur.ObjectId == x.ObjectId && ur.ObjectType == x.ObjectType);
                        item.UpdatedRate = ur.value;
                        if (previous != null && previous.Count() > 0)
                        {
                            item.UpdatedRate = Convert.ToInt32(previous.Select(x => x.value).DefaultIfEmpty(0).Average());
                        }
                    }
                    else
                    {
                        ctx.UserReviews.Add(ur);
                        var previous = ctx.UserReviews.Where(x => ur.ObjectId == x.ObjectId && ur.ObjectType == x.ObjectType);
                        ur.UpdatedRate = ur.value;
                        if (previous != null && previous.Count() > 0)
                        {
                            item.UpdatedRate = Convert.ToInt32(previous.Select(x => x.value).DefaultIfEmpty(0).Average());
                        }
                    }
                   
                    ctx.SaveChanges();
                    return ur;
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        internal static UserReview UpdateRateOnly(UserReview ur)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var item = ctx.UserReviews.Where(x => x.UserId == ur.UserId && ur.ObjectId == x.ObjectId && ur.ObjectType == x.ObjectType).FirstOrDefault();
                    if (item != null)
                    {
                        item.value = ur.value != 0 ? ur.value : item.value;
                        var previous = ctx.UserReviews.Where(x => ur.ObjectId == x.ObjectId && ur.ObjectType == x.ObjectType);
                        item.UpdatedRate = ur.value;
                        if (previous != null && previous.Count() > 0)
                        {
                            item.UpdatedRate = Convert.ToInt32(previous.Select(x => x.value).DefaultIfEmpty(0).Average());
                        }
                    }
                    else
                    {
                        ctx.UserReviews.Add(ur);
                        var previous = ctx.UserReviews.Where(x => ur.ObjectId == x.ObjectId && ur.ObjectType == x.ObjectType);
                        ur.UpdatedRate = ur.value;
                        if (previous != null && previous.Count() > 0)
                        {
                            item.UpdatedRate = Convert.ToInt32(previous.Select(x => x.value).DefaultIfEmpty(0).Average());
                        }
                    }

                    ctx.SaveChanges();
                    return ur;
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);

                return null;
            }
        }

        internal static int GetReviewsAndCommentCount(string uid)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                  return  ctx.UserReviews.Where(x => x.UserId == uid).Count();
                }
            }catch(Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);

                return 0;
            }
        }

        internal static List<BookVMTile> GetWhistListByUser(string uid)
        {
            try
            {
                List<BookVMTile> list = new List<BookVMTile>();
                using (var ctx = new ApplicationDbContext())
                {
                    List<int> bookList = ctx.WishListItems.Where(x => x.UId == uid && !x.isDeleted).Select(x => x.BookId).ToList();
                    if (bookList != null && bookList.Count > 0)
                    {
                        var RelavantBooks = from a in (from q in ctx.Books
                                                       where bookList.Contains(q.Id) && !q.isDeleted
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
                                                Property = ctx.BookProperties.Where(x => x.BookId == a.Id).ToList(),
                                                Categories = (from r in (from t in ctx.Book_Categories
                                                                         where t.BookId == a.Id
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
                        return RelavantBooks.ToList();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);

                return null;
            }
        }
        internal static int GetBookRating(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var rate = ctx.UserReviews.Where(x => x.ObjectId == id);
                    int Ur = rate!=null? rate.Select(y=> (int?)y.value).Sum() ?? 0:0;
                 
                    return Ur;
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);
                return 0;
            }
        }

        internal static bool ShowHide(int id, bool v)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    UserReview Ur = ctx.UserReviews.Where(x => x.Id == id).FirstOrDefault();
                    if (Ur != null)
                    {
                        Ur.isApproved = v;

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
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);

                return false;
            }
        }

        internal static List<UserReview> GetReviews(bool withApproved=false)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if (withApproved)
                    {
                        return ctx.UserReviews.Where(x =>  !string.IsNullOrEmpty(x.UserComment)).OrderByDescending(x => x.Id).ToList();

                    }
                    else
                    {
                        return ctx.UserReviews.Where(x => !x.isApproved && !string.IsNullOrEmpty(x.UserComment)).OrderByDescending(x => x.Id).ToList();

                    }

                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);

                return null;
            }
        }

        internal static int GetRecentReviewForBook(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var item= ctx.UserReviews.Where(x => x.ObjectId == id && x.ObjectType == (int)ObjectTypes.Book).OrderByDescending(x=> x.Id).Take(1).FirstOrDefault();
                    return item !=null?item.UpdatedRate:0;
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);

                return 0;
            }
        }

        internal static List<ReviewVM> GetReviewsForBook(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   return (from a in (ctx.UserReviews.Where(x => x.ObjectId == id && x.ObjectType == (int)ObjectTypes.Book && x.isApproved)) join b 
                          in ctx.Users on a.UserId equals b.Id select new ReviewVM
                          {
                                    Id=a.Id,
                                    UserId =a.UserId,
                                    value=a.value,
                                    UpdatedRate =a.UpdatedRate,
                                    CreatedDate=a.CreatedDate,
                                    CreatedBy=a.CreatedBy ,
                                    UserComment=a.UserComment,
                                    Reviewer=b,
                                    isspolier=a.isspolier,
                                    isanonymous=a.isanonymous
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerUserReviews", MethodBase.GetCurrentMethod().Name);

                return null;
            }
        }
    }
}