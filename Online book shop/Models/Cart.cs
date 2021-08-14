using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CartStatus { get; set; }
        public Decimal AmountBeforeDiscount { get; set; }
        public Decimal Discount { get; set; }
        public Decimal AmountAfterDiscount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string VoucherCode { get; set; } = null;

        [NotMapped]
        public List<Cart_Book> Items { get; set; }
        [NotMapped]
        public string SessionId { get; set; }
        [NotMapped]
        public List<string> Messages { get; set; } = new List<string>();

        [NotMapped]
        public int SelectedBillingAddress { get; set; }
        [NotMapped]
        public int SelectedDeliveryAddress { get; set; }
        [NotMapped]
        public int SelectedDeliveryMethod { get; set; }

        [NotMapped]
        public int SelectedPaymentMethod { get; set; }

        [NotMapped]
        public string AddedPaymentSpecialNote{get;set;}
        [NotMapped]
       
        public string AddedDeliverySpecialNote { get; set; }

        [NotMapped]
        public string OrderId { get; set; }

        [NotMapped]
        public string ClientMessage { get; set; }
        [NotMapped]
        public int TotalItemsCount { get; set; }
    }
}