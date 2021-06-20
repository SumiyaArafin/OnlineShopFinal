using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BahokBdDelivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using static BahokBdDelivery.Helper;

namespace OnlineShopFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DesignationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DesignationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var designations = await _context.Designations.Where(c => c.IsActive == true).ToListAsync();
            return View(designations);
        }
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id)
        {
            if (id == 0)
                return View(new Designation());
            else
            {
                var designationModel = await _context.Designations.FindAsync(id);
                if (designationModel == null)
                {
                    return NotFound();
                }
                return View(designationModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Designation designationModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    designationModel.IsActive = true;
                    _context.Add(designationModel);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        var result = await _context.Designations.FindAsync(id);
                        result.Name = designationModel.Name;
                        result.Salary = designationModel.Salary;                       
                        result.IsActive = true;
                        _context.Update(result);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {

                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Designations.Where(c => c.IsActive == true).ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", designationModel) });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var designation = await _context.Designations.FindAsync(id);
            designation.IsActive = false;
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Designations.Where(c => c.IsActive == true).ToList()) });
        }
    }
}
