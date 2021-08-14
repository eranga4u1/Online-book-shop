using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerDeliveryStatus
    {
        public static List<DeliverStatus> GetAllActiveDeliverStatus()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliverStatuses.Where(c => !c.isDeleted).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static DeliverStatus GetDeliverStatus(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliverStatuses.Where(c => c.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool Update(DeliverStatus model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    DeliverStatus Obj = ctx.DeliverStatuses.Find(model.Id);
                    if (Obj != null)
                    {
                        Obj.DisplayText = model.DisplayText;
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

        internal static bool Delete(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    DeliverStatus Obj = ctx.DeliverStatuses.Find(id);
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
                   if(ctx.Orders.Where(x => x.DeliveryStatus == id).Count() > 0)
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

        internal static bool Add(DeliverStatus model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    model.isDeleted = false;
                    model.Title = model.DisplayText.ToLower().Replace(" ", "_");
                    ctx.DeliverStatuses.Add(model);
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

        internal static DeliverStatus GetDeliverStatusByTitle(string Title)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliverStatuses.Where(c => c.Title == Title).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}