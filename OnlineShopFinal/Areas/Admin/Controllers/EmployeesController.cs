using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BahokBdDelivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using static BahokBdDelivery.Helper;

namespace OnlineShopFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        //ajax call data 
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Include(c=>c.Designation).Where(c => c.IsActive == true).ToListAsync();
            return View(employees);
        }
        [HttpGet("/Employees/GellAllDesignation")]
        public IActionResult GellAllDesignation()
        {
            var designation = _context.Designations.ToList();
            return Json(designation);
        }
        [HttpGet("/Employees/GetDesignationSalary")]
        public IActionResult GetDesignationSalary(int id)
        {
            var employeeSalary = _context.Designations.Where(c=>c.Id==id).FirstOrDefault();
            return Json(employeeSalary.Salary);
        }
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id)
        {
            if (id == 0)
            {
                ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
                return View(new Employee());
            }
            else
            {
                var employeeModel = await _context.Employees.FindAsync(id);
                if (employeeModel == null)
                {
                    return NotFound();
                }
                ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
                return View(employeeModel);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id,  Employee employeeModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    employeeModel.IsActive = true;
                    _context.Add(employeeModel);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        var employee = _context.Employees.Include(c=>c.Designation).Where(c => c.Id == id).FirstOrDefault();
                        employee.FristName = employeeModel.FristName;
                        employee.LastName = employeeModel.LastName;
                        employee.JoiningDate = employeeModel.JoiningDate;
                        employee.Salary = employeeModel.Salary;
                        employee.EmployeeType = employeeModel.EmployeeType;
                        employee.PermanentAddress = employeeModel.PermanentAddress;
                        employee.Email = employeeModel.Email;
                        employee.PresentAddress = employeeModel.PresentAddress;
                        employee.ContactNo = employeeModel.ContactNo;
                        employee.EmergencyContactNo = employeeModel.EmergencyContactNo;
                        employee.EmployeeStatus = employeeModel.EmployeeStatus;
                        employee.IsActive = true;
                        employee.DesignationId = employeeModel.DesignationId;

                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                       
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Employees.Include(c=>c.Designation).Where(c => c.IsActive == true).ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", employeeModel) });
        }
        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            employee.IsActive = false;
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Employees.Include(c=>c.Designation).Where(c => c.IsActive == true).ToList()) });
        }


    }
}
