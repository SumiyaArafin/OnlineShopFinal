using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using OnlineShopFinal.Models.ViewModels;

namespace OnlineShopFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ModelViewModel ModelVM { get; set; }

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
            ModelVM = new ModelViewModel()
            {
                Categories = _db.Categories.ToList(),
                SubCategory = new Models.SubCategory()
            };
        }
        public IActionResult Index()
        {
            var model = _db.SubCategories.Include(m => m.Category);
            return View(model);
        }

        //public IActionResult Create()
        //{
        //    return View(ModelVM);
        //}
        public IActionResult Create()
        {
            //return View(ModelVM);
            return PartialView("_EmployeeModelPartial", ModelVM);
        }
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVM);
            }
            _db.SubCategories.Add(ModelVM.SubCategory);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        //HTTP Get Method
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ModelVM.SubCategory = _db.SubCategories.Include(m => m.Category).SingleOrDefault(m => m.Id == id);
            if (ModelVM.SubCategory == null)
            {
                return NotFound();
            }

            return View(ModelVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVM);
            }

            _db.Update(ModelVM.SubCategory);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    SubCategory model = _db.SubCategories.Find(id);
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }
        //    _db.SubCategories.Remove(model);
        //    _db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}


        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            var supplier = _db.SubCategories.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                _db.SubCategories.Remove(supplier);
                _db.SaveChanges();
                result = true;
            }
            return Json(result);
        }
    }
}