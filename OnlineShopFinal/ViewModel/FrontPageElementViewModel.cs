using OnlineShopFinal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.ViewModel
{
   
 
    public class FrontPageElementViewModel : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string LatestNewsDescription { get; set; }

    }

    public class FrontPageElementListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalProducts { get; set; }
        public string LatestNewsDescription { get; set; }
    }
}
