using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Online_book_shop.Models
{
    public class ActiveMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public int ActiveMenuOrderNo { get; set; }
        public bool isDeleted { get; set; }
    }
}