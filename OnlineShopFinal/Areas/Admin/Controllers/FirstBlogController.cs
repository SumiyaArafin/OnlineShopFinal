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
    public class FirstBlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private IHostingEnvironment _he;

        public FirstBlogController(
            IUnitOfWork unitOfWork,
            ApplicationDbContext db,
             IHostingEnvironment he
        )
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _he = he;
        }


        public IActionResult Index()
        {
            var model = _db.FirstBlogs.ToList();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> AddEditFirstBlog(int id)
        {
            FirstBlogViewModel model = new FirstBlogViewModel();
            if (id > 0)
            {
                FirstBlogViewModel payment = await _unitOfWork.Repository<FirstBlogViewModel>().GetByIdAsync(id);
                model.Name = payment.Name;
                model.Date = payment.Date;
                model.BlogHeader = payment.BlogHeader;
                model.BlogDetails = payment.BlogDetails;
                model.BlockQuote = payment.BlockQuote;
                model.BlockQuoteDetails = payment.BlockQuoteDetails;
            }

            return PartialView("_AddEditFirstBlog", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditFirstBlog(int id, FirstBlogViewModel model, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditFirstBlog", model);
            }

            if (id > 0)
            {
                FirstBlog unit = await _unitOfWork.Repository<FirstBlog>().GetByIdAsync(id);
                if (unit != null)
                {
                    unit.Name = model.Name;
                    unit.Date = model.Date;
                    unit.BlogHeader = model.BlogHeader;
                    unit.BlogDetails = model.BlogDetails;
                    unit.BlockQuote = model.BlockQuote;
                    unit.BlockQuoteDetails = model.BlockQuoteDetails;
                    await _unitOfWork.Repository<FirstBlog>().UpdateAsync(unit);
                }
            }
            else
            {
                FirstBlog firstBlog = new FirstBlog
                {
                    Name = model.Name,

                    Date = model.Date,
                    BlogHeader = model.BlogHeader,

                    BlogDetails = model.BlogDetails,
                    BlockQuote = model.BlockQuote,

                    BlockQuoteDetails = model.BlockQuoteDetails,

                };
                await _unitOfWork.Repository<FirstBlog>().InsertAsync(firstBlog);


                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    firstBlog.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    firstBlog.Image = "images/no image found.jpeg";
                }
                await _unitOfWork.Repository<FirstBlog>().UpdateAsync(firstBlog);


            }
            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> BlogDetails(int product)
        {
            FirstBlogListViewModel pList = new FirstBlogListViewModel();
            FirstBlog pro = await _unitOfWork.Repository<FirstBlog>().GetSingleIncludeAsync(x => x.Id == product); 
            if (pro != null)
            {

                pList.Id = pro.Id;
                pList.Name = pro.Name;
                pList.BlogHeader = pro.BlogHeader;
                pList.BlogDetails = pro.BlogDetails;
                pList.BlockQuote = pro.BlockQuote;
                pList.BlockQuoteDetails = pro.BlockQuoteDetails;
                pList.Date = pro.Date;
                pList.Image = pro.Image;
                

            }
            return View(pList);
        }





        //public IActionResult BlogDetails()
        //{
        //    var model = _db.FirstBlogs.ToList();

        //    return View(model);
        //}

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            var supplier = _db.FirstBlogs.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                _db.FirstBlogs.Remove(supplier);
                _db.SaveChanges();
                result = true;
            }
            return Json(result);
        }





        [HttpGet]
        public async Task<IActionResult> EditFirstBlog(int id)
        {
            FirstBlogViewModel model = new FirstBlogViewModel();


            if (id > 0)
            {
                FirstBlog product = await _unitOfWork.Repository<FirstBlog>().GetByIdAsync(id);
                model.Name = product.Name;
                model.Date = product.Date;
                model.BlogHeader = product.BlogHeader;
                model.BlogDetails = product.BlogDetails;
                model.BlockQuote = product.BlockQuote;
                model.BlockQuoteDetails = product.BlockQuoteDetails;


            }

            return PartialView("_EditFirstBlog", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditFirstBlog(int id, FirstBlogViewModel model, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something Wrong");
                return View("_EditFirstBlog", model);
            }
            if (id > 0)
            {
                FirstBlog product = await _unitOfWork.Repository<FirstBlog>().GetByIdAsync(id);
                if (product != null)
                {
                    product.Name = model.Name;
                    product.Date = model.Date;
                    product.BlogHeader = model.BlogHeader;
                    product.BlogDetails = model.BlogDetails;
                    product.BlockQuote = model.BlockQuote;
                    product.BlockQuoteDetails = model.BlockQuoteDetails;



                    await _unitOfWork.Repository<FirstBlog>().UpdateAsync(product);
                }

            }
            else
            {
                FirstBlog product = new FirstBlog
                {

                    Name = model.Name,

                    Date = model.Date,
                    BlogHeader = model.BlogHeader,

                    BlogDetails = model.BlogDetails,
                    BlockQuote = model.BlockQuote,

                    BlockQuoteDetails = model.BlockQuoteDetails,



                };
                await _unitOfWork.Repository<FirstBlog>().InsertAsync(product);
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
                await _unitOfWork.Repository<FirstBlog>().UpdateAsync(product);


            }
            return RedirectToAction(nameof(Index));
        }
    }
}
