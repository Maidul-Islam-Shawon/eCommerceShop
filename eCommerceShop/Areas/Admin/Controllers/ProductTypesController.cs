using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            var allProductTypes = _db.ProductTypes.ToList();
            
            return View(allProductTypes);
        }
    }
}