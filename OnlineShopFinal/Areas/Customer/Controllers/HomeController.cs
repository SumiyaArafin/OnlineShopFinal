using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OnlineShopFinal.Code;
using OnlineShopFinal.Data;
using Microsoft.AspNetCore.Mvc;
using OnlineShopFinal.Models;

using OnlineShopFinal.Repository;
using OnlineShopFinal.ViewModel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
//using X.PagedList;
using cloudscribe.Pagination.Models;


namespace OnlineShopFinal.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private ApplicationDbContext _db;


        public HomeController(
            IUnitOfWork unitOfWork,
            ApplicationDbContext db
        )
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }


        public IActionResult Index()
        {
            ViewBag.NewOrder = _db.Orders.ToList()
                                   .Where(i => i.Status == false).ToList();
            ViewBag.TotalProducts = _db.Product.ToList();
            ViewBag.TotalCustomer = _db.ApplicationUsers.ToList();
            ViewBag.TotalCategory = _db.Categories.ToList();
            var LatestProducts = GetLatestProduct();

            var frontPageElementss = GetFrontPageElements();
            var product = Getproducts();
            var firstBlogs = GetFirstBlog();

            IndexVM model = new IndexVM();
            model.FrontPageElementss = frontPageElementss;
            model.Products = product;
            model.PurchaseOrderLineItems = GetPurchaseOrderLineItem();
            model.FirstBlogs = firstBlogs;
            model.LatestProducts = LatestProducts;
            return View(model);
        }

        private List<PurchaseOrderLineItem> GetLatestProduct()
        {
            return (_db.PurchaseOrderLineItems.Include(c => c.Product).Where(c => c.Product.Date >= DateTime.Today)).ToList();
        }

        private List<Product> Getproducts()
        {
            return (_db.Product.Include(c => c.Category).Include(c => c.SubCategory).ToList());
        }
        private List<PurchaseOrderLineItem> GetPurchaseOrderLineItem()
        {
            var lineItem = _db.PurchaseOrderLineItems.Include(c=>c.Product).Where(c => c.Quantity > 0);
            foreach (var item in lineItem)
            {
                item.Status = true;
            }
            _db.SaveChanges();
            foreach (var item in lineItem)
            {
                foreach (var item1 in lineItem)
                {
                    if (item1.ProductId==item.ProductId && item.Status==true &&item1.Status==true && item.Id!=item1.Id)
                    {
                        if (item.ComparePrice>item1.ComparePrice)
                        {
                            item.Status = false;
                        }
                        else
                        {
                            item1.Status = false;
                        }
                    }
                }
            }
            _db.SaveChanges();
            var finalPruchase = _db.PurchaseOrderLineItems.Where(c => c.Quantity > 0 && c.Status == true).ToList();
            return finalPruchase;
        }


        private List<FrontPageElement> GetFrontPageElements()
        {
            return (_db.FrontPageElements.ToList());
        }

        private List<FirstBlog> GetFirstBlog()
        {
            return (_db.FirstBlogs.ToList());
        }




        //public IActionResult Index(int pageNumber=1,int pageSize=6)
        //{
        //    ViewBag.NewOrder = _db.Orders.ToList()
        //                           .Where(i => i.Status == false).ToList();
        //    ViewBag.TotalProducts = _db.Product.ToList();
        //    ViewBag.TotalCustomer = _db.ApplicationUsers.ToList();
        //    ViewBag.TotalCategory = _db.Categories.ToList();

        //    return View(_db.Product.Include(c => c.Category).Include(c => c.SubCategory).ToList());
        //}



        public IActionResult Shop(int pageNumber = 1, int pageSize = 8)
        {
           
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var Bikes = _db.PurchaseOrderLineItems.Include(c => c.Product)
                 .Skip(ExcludeRecords)
                 .Take(pageSize);
            var result = new PagedResult<PurchaseOrderLineItem>
            {
                Data = Bikes.AsNoTracking().ToList(),
                TotalItems = _db.PurchaseOrderLineItems.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize

            };
            return View(result);

            
        }


        //public IActionResult Index2(int? page)
        //{
        //    ViewBag.NewOrder = _db.Orders.ToList()
        //                           .Where(i => i.Status == false).ToList();
        //    ViewBag.TotalProducts = _db.Product.ToList();
        //    ViewBag.TotalCustomer = _db.ApplicationUsers.ToList();

        //    ViewBag.TotalCategory = _db.Categories.ToList();
        //    return View(_db.Product.Include(c => c.Category).Include(c => c.SubCategory).ToList().ToPagedList(pageNumber: page ?? 1, pageSize: 9));
        //}


        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [ActionName("Details")]
        public async Task<IActionResult> Details(int id)
        {
            ProductListViewModel pList = new ProductListViewModel();
            PurchaseOrderLineItem pro = _db.PurchaseOrderLineItems.Include(c=>c.Product).ThenInclude(c=>c.Category).ThenInclude(c=>c.SubCategories).Include(c => c.Product.ProductImages).Where(c => c.Id == id).FirstOrDefault();
            //Product pro = await _unitOfWork.Repository<Product>().GetSingleIncludeAsync(x => x.Id == product., b => b.SubCategory, c => c.Category, u => u.Unit, pi => pi.ProductImages); ;
            
            if (pro != null)
            {

                pList.Id = pro.Id;
                pList.Name = pro.Product.Name;
                pList.AvailablePrice = pro.UnitPrice;
                //pList.PreviousPrice = pro.PreviousPrice;
                pList.CategoryId = pro.Product.CategoryId;
                pList.SubCategoryId = pro.Product.SubCategoryId;
                //pList.UnitId = pro.Product.UnitId;
                pList.Description = pro.Product.Description;
                pList.Size = pro.Product.Size;
                pList.SizeUnite = pro.Product.SizeUnite;
                pList.ProductColor = pro.Product.ProductColor;
                pList.Quantity = pro.Quantity;
                pList.Subtotal = pro.Subtotal;
                pList.Image = pro.Product.Image;

                var productImageList = pro.Product.ProductImages.Where(x => x.ProductId == pro.Id).ToList();
                //pList.CategoryName = pro.Product.Category.Name;
                //pList.SubCategoryName = pro.Product.SubCategory.Name;
                //pList.UnitName = pro.Product.Unit.Name;

                pList.TotalImage = pro.Product.ProductImages.Count(x => x.ProductId == pro.ProductId);


                pList.ImageList = new List<ProductImageListViewModel>();


                var productImage = productImageList.FirstOrDefault();
                if (productImage != null)
                {
                    pList.ImageTitle = productImage.Title;
                    pList.ImagePath = productImage.ImagePath;
                    productImageList.ForEach(x =>
                    {
                        ProductImageListViewModel image = new ProductImageListViewModel
                        {
                            Id = x.Id,
                            ImagePath = x.ImagePath,
                            Title = x.Title,
                            ProductId = x.ProductId
                        };
                        pList.ImageList.Add(image);
                    });
                }

            }
            return View(pList);
        }



       // Post product Details Action method

        [HttpPost]
        [ActionName("Details")]
        public ActionResult ProductDetails(int? id,PurchaseOrderLineItem produc)
        {
            List<LineItemViewModel> products = new List<LineItemViewModel>();
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.PurchaseOrderLineItems.Include(c=>c.Product).FirstOrDefault(c => c.Id == id);
            product.Quantity = produc.Quantity;
            LineItemViewModel lineItemViewItem = new LineItemViewModel();
            lineItemViewItem.Id = product.Id;
            lineItemViewItem.ProductId = product.Id;
            lineItemViewItem.ProductName = product.Product.Name;
            lineItemViewItem.Quantity = product.Quantity;
            lineItemViewItem.UnitPrice = product.UnitPrice;
            lineItemViewItem.SalePrice = product.SalePrice;
            lineItemViewItem.ComparePrice = product.ComparePrice;
            lineItemViewItem.Unit = product.Unit;
            lineItemViewItem.ProductTax = product.ProductTax;
            lineItemViewItem.Discount = product.Discount;
            lineItemViewItem.Subtotal = product.Quantity * product.UnitPrice;
            lineItemViewItem.PurchaseOrderId = product.PurchaseOrderId;
            lineItemViewItem.Status = product.Status;
            lineItemViewItem.image = product.Product.Image;
            
            if (product == null)
            {
                return NotFound();
            }
            products = HttpContext.Session.Get<List<LineItemViewModel>>("products");
            if (products == null)
            {
                products = new List<LineItemViewModel>();
            }
            products.Add(lineItemViewItem);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }


        //Get Remove Action Method
        [ActionName("Remove")]
        public ActionResult RemoveToCart(int? id)
        {
            List<LineItemViewModel> products = HttpContext.Session.Get<List<LineItemViewModel>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public ActionResult Remove(int? id)
        {
            List<LineItemViewModel> products = HttpContext.Session.Get<List<LineItemViewModel>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }




        [HttpGet("/Home/Cart")]

        public ActionResult Cart()
        {
            List<LineItemViewModel> products = HttpContext.Session.Get<List<LineItemViewModel>>("products");
            if (products == null)
            {
                products = new List<LineItemViewModel>();

            }
            
            return View(products);
        }
        [HttpPost("/Home/Cart")]
        public ActionResult Cart(List<LineItemViewModel> CartItem , string number1, string number2, string number3)
        {
            int qt = Convert.ToInt32(number1);
            decimal sbtotal = Convert.ToDecimal(number2);
            int id = Convert.ToInt32(number3);
            if (number1 == null && number2 == null && number3 == null)
            {
                List<LineItemViewModel> products = HttpContext.Session.Get<List<LineItemViewModel>>("products");
                foreach (var i in CartItem)
                {
                    var item = products.SingleOrDefault(X => X.Id.Equals(i.Id));
                    item.Quantity = i.Quantity;
                    item.Subtotal = i.Subtotal;
                    HttpContext.Session.Set("products", products);

                }
                return RedirectToAction("Checkout", "Order", new { area = "Customer" });


            }
            else
            {
                List<LineItemViewModel> products = HttpContext.Session.Get<List<LineItemViewModel>>("products");
                var item = products.SingleOrDefault(X => X.Id.Equals(id));
                item.Quantity = qt;
                item.Subtotal = sbtotal;
                HttpContext.Session.Set("products", products);
                return RedirectToAction("Cart", "Home", new { area = "Customer" });
            }
        }
        [HttpGet("/Home/GetData")]
        public ActionResult GetData(int? id)
        {
            var Quantity = _db.PurchaseOrderLineItems.Where(c => c.Id == id).FirstOrDefault();
            return Json(Quantity.Quantity);

        }

        [HttpPost]
        public IActionResult Update(int[] quantity)
        {

            List<PurchaseOrderLineItem> addProductList = HttpContext.Session.Get<List<PurchaseOrderLineItem>>("products");

            for (int i = 0; i < addProductList.Count(); i++)
            {
                addProductList[i].Quantity = quantity[i];
            }

            HttpContext.Session.Set("products", addProductList);
            return RedirectToAction("Cart");

        }


        public async Task<IActionResult> QuickViewProduct(int product)
        {
            ProductListViewModel pList = new ProductListViewModel();
            Product pro = await _unitOfWork.Repository<Product>().GetSingleIncludeAsync(x => x.Id == product, b => b.SubCategory, c => c.Category, u => u.Unit, pi => pi.ProductImages); ;
            if (pro != null)
            {

                pList.Id = pro.Id;
                pList.Name = pro.Name;
                pList.AvailablePrice = pro.AvailablePrice;
                pList.PreviousPrice = pro.PreviousPrice;
                pList.CategoryId = pro.CategoryId;
                pList.SubCategoryId = pro.SubCategoryId;
                pList.UnitId = pro.UnitId;
                pList.Description = pro.Description;
                pList.Size = pro.Size;
                pList.SizeUnite = pro.SizeUnite;
                pList.ProductColor = pro.ProductColor;
                pList.Quantity = pro.Quantity;
                pList.CategoryName = pro.Category.Name;
                pList.SubCategoryName = pro.SubCategory.Name;
                pList.UnitName = pro.Unit.Name;
                pList.Image = pro.Image;

                pList.TotalImage = pro.ProductImages.Count(x => x.ProductId == pro.Id);

                var productImageList = pro.ProductImages.Where(x => x.ProductId == pro.Id).ToList();

                var productImage = productImageList.FirstOrDefault();
                if (productImage != null)
                {
                    pList.ImageTitle = productImage.Title;
                    pList.ImagePath = productImage.ImagePath;

                }

            }
            return PartialView("_QuickView", pList);
        }



        public async Task<IActionResult> GetCategoryDetails(String name)
        {
            var SubCategoryList = await _db.Product.Where(x => x.Name == name).ToListAsync();
            return View(SubCategoryList);
        }






        //#region ProductCart

        //[HttpGet]
        //public async Task<IActionResult> AddToCart(int product)
        //{

        //    if (product > 0)
        //    {
        //        Models.Product pro = await _unitOfWork.Repository<Product>().GetByIdAsync(product);
        //        if (pro != null)
        //        {
        //            AddToCartViewModel addTo = new AddToCartViewModel
        //            {
        //                ProductId = pro.Id,
        //                ProductName = pro.Name,
        //                FinalPrice = pro.AvailablePrice,
        //                Quantity = 1
        //            };
        //            ViewBag.productId = product;
        //            return PartialView("_AddtoCart", addTo);
        //        }
        //    }

        //    return PartialView("_AddtoCart");
        //}

        //[HttpPost]
        //public IActionResult AddToCart(int prodcut, AddToCartViewModel model)
        //{
        //    List<AddToCartViewModel> addToCart = new List<AddToCartViewModel>();
        //    List<AddToCartViewModel> addToCartList = HttpContext.Session.Get<List<AddToCartViewModel>>("products");
        //    int itemInCart = 0;
        //    var valu = HttpContext.Session.GetInt32("itemCount");
        //    if (valu != null)
        //    {
        //        itemInCart = valu.Value;
        //    }

        //    if (addToCartList == null)
        //    {
        //        AddToCartViewModel cart = new AddToCartViewModel();
        //        cart.ProductId = model.ProductId;
        //        cart.FinalPrice = model.FinalPrice;
        //        cart.ProductName = model.ProductName;
        //        cart.Quantity = model.Quantity;
        //        addToCart.Add(cart);
        //        HttpContext.Session.Set<List<AddToCartViewModel>>("products", addToCart);
        //        itemInCart = 1;
        //    }
        //    else
        //    {

        //        if (addToCartList.Where(x => x.ProductId == model.ProductId).ToList().Count < 1)
        //        {
        //            AddToCartViewModel cart = new AddToCartViewModel();
        //            cart.ProductId = model.ProductId;
        //            cart.FinalPrice = model.FinalPrice;
        //            cart.ProductName = model.ProductName;
        //            cart.Quantity = model.Quantity;
        //            addToCartList.Add(cart);
        //            HttpContext.Session.Set<List<AddToCartViewModel>>("products", addToCartList);
        //            itemInCart = HttpContext.Session.Get<List<AddToCartViewModel>>("products").Count;
        //        }
        //    }


        //    HttpContext.Session.SetInt32("itemCount", itemInCart);
        //    return RedirectToAction(nameof(Index));
        //}



        //[HttpGet]
        //public IActionResult RemoveCartItem(int product)
        //{
        //    return View();
        //}
        //#endregion



    }
}
