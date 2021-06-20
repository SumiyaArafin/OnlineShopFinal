using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopFinal.Models;

namespace OnlineShopFinal.ViewModel
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<SubCategory> SubCategories { get; set; }
        

        public ProductViewModel()
        {
            Images = new List<IFormFile>();
            CategoryList = new List<SelectListItem>();
            SubCategoryList = new List<SelectListItem>();
            UnitList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public SubCategory SubCategory { get; set; }

        public int SubCategoryId { get; set; }

        public decimal AvailablePrice { get; set; }

        public decimal PreviousPrice { get; set; }

        public string ProductColor { get; set; }

        public int Quantity { get; set; }
        public int UnitId { get; set; }

        public string Image { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int Size { get; set; }
        [Display(Name = "Currency")]
        public string SizeUnite { get; set; }

        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> SubCategoryList { get; set; }
        public List<SelectListItem> UnitList { get; set; }

        //Image Upload
        public IList<IFormFile> Images { get; set; }
    }

    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public string UnitName { get; set; }


        public string CategoryName { get; set; }
   
        public string BrandName { get; set; }

        public string SubCategoryName { get; set; }

        public string Image { get; set; }

        public int UnitId { get; set; }
        public SubCategory SubCategory { get; set; }
        public int SubCategoryId { get; set; }
        public decimal AvailablePrice { get; set; }
        public decimal PreviousPrice { get; set; }
        public string ProductColor { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        [Display(Name = "Currency")]
        public string SizeUnite { get; set; }
        public int TotalImage { get; set; }

        //For HomePage
        public string ImageTitle { get; set; }
        public string ImagePath { get; set; }
        public List<ProductImageListViewModel> ImageList { get; set; }
        public string SecondImagePath { get; set; }
        //public List<CommentsListViewModel> ProductCommentsList { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class ProductImageListByProduct
    {
        public string Path { get; set; }
        public List<ProductImageListViewModel> ProuctImages { get; set; }
    }
}
