using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public int NumberOfValidCodes { get; set; }
        public int NumberOfUsedCodes { get; set; }
        public decimal VoucherAmount { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
//අපේ vouchers තියනවා.එකේ code එක enter කරන්න දෙනවා නම් වඩා හොඳ cart එක තියන page එකේ නේද?

//එතැනදී ගණන් හිලවු අඩුවෙලා ගෙවන්න තියන ඉතිරිය පෙන්නන එක හොඳයි නේ.

//1500/= ක පොත් අරන් 1000/= ක voucher code එක enter කරලා apply කරාම, එකේම ගෙවන්න ඕන දැන් 500/= කියලා පෙන්නන්න.