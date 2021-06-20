using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }        

    }
}
