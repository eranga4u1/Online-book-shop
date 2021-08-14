using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class DeliveryCharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DeliveryType { get; set; }
        public int StartWeightByGrams { get; set; }
        public int EndWeightByGrams { get; set; }
        public decimal Amount { get; set; }
        public string Area { get; set; }
        public string Country { get; set; }

        public int SliceByGrams { get; set; }

        public decimal UnitPricePerSlice { get; set; }
        public bool isDynamic { get; set; }
        public bool isDeleted { get; set; }
    }
}