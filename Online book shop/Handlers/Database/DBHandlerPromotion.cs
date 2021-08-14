using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerPromotion
    {
        public static Promotion Add(Promotion promotion)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Promotions.Add(promotion);
                    ctx.SaveChanges();
                }
                return promotion;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static List<Promotion> Get(bool isActive=false,bool isDeleted=false)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {   
                    var promotions = isActive?ctx.Promotions.Where(p=> p.StartDate.AddDays(-1) < DateTime.Today && p.EndDate > DateTime.Today.AddDays(1) && !p.isDeleted): ctx.Promotions;
                    if (promotions != null)
                    {
                        return promotions.ToList();
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Promotion Get(int bookId, int propertyId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var promotions = ctx.Promotions.Where(x => x.ObjectId == bookId && x.ObjectType == (int)ObjectTypes.Book);
                    foreach(var p in promotions)
                    {
                        if (!string.IsNullOrEmpty(p.OtherParameters))
                        {
                            PromptionParameters pp = JsonConvert.DeserializeObject<PromptionParameters>(p.OtherParameters);
                            if (pp.BookPropertyId == propertyId)
                            {
                                return p;
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Promotion> Get(int Objid)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                  var promotions=  ctx.Promotions.Where(p => p.ObjectType == (int)PromotionTypesFor.Book && p.ObjectId== Objid && (p.StartDate < DateTime.Today && p.EndDate > DateTime.Today) && !p.isDeleted);
                    if(promotions != null)
                    {
                        return promotions.ToList();
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Promotion GetById(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var promotions = ctx.Promotions.Where(p => p.Id==id).FirstOrDefault();
                    if (promotions != null)
                    {
                        return promotions;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Promotion Update(Promotion promotion)
        {
            if(promotion != null)
            {
                try
                {
                    using (var ctx = new ApplicationDbContext())
                    {
                        Promotion promotionsDb = ctx.Promotions.Where(p => p.Id == promotion.Id).FirstOrDefault();
                        if(promotion != null)
                        {
                            promotionsDb.ObjectId = promotion.ObjectId;
                            promotionsDb.PromotionTitle = promotion.PromotionTitle;
                            promotionsDb.PromotionDescription = promotion.PromotionDescription;
                            promotionsDb.PromotionMethods = promotion.PromotionMethods;
                            promotionsDb.OtherParameters = promotion.OtherParameters;
                            promotionsDb.PromotionTypesFor = promotion.PromotionTypesFor;
                            promotionsDb.StartDate = promotion.StartDate;
                            promotionsDb.EndDate = promotion.EndDate;
                            promotionsDb.UpdatedDate = promotion.UpdatedDate;
                            promotionsDb.UpdatedBy = promotion.UpdatedBy;
                            promotionsDb.DiscountValue = promotion.DiscountValue;
                            promotionsDb.PromotionMediaId = promotion.PromotionMediaId > 0 ? promotion.PromotionMediaId : promotionsDb.PromotionMediaId;
                            ctx.SaveChanges();
                            return promotionsDb;
                        }
                    }
                  
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        internal static bool ShowHide(int id, bool v)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Promotion p = ctx.Promotions.Where(x => x.Id == id).FirstOrDefault();
                    if (p != null)
                    {
                        p.isDeleted = v;
                        p.UpdatedDate = DateTime.UtcNow;
                        p.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
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

        internal static List<Promotion> GetLatestPromotion()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var promotions = ctx.Promotions.Where(p =>p.StartDate < DateTime.Today && p.EndDate > DateTime.Today).Take(5);
                    if (promotions != null)
                    {
                        return promotions.Take(5).ToList();
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}