using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopFinal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.ViewComponents
{
    public class CategoryViewComponent: ViewComponent
    {
        private ApplicationDbContext _db;
        public CategoryViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult>InvokeAsync()
        {
            var item = await _db.Categories.ToListAsync();
            return View(item);
        }
    }
}
