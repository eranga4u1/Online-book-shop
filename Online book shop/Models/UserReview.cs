using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class UserReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ObjectType { get; set; }
        public int ObjectId { get; set; }      
        public int value { get; set; }
        public int UpdatedRate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UserComment { get; set; }
        public bool isApproved { get; set; }
        public bool isspolier { get; set; }
        public bool isanonymous { get; set; }
        [NotMapped]
        public ApplicationUser Reviewer { get; set; }
    }
}