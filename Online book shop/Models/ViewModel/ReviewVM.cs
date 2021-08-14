using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class ReviewVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int value { get; set; }
        public int UpdatedRate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UserComment { get; set; }
        public ApplicationUser Reviewer { get; set; }
        public bool isspolier { get; set; }
        public bool isanonymous { get; set; }
    }
}