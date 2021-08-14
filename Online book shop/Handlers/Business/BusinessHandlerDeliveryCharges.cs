using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerDeliveryCharges
    {
        public static DeliveryCharge GetStaticDeliveryCharge(List<DeliveryCharge> dc, decimal WeightByGrams, string Area, string Country)
        {

            DeliveryCharge d = dc.Where(x => (x.Area.ToLower().Trim() == Area.ToLower().Trim() || x.Area==Country) &&
                                              x.Country.ToLower() == Country.ToLower() && !x.isDynamic &&
                                              (x.StartWeightByGrams < WeightByGrams || x.StartWeightByGrams == WeightByGrams) &&
                                              (x.EndWeightByGrams > WeightByGrams) && !x.isDynamic).FirstOrDefault();
            //if(d != null && (Area != "All" || Area != Country))
            //{
            //    return d;
            //}         
            //else if(Area != "All")
            //{
            //    return GetStaticDeliveryCharge(dc,WeightByGrams,"All",Country);
            //}
            return d;
        }
        public static decimal GetDynamicDeliveryCharge(List<DeliveryCharge> dc, decimal WeightByGrams, string Area, string Country)
        {
            DeliveryCharge baseCharge= dc.Where(x=> (x.Area.ToLower().Trim() == Area.ToLower().Trim() || x.Area == Country ) &&
                                                x.Country.ToLower() == Country.ToLower() && !x.isDynamic).
                                                OrderByDescending(x=> x.EndWeightByGrams).FirstOrDefault();
            DeliveryCharge d = dc.Where(x => (x.Area.ToLower().Trim() == Area.ToLower().Trim()|| x.Area == Country) && x.isDynamic &&
                                             x.Country.ToLower() == Country.ToLower()
                                              && x.isDynamic).FirstOrDefault();
            if (baseCharge != null && d !=null)
            {
                if (WeightByGrams < baseCharge.EndWeightByGrams)
                {
                    return baseCharge.Amount;
                }
                else
                {
                    decimal remaning = (WeightByGrams - baseCharge.EndWeightByGrams) / d.SliceByGrams;
                    int nearest = (int)Math.Ceiling(remaning);
                    return (baseCharge.Amount + nearest * d.UnitPricePerSlice);
                }
            }
            else if(d != null)
            {
                decimal remaning = d.EndWeightByGrams / d.SliceByGrams;
                int nearest = (int)Math.Ceiling(remaning);
                return (d.UnitPricePerSlice);
            }
            else if(Area != "All")
            {
                return GetDynamicDeliveryCharge(dc, WeightByGrams, "All", Country);
            }
            return 0;
        }
        public static decimal GetDeliveryCharge(decimal WeightByGrams,DeliveryTypes type,string Area, string Country)
        {
            if (type == DeliveryTypes.In_Store_Pickup)
            {
                return 0;
            }
            if(Country != "Sri Lanka")
            {
                Area = Country;
                type = DeliveryTypes.Foreign_Airmail;
            }
            List<DeliveryCharge> list = DBHandlerDeliveryCharge.GetDeliveryChargesByType(type).Select(y=> new DeliveryCharge { 
            Id=y.Id,
            DeliveryType=y.DeliveryType,
            StartWeightByGrams=y.StartWeightByGrams,
            EndWeightByGrams=y.EndWeightByGrams,
            Amount=y.Amount,
            Area=(y.Area=="All"|| string.IsNullOrEmpty(y.Area))?y.Country:y.Area,
            Country=y.Country,
            SliceByGrams=y.SliceByGrams,
            UnitPricePerSlice=y.UnitPricePerSlice,
            isDynamic=y.isDynamic
            }).ToList();

            DeliveryCharge d = GetStaticDeliveryCharge(list, WeightByGrams, Area, Country);
           if ( d != null)
            {
                return d.Amount;
            }
            else
            {
                d = GetStaticDeliveryCharge(list, WeightByGrams, Country, Country);
                if (d != null)
                {
                    return d.Amount;
                }
                else
                {                 
                     return GetDynamicDeliveryCharge(list, WeightByGrams, Area, Country);
                }
                
            }
        }
        public static List<DeliveryCharge> GetCourierCharges()
        {
            return DBHandlerDeliveryCharge.GetCourierCharges();
        }
        public static List<DeliveryCharge> GetPostalCharges()
        {
            return DBHandlerDeliveryCharge.GetPostalCharges();
        }
        public static string GetAreaById(int id)
        {
            return DBHandlerDeliveryCharge.GetAreaById(id);
        }
        public static DeliveryCharge GetDeliveryChargeByAreaId(string Area, DeliveryTypes deliveryType) => DBHandlerDeliveryCharge.GetDeliveryChargeByAreaId(Area, deliveryType);
        public static DeliveryCharge GetDeliveryChargeById(int id) => DBHandlerDeliveryCharge.GetDeliveryChargeById(id);

        public static List<DeliveryCharge> GetDeliveryCharges(DeliveryTypes type)
        {
            return DBHandlerDeliveryCharge.GetDeliveryCharges(type);
        }

        internal static bool Add(DeliveryCharge model)
        {
            return DBHandlerDeliveryCharge.Add(model);
        }

        internal static bool remove(DeliveryCharge model)
        {
            return DBHandlerDeliveryCharge.Remove(model);
        }
        public static List<string> GetEnabledCountryForDelivery()
        {
            return DBHandlerDeliveryCharge.GetEnabledCountryForDelivery();
        }
        public static List<string> GetEnabledDistrictForDelivery()
        {
            return DBHandlerDeliveryCharge.GetEnabledCountryForDelivery();
        }
    }
}