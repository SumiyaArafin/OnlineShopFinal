using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OnlineShopFinal.Data;
using OnlineShopFinal.Models;
using OnlineShopFinal.Repository;
using OnlineShopFinal.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineShop.Controllers.Admin
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hosingEnv;
        private readonly ApplicationDbContext _db;
        public ProductController(
            IUnitOfWork unitOfWork,
            ApplicationDbContext db,
            IHostingEnvironment hosingEnv
        )
        {
            _hosingEnv = hosingEnv;
            _db = db;
            _unitOfWork = unitOfWork;

        }

        public async Task<IActionResult> Index(string search)
        {
            List<ProductListViewModel> model = new List<ProductListViewModel>();
            var dbData = await _unitOfWork.Repository<Product>().GetAllIncludeAsync(b => b.SubCategory, c => c.Category,
                u => u.Unit, /*com => com.ProductCommentses,*/ pi => pi.ProductImages/*, ps => ps.ProductStocks*/);

            if (!String.IsNullOrEmpty(search))
            {
                dbData = dbData.Where(x =>
                      x.Name.ToLower().Contains(search.ToLower()) ||
                     
                      x.Description.ToLower().Contains(search.ToLower())

                ).ToList();

            }

            foreach (var b in dbData)
            {
                ProductListViewModel product = new ProductListViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    AvailablePrice = b.AvailablePrice,
                    PreviousPrice = b.PreviousPrice,
                    ProductColor = b.ProductColor,
                    Image = b.Image,
                    Quantity = b.Quantity,
                    CategoryId = b.CategoryId,
                    SubCategoryId = b.SubCategoryId,
                    UnitId = b.UnitId,
                    Description = b.Description,
                    Size = b.Size,
                    SizeUnite = b.SizeUnite,
                    Date = b.Date,

                    SubCategoryName = b.SubCategory.Name,
                    CategoryName = b.Category?.Name,
                    UnitName = b.Unit.Name,
                   
                    //ProductComments = b.ProductCommentses.Count,
                    TotalImage = b.ProductImages.Count
                };
                //var prdctStocks = b.ProductStocks.FirstOrDefault();
                //product.ProductStocks = prdctStocks != null ? product.ProductStocks = prdctStocks.InQuantity - prdctStocks.OutQuantity : product.ProductStocks = 0;

                model.Add(product);
            }

            return View(model);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    ProductViewModel model = new ProductViewModel();
        //    //return View(ModelVM);
        //    return PartialView("_AddEditProduct", model);
        //}

        [HttpGet]
        public async Task<IActionResult> AddEditProduct(int id)
        {
            ProductViewModel model = new ProductViewModel();
            model.SubCategoryList = _unitOfWork.Repository<SubCategory>().GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.CategoryList = _unitOfWork.Repository<Category>().GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.UnitList = _unitOfWork.Repository<Unit>().GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (id > 0)
            {
                Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                model.Name = product.Name;
                model.AvailablePrice = product.AvailablePrice;
                model.Image = product.Image;
                model.PreviousPrice = product.PreviousPrice;
                model.CategoryId = product.CategoryId;
                model.SubCategoryId = product.SubCategoryId;
                model.UnitId = product.UnitId;
                model.Description = product.Description;
                model.Quantity = product.Quantity;
                model.Size = product.Size;
                model.SizeUnite = product.SizeUnite;
                model.ProductColor = product.ProductColor;

                model.Date = product.Date;

            }

            return PartialView("_AddEditProduct", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditProduct(int id, ProductViewModel model,IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something Wrong");
                return View("_AddEditProduct", model);
            }
            if (id > 0)
            {
                Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                if (product != null)
                {
                    product.Name = model.Name;
                    product.AvailablePrice = model.AvailablePrice;
                    product.PreviousPrice = model.PreviousPrice;
                    product.CategoryId = model.CategoryId;
                    product.SubCategoryId = model.SubCategoryId;
                    product.UnitId = model.UnitId;
                    product.Description = model.Description;
                    product.ProductColor = model.ProductColor;
                    product.Image = model.Image;
                    product.Quantity = model.Quantity;
                    product.Size = model.Size;
                    product.SizeUnite = model.SizeUnite;
                    product.Date = model.Date;


                    await _unitOfWork.Repository<Product>().UpdateAsync(product);
                }

            }
            else
            {
                Product product = new Product
                {
                  
                    Name = model.Name,
                    AvailablePrice = model.AvailablePrice,
                    PreviousPrice = model.PreviousPrice,
                    ProductColor = model.ProductColor,
                    Quantity = model.Quantity,
                    Image = model.Image,
                    CategoryId = model.CategoryId,
                    SubCategoryId = model.SubCategoryId,
                    UnitId = model.UnitId,
                    Description = model.Description,
                    Size = model.Size,
                    SizeUnite = model.SizeUnite,
                    Date = model.Date,
                    
                };
                await _unitOfWork.Repository<Product>().InsertAsync(product);
                if (image != null)
                {
                    var name = Path.Combine(_hosingEnv.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    model.Image = "images/no image found.jpeg";
                }
                await _unitOfWork.Repository<Product>().UpdateAsync(product);
                if (model.Images.Count() > 0)
                {
                    await UploadProductImages(model.Images, product.Name, product.Id);
                }

                //if (model.Manual != null)
                //{
                //    await UploadProductManual(model.Manual, product.Id, product.Name);
                //}
            }
            return RedirectToAction(nameof(Index));
        }


        private async Task UploadProductImages(IList<IFormFile> list, string productName, int productId)
        {
            foreach (var file in list)
            {
                if (file != null && (file.ContentType == "image/png" || file.ContentType == "image/jpg" || file.ContentType == "image/jpeg"))
                {
                    var productImage = new ProductImage
                    {
            
                        ProductId = productId,
                        Title = productName
                    };
                    var uploads = Path.Combine(_hosingEnv.WebRootPath, "uploads/ProductImages");
                    var fileName = Path.Combine(uploads, productName + "_" + Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal)) + ".png");
                    //Model
                    productImage.ImagePath = Path.GetFileName(fileName);

                    await _unitOfWork.Repository<ProductImage>().InsertAsync(productImage);

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
        }


         public JsonResult LoadState(int Id)
        {
            var state = _db.SubCategories.Where(a => a.Category.Id == Id).ToList();
            return Json(new SelectList(state, "Id", "Name"));
        }


        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            return PartialView("_DeleteProduct", product?.Name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id, IFormCollection form)
        {
            Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product != null)
            {
                await _unitOfWork.Repository<Product>().DeleteAsync(product);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ListImageView(int id)
        {
            ProductImageListByProduct productImage = new ProductImageListByProduct();
            //productImage.ProuctImages = GetProdutcsImages(id);
            productImage.Path = productImage.ProuctImages.Max(x => x.ImagePath);

            //return PartialView("_ShowImageByProduct", productImage);
            return View();
        }

        public PartialViewResult GetProdutcsImages(int id)
        {
            List<ProductImageListViewModel> productImageList = new List<ProductImageListViewModel>();
            ViewBag.productName = _unitOfWork.Repository<Product>().GetById(id).Name;
            _unitOfWork.Repository<ProductImage>().GetAll().Where(x => x.ProductId == id).ToList().ForEach(x =>
            {
                ProductImageListViewModel pImage = new ProductImageListViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ImagePath = x.ImagePath,
                    Title = x.Title,
                    ProductName = _unitOfWork.Repository<Product>().GetById(id).Name
                };
                productImageList.Add(pImage);
            });
            return PartialView("_ShowImageByProduct", productImageList);

        }



        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            var supplier = _db.Product.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                _db.Product.Remove(supplier);
                _db.SaveChanges();
                result = true;
            }
            return Json(result);
        }





        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            ProductViewModel model = new ProductViewModel();
            model.SubCategoryList = _unitOfWork.Repository<SubCategory>().GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.CategoryList = _unitOfWork.Repository<Category>().GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.UnitList = _unitOfWork.Repository<Unit>().GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (id > 0)
            {
                Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                model.Name = product.Name;
                model.AvailablePrice = product.AvailablePrice;
                model.Image = product.Image;
                model.PreviousPrice = product.PreviousPrice;
                model.CategoryId = product.CategoryId;
                model.SubCategoryId = product.SubCategoryId;
                model.UnitId = product.UnitId;
                model.Description = product.Description;
                model.Quantity = product.Quantity;
                model.Size = product.Size;
                model.SizeUnite = product.SizeUnite;
                model.ProductColor = product.ProductColor;

                model.Date = product.Date;

            }

            return PartialView("EditProduct", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(int id, ProductViewModel model, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something Wrong");
                return View("EditProduct", model);
            }
            if (id > 0)
            {
                Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                if (product != null)
                {
                    product.Name = model.Name;
                    product.AvailablePrice = model.AvailablePrice;
                    product.PreviousPrice = model.PreviousPrice;
                    product.CategoryId = model.CategoryId;
                    product.SubCategoryId = model.SubCategoryId;
                    product.UnitId = model.UnitId;
                    product.Description = model.Description;
                    product.ProductColor = model.ProductColor;
                    product.Image = model.Image;
                    product.Quantity = model.Quantity;
                    product.Size = model.Size;
                    product.SizeUnite = model.SizeUnite;
                    product.Date = model.Date;


                    await _unitOfWork.Repository<Product>().UpdateAsync(product);
                }

            }
            else
            {
                Product product = new Product
                {

                    Name = model.Name,
                    AvailablePrice = model.AvailablePrice,
                    PreviousPrice = model.PreviousPrice,
                    ProductColor = model.ProductColor,
                    Quantity = model.Quantity,
                    Image = model.Image,
                    CategoryId = model.CategoryId,
                    SubCategoryId = model.SubCategoryId,
                    UnitId = model.UnitId,
                    Description = model.Description,
                    Size = model.Size,
                    SizeUnite = model.SizeUnite,
                    Date = model.Date,

                };
                await _unitOfWork.Repository<Product>().InsertAsync(product);
                if (image != null)
                {
                    var name = Path.Combine(_hosingEnv.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    model.Image = "images/no image found.jpeg";
                }
                await _unitOfWork.Repository<Product>().UpdateAsync(product);
                if (model.Images.Count() > 0)
                {
                    await UploadProductImages(model.Images, product.Name, product.Id);
                }

                //if (model.Manual != null)
                //{
                //    await UploadProductManual(model.Manual, product.Id, product.Name);
                //}
            }
            return RedirectToAction(nameof(Index));
        }







    }
}