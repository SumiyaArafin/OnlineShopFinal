using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "SubCategory")]
        public string Name { get; set; }
        [Display(Name = "Category")]
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
