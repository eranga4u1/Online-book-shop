using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerVoucher
    {
        public static List<Voucher> GetAllActiveVouchers(bool withDelete=false)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var codes = withDelete? ctx.Vouchers.ToList() : ctx.Vouchers.Where(v=> ! v.isDeleted && (v.NumberOfValidCodes>v.NumberOfUsedCodes || v.NumberOfValidCodes == v.NumberOfUsedCodes)).ToList();

                    return codes;
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerVoucher", MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        internal static Voucher AddVoucher(Voucher voucher)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Vouchers.Add(voucher);
                    if (ctx.SaveChanges() > 0)
                    {
                        return voucher;
                    }

                }
                return voucher;
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerVoucher", MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        internal static bool ShowHide(int id, bool v)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Voucher voucher = ctx.Vouchers.Where(x => x.Id == id).FirstOrDefault();
                    if (voucher != null)
                    {
                        voucher.isDeleted = v;
                        voucher.UpdatedDate = DateTime.UtcNow;
                        voucher.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
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
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerVoucher", MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        internal static bool SpendVoucher(string code, int orderId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   Voucher v= ctx.Vouchers.Where(x=> x.Code.ToLower()==code.ToLower()).FirstOrDefault();
                    if(v != null)
                    {
                        v.NumberOfUsedCodes = v.NumberOfUsedCodes + 1;
                        v.UpdatedDate = DateTime.UtcNow;
                        v.UpdatedBy= BusinessHandlerAuthor.GetLoginUserId();
                        v.OrderId = orderId;
                        if (v.NumberOfUsedCodes == 0)
                        {
                            v.isDeleted = true;
                        }
                        VoucherUser voucherUser = new VoucherUser
                        {
                            VoucherId = v.Id,
                            UserId = BusinessHandlerAuthor.GetLoginUserId(),
                            OrderId = orderId,
                            CreatedDate = DateTime.UtcNow
                        };
                        ctx.VoucherUsers.Add(voucherUser);
                        ctx.SaveChanges();
                        return true;
                    }
                   

                }
                return false;
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerVoucher", MethodBase.GetCurrentMethod().Name);
                return false;
            }

        }

        internal static bool IsVoucherUsed(string user, int voucherID)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var list= ctx.VoucherUsers.Where(x => x.UserId == user && x.VoucherId == voucherID).ToList();
                    if (list !=null && list.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }catch(Exception ex)
            {

            }
            return true;
        }
    }
}