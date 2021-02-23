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

        [HttpPost]
        public async Task<IActionResult> Index(decimal? minAmount, decimal? maxAmount)
        {
            if(minAmount==null || maxAmount == null)
            {
                var products = await _db.Products.Include(m => m.ProductTypes)
                                                 .Include(m => m.SpecialTags)
                                                 .ToListAsync();
                return View(products);
            }

            var filterdProducts = await _db.Products.Where(m => m.Price >= minAmount && m.Price <= maxAmount)
                                                .Include(m=>m.ProductTypes)
                                                .Include(m=>m.SpecialTags)
                                                .ToListAsync();

            return View(filterdProducts);
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
                var searchProduct =await _db.Products.Where(m => m.Name.Equals(products.Name)).
                                                Include(m => m.ProductTypes)
                                                .Include(m => m.SpecialTags)
                                                .FirstOrDefaultAsync();

                if (searchProduct != null)
                {
                    ViewBag.message = "This product is already exists.";
                    GetProductAndTagList();
                    return View(products);
                }

                if (image != null)
                {
                    var imgPath = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(imgPath, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    products.Image = "Images/No-image-found.JPG";
                }

                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTypesList = await _db.ProductTypes.ToListAsync();
            var specialTagList = await _db.SpecialTags.ToListAsync();

            ViewData["ProductTypesId"] = new SelectList(productTypesList, "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(specialTagList, "Id", "TagName");

            var Product =await _db.Products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products products, IFormFile image)
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
                    products.Image = "Images/No-image-found";
                }


                _db.Products.Update(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(products);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTypesList = await _db.ProductTypes.ToListAsync();
            var specialTagList = await _db.SpecialTags.ToListAsync();

            ViewData["ProductTypesId"] = new SelectList(productTypesList, "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(specialTagList, "Id", "TagName");

            var product =await _db.Products.Where(m => m.Id.Equals(id))
                                      .Include(m => m.ProductTypes)
                                      .Include(m => m.SpecialTags)
                                      .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTypesList = await _db.ProductTypes.ToListAsync();
            var specialTagList = await _db.SpecialTags.ToListAsync();

            ViewData["ProductTypesId"] = new SelectList(productTypesList, "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(specialTagList, "Id", "TagName");

            var product = await _db.Products.Where(m => m.Id.Equals(id))
                                      .Include(m => m.ProductTypes)
                                      .Include(m => m.SpecialTags)
                                      .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id, Products products)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != products.Id)
            {
                return NotFound();
            }

            var product = _db.Products.Find(id);

            if (product==null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public void GetProductAndTagList()
        {
            var productTypesList =  _db.ProductTypes.ToList();
            var specialTagList =  _db.SpecialTags.ToList();

            ViewData["ProductTypesId"] = new SelectList(productTypesList, "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(specialTagList, "Id", "TagName");
        }
    }
}