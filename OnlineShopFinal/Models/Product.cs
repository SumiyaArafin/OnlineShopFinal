using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
 
    
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public decimal AvailablePrice { get; set; }

        public decimal PreviousPrice { get; set; }

        public string ProductColor { get; set; }

        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int Size { get; set; }
        [Display(Name = "Currency")]
        public string SizeUnite { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public string Image { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<PurchaseOrderLineItem> PurchaseOrderLineItems { get; set; }
    }
}
