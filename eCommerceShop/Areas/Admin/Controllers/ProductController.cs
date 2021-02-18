using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eCommerceShop.Data;
using eCommerceShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eCommerceShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;

        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            this._db = db;
            this._he = he;
        }
        public async Task<IActionResult> Index()
        {
            var AllProducts = await _db.Products.Include(m=>m.ProductTypes)
                                                 .Include(m=>m.SpecialTags)
                                                 .ToListAsync();
            return View(AllProducts);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var productTypesList = await _db.ProductTypes.ToListAsync();
            var specialTagList = await _db.SpecialTags.ToListAsync();

            ViewData["ProductTypesId"] = new SelectList(productTypesList, "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(specialTagList, "Id", "TagName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var imgPath = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(imgPath, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    products.Image = "Images/No-image-found.jpg";
                }

                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }
    }
}