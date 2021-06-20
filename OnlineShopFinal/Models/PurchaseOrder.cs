using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public class PurchaseOrder
    {
        public PurchaseOrder()
        {
            PurchaseOrderLineItems = new HashSet<PurchaseOrderLineItem>();
        }
        public int Id { get; set; }

        public string ReferenceNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Status { get; set; }
        //Navigation Propertity
        public virtual ICollection<PurchaseOrderLineItem> PurchaseOrderLineItems { get; set; }
    }
}