using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public string BookName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
