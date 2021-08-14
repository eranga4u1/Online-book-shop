using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class BookVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LocalTitle { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string YoutubeUrl { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int Ratings { get; set; }
        public int SaleType { get; set; }
        public DateTime PreReleaseEndDate { get; set; }
        public Media FrontCover { get; set; }
        public Media BackCover { get; set; }
        public Media ReadPDF { get; set; }
        public int LanguageId { get; set; }
        public List<BookPropertyVM> BookProperties { get; set; }
        public string Categories { get; set; }

        public int[] OtherAthors { get; set; }

        public int MaximumItemPerOrder { get; set; }

        //public int NumberOfPages_1 { get; set; }
        //public decimal WeightByGrams_1 { get; set; }
        //public int NumberOfCopies_1 { get; set; }
        //public decimal Price_1 { get; set; }
        //public int isAvailable_1 { get; set; }

        //public int NumberOfPages_2 { get; set; }
        //public int WeightByGrams_2 { get; set; }
        //public int NumberOfCopies_2 { get; set; }
        //public decimal Price_2 { get; set; }
        //public int isAvailable_2 { get; set; }

        //public int NumberOfPages_3 { get; set; }
        //public int WeightByGrams_3 { get; set; }
        //public int NumberOfCopies_3 { get; set; }
        //public decimal Price_3 { get; set; }
        //public int isAvailable_3 { get; set; }

        //public int NumberOfPages_4 { get; set; }
        //public int WeightByGrams_4 { get; set; }
        //public int NumberOfCopies_4 { get; set; }
        //public decimal Price_4 { get; set; }
        //public int isAvailable_4 { get; set; }

        //public int NumberOfPages_5 { get; set; }
        //public int WeightByGrams_5 { get; set; }
        //public int NumberOfCopies_5 { get; set; }
        //public decimal Price_5 { get; set; }
        //public int isAvailable_5 { get; set; }
    }
    public class BookPropertyVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfPages { get; set; }
        public decimal WeightByGrams { get; set; }
        public int NumberOfCopies { get; set; }
        public decimal Price { get; set; }
        public int isAvailable { get; set; }
        public int PromotionMethods { get; set; }
        public Double DiscountValue { get; set; }
    }
}