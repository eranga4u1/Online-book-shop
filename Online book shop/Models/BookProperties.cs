using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class BookProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfPages { get; set; }
        public int LanguageId { get; set; }
        public Decimal WeightByGrams { get; set; }
        public int FrontCoverMediaId { get; set; }
        public int BackCoverMediaId { get; set; }
        public int NumberOfCopies { get; set; }
        public Decimal Price { get; set; }
        public int FreeReadPDFMediaId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }


    }
}