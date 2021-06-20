using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public class IndexVM
    {
        public List<Product> Products { get; set; }
        public List<PurchaseOrderLineItem> PurchaseOrderLineItems { get; set; }
        public List<FrontPageElement> FrontPageElementss { get; set; }
        public List<FirstBlog> FirstBlogs { get; set; }
        public List<PurchaseOrderLineItem> LatestProducts { get; set; }
    }
}
