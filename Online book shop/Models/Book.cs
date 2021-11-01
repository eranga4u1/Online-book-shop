using Online_book_shop.Handlers.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class Book
    {
        public Book()
        {
            this.FriendlyName = HTMLHelper.RemoveSpecialCharacters(this.Title);
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string LocalTitle { get; set; } = "";
        public string ISBN { get; set; }
        public int PublisherId { get; set; }
        public string Description { get; set; }
        public string YoutubeUrl { get; set; }
        public int Ratings { get; set; }
        public int AuthorId { get; set; }
        public int SaleType { get; set; }
       // public DateTime? PreReleaseEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime RelaseDate { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public string FriendlyName { get; set; }
        public int MaximumItemPerOrder { get; set; }
        public int ItemType { get; set; }
        [NotMapped]
        public string AuthorName { get; set; }

    }
}