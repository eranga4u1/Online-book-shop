using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel.Report
{
    public class WebOrder
    {
        public string Date { get; set; }
        public string WayBillId { get; set; }
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReceiverName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DistrictName { get; set; }
        public string ReceiverPhone { get; set; }
        public string COD { get; set; }
        public string Description { get; set; }
        public string PayerName { get; set; }
        public string PayerAddress { get; set; }
        public string PayerPhone { get; set; }
        public string SpecialNote { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal Total { get; set; }
        public string Email { get; set; }
    }
}