using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceShop.Data;
using eCommerceShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            this._db = db;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            var allProductTypes = _db.ProductTypes.ToList();
            
            return View(allProductTypes);
        }

        //Create Get Action Method

        
        public IActionResult Create()
        {
            return View();
        }

        //Crete Post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["Save"] = "Product Data Saved Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        //Edit Get Action Method
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);

            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //Edit Post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Update(productTypes);
                await _db.SaveChangesAsync();
                TempData["Edit"] = "Product Data Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        //Details Get Action Method
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);

            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //Delete Get Action Method
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);

            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //Delete Post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypes _productTypes)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != _productTypes.Id)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);

            if (productType == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.ProductTypes.Remove(productType);
                await _db.SaveChangesAsync();
                TempData["Delete"] = "Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(_productTypes);
        }
    }
}