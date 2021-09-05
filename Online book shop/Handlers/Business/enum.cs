using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
   public enum MediaCategory
    {
        ProfilePicture,
        CoverImage,
        BookFrontCover,
        BookBackCover,
        ReadablePDF
    }
    //public enum SaleType
    //{
    //    PreOrder,
    //    NormalSale,
    //    OutOfPrint,
    //    In2Days,
    //    In2Weeks
    //}
    public enum StockEntryOperation
    {
        In,
        Out,
        In_Admin_Updated,
        Out_Admin_Updated,
        Added_To_Item_Pack,
        Out_For_Book_Pack,
        In_From_Book_Pack
    }
    public enum CartStatus
    {
        DraftCart,
        SavedCart,
        OrderConfirmedCart,
        DeliveredCart
    }
    //public enum DeliveryStatus
    //{
    //    TempCart,
    //    ConfirmedOrder,
    //    Processing,
    //    DispatchedFromStore,
    //    Delivered,
    //    Complete,
    //    Denied,
    //    Refunded,
    //    Returned
    //}
    public enum PaymentStatus
    {
        PendingPayment,
        Paid,
        CheckOnPayHere
    }
    public enum DeliveryTypes
    {
        Postal_Service,
        Currier_Service,
        Foreign_Airmail,
        EMS,
        In_Store_Pickup
    }
    public enum PaymentMethods
    {
        Cash_On_Delivery,
        Online_Payment,
        Bank_Deposit,
        Ez_cash,
        In_store_payment
    }
    public enum ObjectTypes
    {
        Book,
        Author,
        Publisher,
        Category,
        ItemPack
    }
    public enum PromotionTypesFor
    {
        Book,
        Author,
        Publisher,
        Category,
        Collection,
        Delivery,
        User,
        PromotionCode
    }
    public enum PromotionMethods
    {
        FixedAmount,
        Percentage,
        DeductFromFinalAmount
    }

    public enum LogType
    {
        Exception,
        Message,
        GateParse,
        StatusChanged
    }

    public enum OrderType
    {
        PreOrder,
        DailyOrder
    }

    public enum EmailStatues
    {
        Pending,
        Sent,
        Failed
    }

    public enum EmailPriority
    {
        Normal,
        Urgent
    }

    public enum ItemType
    {
        Book,
        BookPack
    }
}