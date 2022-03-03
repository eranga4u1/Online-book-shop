using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UId { get; set; }
        public int CartId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BillingAddress { get; set; }
        public int BillingAddressId { get; set; }
        public string DeliveryAddress { get; set; }
        public int DeliveryAddressId { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public int PaymentMethod { get; set; }
        public int AreaId { get; set; }
        public decimal DeliveryCharges { get; set; }
        public string PaymentSpecialNote { get; set; }
        public string DeliverySpecialNote { get; set; }

        public int DeliveryStatus { get; set; }

        public int PaymentStatus { get; set; }

        public string OrderSummary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string WaybillId { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public int update_ac_info { get; set; }

    }
}