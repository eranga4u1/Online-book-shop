using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class BookVMTile
    {
        public int Id { get; set; }
        public Media FrontCover { get; set; }
        public Media BackCover { get; set; }
        public string BookName { get; set; }
        public string LocalBookName { get; set; }
        public string AuthorName { get; set; }
        public string LocalAuthorName { get; set; }
        public List<Category> Categories { get; set; }
        public int Rating { get; set; } = 1;
        public List<BookProperties> Property { get; set;}
        public decimal DiscountPercentage { get; set; } = 0;
        public bool isDeleted { get; set; }

        public int SaleType { get; set; }
        public int NumberOfPages { get; set; }
        public Decimal WeightByGrams { get; set; }
        public string ISBN { get; set; }
        public Publisher publisher { get; set; }
        public int AuthorId { get; set; }
        public DateTime RelaseDate { get; set; }
        public string Description { get; set; }
        public string YoutubeUrl { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
        public int MaximumItemPerOrder { get; set; }

        public int ItemType { get; set; }
    }
}