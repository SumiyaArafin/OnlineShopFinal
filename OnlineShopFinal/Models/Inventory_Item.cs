using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public class Inventory_Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WarningLevel { get; set; }
        public string Remarks { get; set; }
        public bool FixedAssests { get; set; }
        public bool Status { get; set; }
        public string ImagePath { get; set; }


    }
}
