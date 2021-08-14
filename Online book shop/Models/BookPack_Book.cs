using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class ItemPack_Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ItemPackId { get; set; }//BookPackId

        public int ItemId { get; set; }//BookId
        public int ItemPropertyId { get; set; }//BookPropertyId
        public int NumberOfItems { get; set; }
    }
}