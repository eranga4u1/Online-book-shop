using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerVoucher
    {
        private static Random random = new Random();
        public static Voucher GenarateVoucherCode(decimal amount,int numberOfCodes=1, int times=1, List<Voucher> Codes = null)
        {
            if(Codes == null)
            {
                Codes = DBHandlerVoucher.GetAllActiveVouchers(false);
            }
          
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            if (times == 50)
            {
                return null;
            }
            else if(Codes.Where(x => x.Code == code) !=null && Codes.Where(x=> x.Code == code).Count() > 0)
            {
               return GenarateVoucherCode(amount, numberOfCodes, times +1,Codes);
            }
            else
            {
                return new Voucher
                {
                    Code = code,

                    NumberOfValidCodes = numberOfCodes,
                    NumberOfUsedCodes = 0,
                    VoucherAmount = amount,
                    OrderId = 0,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = BusinessHandlerAuthor.GetLoginUserId(),
                    UpdatedBy = BusinessHandlerAuthor.GetLoginUserId(),
                    UpdatedDate = DateTime.UtcNow
                };
            }
        }
        public static bool Spendvoucher(string code,int orderId)
        {
            return DBHandlerVoucher.SpendVoucher(code,orderId);
        }
        internal static bool Show(int id)
        {
            return DBHandlerVoucher.ShowHide(id, false);
        }

        internal static bool Hide(int id)
        {
            return DBHandlerVoucher.ShowHide(id, true);
        }

        public static List<Voucher> GetAllActiveVouchers()
        {
            return DBHandlerVoucher.GetAllActiveVouchers();
        }
        public static List<Voucher> GetAllVouchers()
        {
            return DBHandlerVoucher.GetAllActiveVouchers(true);
        }
        public static Voucher AddVoucher(Voucher voucher)
        {
            return DBHandlerVoucher.AddVoucher(voucher);
        }

        public static Voucher GetActiveVoucherByCode(string code)
        {
            
            Voucher voucher = DBHandlerVoucher.GetAllActiveVouchers(true).Where(v => v.Code.ToLower() == code.ToLower()).FirstOrDefault();
            if(voucher != null)
            {
                if (!DBHandlerVoucher.IsVoucherUsed(BusinessHandlerAuthor.GetLoginUserId(), voucher.Id))
                {
                    return voucher;
                }
                   
            }
            return null;
        }
        public static Voucher GetActiveVoucherByCodeForInvoice(string code)
        {

            Voucher voucher = DBHandlerVoucher.GetAllActiveVouchers(true).Where(v => v.Code.ToLower() == code.ToLower()).FirstOrDefault();
            if (voucher != null)
            {
                if (DBHandlerVoucher.IsVoucherUsed(BusinessHandlerAuthor.GetLoginUserId(), voucher.Id))
                {
                    return voucher;
                }

            }
            return null;
        }
    }
}