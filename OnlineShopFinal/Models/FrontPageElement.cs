using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public class FrontPageElement
    {
        public int Id { get; set; }
        [Display(Name = "Slider-Text")]
        public string Name { get; set; }
        [Display(Name = "Slider-Banner")]
        public string Image { get; set; }
        [Display(Name = "Header-Description")]
        public string Description { get; set; }
        [Display(Name = "Footer-News-Description")]
        public string LatestNewsDescription { get; set; }
    }
}
