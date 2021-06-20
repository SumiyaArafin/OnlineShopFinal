using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.ViewModel
{
    public class OrderInfoViewModel
    {
        public int Id { get; set; }

        public string OrderNo { get; set; }

        public string Name { get; set; }


        public bool Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SubtoTotal { get; set; }

        public DateTime Date { get; set; }
    }
}
