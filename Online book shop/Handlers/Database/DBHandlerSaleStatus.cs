using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerSaleStatus
    {
        public static List<SaleStatus> GetAllActiveSaleStatus()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.SaleStatus.Where(c => !c.isDeleted).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static SaleStatus GetSaleStatus(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.SaleStatus.Where(c => c.Id== id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool Update(SaleStatus model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    SaleStatus Obj = ctx.SaleStatus.Find(model.Id);
                    if(Obj != null)
                    {
                        Obj.DisplayText = model.DisplayText;
                        Obj.BackGroundColor = model.BackGroundColor;
                        Obj.ForeColor = model.ForeColor;
                        Obj.isAddToCartEnables = model.isAddToCartEnables;
                        model.isDeleted = false;
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

        //internal static SaleStatus GetBookSaleStatusOnGivenDate(int bookId, DateTime date)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        internal static bool Delete(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    SaleStatus Obj = ctx.SaleStatus.Find(id);
                    if (Obj != null)
                    {
                        Obj.isDeleted = true;
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

        internal static bool IsAssigned(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   if(ctx.Books.Where(x=> x.SaleType == id).Count() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static bool Add(SaleStatus model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    model.isDeleted = false;
                    model.Title = model.DisplayText.ToLower().Replace(" ", "_");
                    ctx.SaleStatus.Add(model);
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static SaleStatus GetSaleStatusByTitle(string Title)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.SaleStatus.Where(c => c.Title== Title).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}