using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using cloudscribe.Pagination.Models;

namespace OnlineShopFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
       


        public IActionResult Index()
        {
            var suppliers = _db.Categories.ToList();
            ViewBag.Suppliers = suppliers;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            var supplier = _db.Categories.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                _db.Categories.Remove(supplier);
                _db.SaveChanges();
                result = true;
            }
            return Json(result);
        }

        public async Task<IActionResult> GetCategoryDetails(int id)
        {
            var SubCategoryList = await _db.Product.Where(x => x.CategoryId == id).ToListAsync();
            return View(SubCategoryList);
        }


        //public IActionResult GetCategoryDetails(int id,int pageNumber = 1, int pageSize = 6)
        //{
        //    ViewBag.NewOrder = _db.Orders.ToList()
        //                           .Where(i => i.Status == false).ToList();
        //    ViewBag.TotalProducts = _db.Product.ToList();
        //    ViewBag.TotalCustomer = _db.ApplicationUsers.ToList();
        //    ViewBag.TotalCategory = _db.Categories.ToList();

        //    int ExcludeRecords = (pageSize * pageNumber) - pageSize;

        //    var Bikes = _db.Product.Where(x => x.CategoryId == id)
        //        .Skip(ExcludeRecords)
        //        .Take(pageSize);
        //    var result = new PagedResult<Product>
        //    {
        //        Data = Bikes.AsNoTracking().ToList(),
        //        TotalItems = _db.Product.Count(),
        //        PageNumber = pageNumber,
        //        PageSize = pageSize

        //    };
        //    return View(result);
        //}
    }
}