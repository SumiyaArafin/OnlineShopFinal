using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using OnlineShopFinal.Repository;
using OnlineShopFinal.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShopFinal.Controllers.Admin
{
    [Area("Admin")]
    public class UnitsController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public UnitsController(
            IUnitOfWork unitOfWork,
            ApplicationDbContext db
        )
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index(string search="")
        {
            List<UnitListViewModel> model = new List<UnitListViewModel>();
            var dbData = _unitOfWork.Repository<Unit>().GetAllInclude(x => x.Products);
            if (!String.IsNullOrEmpty(search))
            {
                dbData=dbData.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();

            }

            foreach (var b in dbData)
            {
                
                UnitListViewModel unit = new UnitListViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    TotalProducts = b.Products.Count
                };

                model.Add(unit);
            }


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditUnit(int id)
        {
            UnitViewModel model = new UnitViewModel();
            if (id > 0)
            {
                Unit payment = await _unitOfWork.Repository<Unit>().GetByIdAsync(id);
                model.Name = payment.Name;
            }

            return PartialView("_AddEditUnit", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditUnit(int id, UnitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditUnit", model);
            }

            if (id > 0)
            {
                Unit unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(id);
                if (unit != null)
                {
                    unit.Name = model.Name;
                    //unit.ModifiedDate = DateTime.Now;
                    //unit.Description = model.Description;
                    await _unitOfWork.Repository<Unit>().UpdateAsync(unit);
                }
            }
            else
            {
                Unit unit = new Unit
                {
                    Name = model.Name,
                    //ModifiedDate = DateTime.Now,
                    //AddedDate = DateTime.Now,
                    //Description = model.Description,
                };
                await _unitOfWork.Repository<Unit>().InsertAsync(unit);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            Unit unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(id);

            return PartialView("_DeleteUnit", unit?.Name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUnit(int id, IFormCollection form)
        {
            Unit unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(id);
            if (unit != null)
            {
                await _unitOfWork.Repository<Unit>().DeleteAsync(unit);
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            var supplier = _db.Unit.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                _db.Unit.Remove(supplier);
                _db.SaveChanges();
                result = true;
            }
            return Json(result);
        }
    }
}