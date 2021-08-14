using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class ItemPack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string LocalTitle { get; set; } = "";
        public string ItemPackIdentityID { get; set; }

        public int NumberOfPacks { get; set; }
        public string Description { get; set; }
        public string YoutubeUrl { get; set; }
        public int SaleType { get; set; }
        // public DateTime? PreReleaseEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }

        public int FrontCoverMediaId { get; set; }

        public decimal Price{ get; set; }

        [NotMapped]
        public string SelectedBooks { get; set; }

        [NotMapped]

        public HttpPostedFile[] image_files { get; set; }

    }
}