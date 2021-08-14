using Microsoft.Ajax.Utilities;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerDeliveryCharge
    {
        public static List<DeliveryCharge> GetPostalCharges(string Area= "Sri Lanka")
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliveryCharges.Where(b => b.DeliveryType == (int)DeliveryTypes.Postal_Service && b.Area.ToLower()==Area.ToLower()).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<DeliveryCharge> GetCourierCharges()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliveryCharges.Where(b => b.DeliveryType == (int)DeliveryTypes.Currier_Service &&  !b.isDeleted).DistinctBy(y => y.Area).OrderBy(d=>d.Area).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<DeliveryCharge> GetDeliveryChargesByType(DeliveryTypes type)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliveryCharges.Where(b => b.DeliveryType == (int)type && !b.isDeleted).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static DeliveryCharge GetDeliveryChargeByAreaId(string area, DeliveryTypes deliveryType)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliveryCharges.Where(b => b.DeliveryType == (int)deliveryType && b.Area.Trim().ToLower() == area.Trim().ToLower()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static DeliveryCharge GetDeliveryChargeById(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliveryCharges.Where(b => b.Id==id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static string GetAreaById(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var dc= ctx.DeliveryCharges.Where(b => b.Id == id).FirstOrDefault();
                    if(dc != null)
                    {
                        return dc.Area;
                    }
                    return "Sri Lanka";
                }
            }
            catch (Exception ex)
            {
                return "Sri Lanka";
            }
        }

        internal static List<DeliveryCharge> GetDeliveryCharges(DeliveryTypes type)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var dcs = ctx.DeliveryCharges.Where(b => b.DeliveryType==(int)type && !b.isDeleted).ToList();
                    
                    return dcs;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool Add(DeliveryCharge model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.DeliveryCharges.Add(model);

                    if (ctx.SaveChanges() > 0) { return true; } else { return false; }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static bool Remove(DeliveryCharge model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Obj = ctx.DeliveryCharges.Find(model.Id);
                    if(Obj != null)
                    {
                        Obj.isDeleted = true;
                        if (ctx.SaveChanges() > 0) {                            
                            return true; 
                        } else { return false; }
                    }

                    return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static List<string> GetEnabledCountryForDelivery()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.DeliveryCharges.Select(x => x.Country).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}