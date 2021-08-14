using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PromotionTitle { get; set; }
        public string PromotionDescription { get; set; }
        public int PromotionTypesFor { get; set; }
        public int PromotionMethods { get; set; }
        public int ObjectType { get; set; }
        public int ObjectId { get; set; }
        public int PromotionMediaId { get; set; }
        public Double DiscountValue { get; set; }
        public String OtherParameters { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
    public class PromptionParameters
    {
        public int BookPropertyId { get; set; }
    }
}