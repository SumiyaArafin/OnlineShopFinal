using OnlineShopFinal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.ViewModel
{
    public class FirstBlogViewModel : BaseEntity
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
        public string BlogHeader { get; set; }
        public string BlogDetails { get; set; }
        public string BlockQuote { get; set; }
        public string BlockQuoteDetails { get; set; }
    }
    public class FirstBlogListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
        public string BlogHeader { get; set; }
        public string BlogDetails { get; set; }
        public string BlockQuote { get; set; }
        public string BlockQuoteDetails { get; set; }
    }
}
