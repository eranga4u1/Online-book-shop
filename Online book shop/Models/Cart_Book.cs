using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class Cart_Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int BookPropertyId { get; set; }
        public int NumberOfItems { get; set; }
        public Decimal AmountBeforeDiscount { get; set; }
        public Decimal Discount { get; set; }
        public Decimal AmountAfterDiscount { get; set; }
        public string SpecialNote { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public BookVMTile BookTile { get; set; }
    }
}