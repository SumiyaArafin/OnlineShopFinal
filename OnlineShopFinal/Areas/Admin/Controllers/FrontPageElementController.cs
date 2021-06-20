using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using OnlineShopFinal.Repository;
using OnlineShopFinal.ViewModel;

namespace OnlineShopFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FrontPageElementController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private IHostingEnvironment _he;

        public FrontPageElementController(
            IUnitOfWork unitOfWork,
            ApplicationDbContext db,
             IHostingEnvironment he
        )
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _he = he;
        }
     

        public IActionResult Create()
        {
            return View();
        }
        

        public IActionResult Index()
        {
            var model = _db.FrontPageElements.ToList();
            
            return View(model);
        }
       

        [HttpGet]
        public async Task<IActionResult> AddEditFrontPageElement(int id)
        {
            FrontPageElementViewModel model = new FrontPageElementViewModel();
            if (id > 0)
            {
                FrontPageElementViewModel payment = await _unitOfWork.Repository<FrontPageElementViewModel>().GetByIdAsync(id);
                model.Name = payment.Name;
            }

            return PartialView("_AddEditFrontPageElement", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditFrontPageElement(int id, FrontPageElementViewModel model, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditFrontPageElement", model);
            }

            if (id > 0)
            {
                FrontPageElement unit = await _unitOfWork.Repository<FrontPageElement>().GetByIdAsync(id);
                if (unit != null)
                {
                    unit.Name = model.Name;
                    unit.Description = model.Description;
                    unit.LatestNewsDescription = model.LatestNewsDescription;
                    await _unitOfWork.Repository<FrontPageElement>().UpdateAsync(unit);
                }
            }
            else
            {
                FrontPageElement frontPageElement = new FrontPageElement
                {
                    Name = model.Name,

                    Description = model.Description,
                    LatestNewsDescription=model.LatestNewsDescription,
                };
                await _unitOfWork.Repository<FrontPageElement>().InsertAsync(frontPageElement);


                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    frontPageElement.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    frontPageElement.Image = "images/no image found.jpeg";
                }
                await _unitOfWork.Repository<FrontPageElement>().UpdateAsync(frontPageElement);


            }
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            var supplier = _db.FrontPageElements.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                _db.FrontPageElements.Remove(supplier);
                _db.SaveChanges();
                result = true;
            }
            return Json(result);
        }






        [HttpGet]
        public async Task<IActionResult> EditFrontPageElement(int id)
        {
            FrontPageElementViewModel model = new FrontPageElementViewModel();
            

            if (id > 0)
            {
                FrontPageElement product = await _unitOfWork.Repository<FrontPageElement>().GetByIdAsync(id);
                model.Name = product.Name;
                model.Description = product.Description;
                model.Image = product.Image;
                model.LatestNewsDescription = product.LatestNewsDescription;
                

            }

            return PartialView("_EditFrontPageElement", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditFrontPageElement(int id, FrontPageElementViewModel model, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something Wrong");
                return View("_EditFrontPageElement", model);
            }
            if (id > 0)
            {
                FrontPageElement product = await _unitOfWork.Repository<FrontPageElement>().GetByIdAsync(id);
                if (product != null)
                {
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.LatestNewsDescription = model.LatestNewsDescription;
                    product.Image = model.Image;
                   


                    await _unitOfWork.Repository<FrontPageElement>().UpdateAsync(product);
                }

            }
            else
            {
                FrontPageElement product = new FrontPageElement
                {

                    Name = model.Name,
                    Description = model.Description,
                    LatestNewsDescription = model.LatestNewsDescription,
                    Image = model.Image,
                    

                };
                await _unitOfWork.Repository<FrontPageElement>().InsertAsync(product);
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    model.Image = "images/no image found.jpeg";
                }
                await _unitOfWork.Repository<FrontPageElement>().UpdateAsync(product);
               
                
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
