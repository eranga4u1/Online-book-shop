using Online_book_shop.Handlers.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class Publisher
    {
        public Publisher()
        {
          this.FriendlyName = HTMLHelper.RemoveSpecialCharacters(this.Name);
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; } = "";
        public string Description { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int ProfilePictureMediaId { get; set; }
        public int CoverPictureMediaId { get; set; }
        public string ExternalURL { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string FriendlyName { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public Media ProfileImage { get; set; }
        [NotMapped]
        public Media CoverImage { get; set; }
    }
}