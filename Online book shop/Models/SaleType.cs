using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class SaleStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string DisplayText { get; set; }
        public string BackGroundColor { get; set; }
        public string ForeColor { get; set; }
        public bool isDeleted { get; set; }
        public bool isAddToCartEnables { get; set; }
    }
}