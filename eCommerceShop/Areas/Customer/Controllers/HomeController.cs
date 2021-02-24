using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eCommerceShop.Models;
using eCommerceShop.Data;
using Microsoft.EntityFrameworkCore;
using eCommerceShop.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace eCommerceShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            this._db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var allProducts = await _db.Products.Include(m => m.ProductTypes)
                                                .Include(m => m.SpecialTags)
                                                .ToListAsync();

            //...for pagination...//
            var allProductsWithPaged = allProducts.ToPagedList(page ?? 1, 6);

            return View(allProductsWithPaged);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTypesList = await _db.ProductTypes.ToListAsync();
            ViewData["ProductTypesId"] = new SelectList(productTypesList, "Id", "ProductType");

            var product =await _db.Products.Include(m => m.ProductTypes)
                                      .Include(m => m.SpecialTags)
                                      .FirstOrDefaultAsync(m=>m.Id.Equals(id));

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> ProductDetails(int? id)
        {
            List<Products> products = new List<Products>(); 

            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products.Include(m => m.ProductTypes)
                                      .Include(m => m.SpecialTags)
                                      .FirstOrDefaultAsync(m => m.Id.Equals(id));

            if (product == null)
            {
                return NotFound();
            }
            
            products = HttpContext.Session.Get<List<Products>>("products");

            if (products == null)
            {
                products = new List<Products>();
            }

            products.Add(product);
            HttpContext.Session.Set("products", products);

            return View(product);
        }

        [HttpGet]
        [ActionName("Remove")]
        public IActionResult RemoveFromCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");

            if (products != null)
            {
                var product = products.FirstOrDefault(m => m.Id.Equals(id));

                if (products != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }

            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");

            if (products != null)
            {
                var product = products.FirstOrDefault(m => m.Id.Equals(id));

                if (products != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Cart()
        {
             List<Products> products = HttpContext.Session.Get<List<Products>>("products");

            if (products == null)
            {
                products = new List<Products>();
            }

            return View(products);
        }
    }
}
