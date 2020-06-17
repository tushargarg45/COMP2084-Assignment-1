using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string Sex { get; set; }
    }
}
