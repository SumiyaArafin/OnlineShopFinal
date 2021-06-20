using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.ViewModel
{
    public class LineItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ComparePrice { get; set; }
        public string Unit { get; set; }
        public decimal ProductTax { get; set; }
        public decimal Discount { get; set; }
        public decimal Subtotal { get; set; }
        public int PurchaseOrderId { get; set; }
        public bool Status { get; set; }

        public string image { get; set; }
    }
}
