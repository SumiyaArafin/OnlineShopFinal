using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineShopFinal.ViewModel
{
    public class SubCategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }


        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public List<SelectListItem> SubCategories { get; set; }
        public List<CategoryViewModel> SubCategoryMenus { get; set; }
    }

    public class SubCategoryListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public int? SubCategoryId { get; set; }
        [Display(Name = "SubCategory")]
        public string SubCategoryParentName { get; set; }
        public int TotalProduct { get; set; }
    }
}
