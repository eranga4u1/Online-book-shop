using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerAddress
    {
        public static Address Add(Address model)
        {
            try
            {
                using(var ctx = new ApplicationDbContext())
                {
                    ctx.Addreses.Add(model);
                    if (ctx.SaveChanges() > 0)
                    {
                        return model;
                    }
                }

            }catch(Exception ex)
            {
               
            }
            return null;
        }

        internal static List<Address> GetAddresses(string userId)
        {
            try
            {
                using(var ctx= new ApplicationDbContext())
                {
                   return ctx.Addreses.Where(x => !x.isDeleted && x.isPublic && x.UserId==userId).ToList();
                }
            }catch(Exception ex)
            {

            }
            return null;
        }

        internal static bool RemoveAddress(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var address = ctx.Addreses.Where(x => x.Id == id).FirstOrDefault();
                    if(address != null)
                    {
                        address.isDeleted = true;
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        internal static Address Update(Address model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Obj =ctx.Addreses.Where(x=> x.Id==model.Id).FirstOrDefault();
                    if(Obj != null)
                    {
                        Obj.FirstName = model.FirstName;
                        Obj.LastName = model.LastName;
                        Obj.Company = model.Company;
                        Obj.AddressLine01 = model.AddressLine01;
                        Obj.AddressLine02 = model.AddressLine02;
                        Obj.AddressLine03 = model.AddressLine03;
                        Obj.City = model.City;
                        Obj.ContactNumber1 = model.ContactNumber1;
                        Obj.ContactNumber2 = model.ContactNumber2;
                        Obj.Country = model.Country;
                        Obj.District = model.District;
                        Obj.EmailAddress = model.EmailAddress;
                        Obj.isDeleted = false;
                        Obj.State = model.State;
                        Obj.UpdatedBy = model.UpdatedBy;
                        Obj.UpdatedDate = model.UpdatedDate;
                        if (ctx.SaveChanges() > 0)
                        {
                            return model;
                        }
                    }
                    
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        internal static bool SetDefault(int id, string userId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var alladdress = ctx.Addreses.Where(x => x.UserId== userId && !x.isDeleted);
                    if(alladdress != null)
                    {
                        foreach(Address a in alladdress)
                        {
                            a.isDefaultAddress = false;
                        }
                    }
                    var address = ctx.Addreses.Where(x => x.Id == id).FirstOrDefault();
                    if (address != null)
                    {
                        address.isDefaultAddress = true;
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        internal static Address GetAddress(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Addreses.Where(x => x.Id==id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}