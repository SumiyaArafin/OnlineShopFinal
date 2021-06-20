using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PurchaseController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var purcahse = _db.PurchaseOrderLineItems.Include(m => m.Product);
            return View(purcahse);
        }
        public IActionResult Create(int? id)
        {
            ViewData["productName"] = new SelectList(_db.Product.ToList(), "Id", "Name");

            List<PurchaseOrderLineItem> ci = new List<PurchaseOrderLineItem> { new PurchaseOrderLineItem { Id = 0 } };
            //PurchaseOrder rowCount = _db.PurchaseOrders.LastOrDefault<PurchaseOrder>();
            //if (_db.PurchaseOrders.ToList().Count() == 0)
            //{
            //    int no = 1;
            //    ViewBag.puchaseNumber = no;
            //}
            //else
            //{
            //    int no = rowCount.ReferenceNo + 1;
            //    ViewBag.puchaseNumber = no;
            //}

            return View(ci);
        }
        [HttpPost, ActionName("Create")]

        public IActionResult CreatePost(List<PurchaseOrderLineItem> purchases)
        {

            ViewData["productName"] = new SelectList(_db.Product.ToList(), "Id", "Name");
            List<PurchaseOrderLineItem> ci = new List<PurchaseOrderLineItem> { new PurchaseOrderLineItem { Id = 0 } };

            DateTimeOffset date = purchases[0].PurchaseOrder.Date;
            PurchaseOrder rowCount = _db.PurchaseOrders.LastOrDefault<PurchaseOrder>();
            //int no = 1;
            //if (_db.PurchaseOrders.ToList().Count() > 0)
            //{
            //    no = rowCount.ReferenceNo + 1;

            //}
            foreach (var i in purchases)
            {
                //i.PurchaseOrder.ReferenceNo = no;
                i.PurchaseOrder.Date = date;
                _db.PurchaseOrderLineItems.Add(i);

            }
            _db.SaveChanges();
            ViewBag.Message = "Data successfully saved!";
            ModelState.Clear();
            
            return RedirectToAction("Index");
        }
    }
}
