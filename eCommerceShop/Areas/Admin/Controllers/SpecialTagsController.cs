using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceShop.Data;
using eCommerceShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SpecialTagsController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<IActionResult> Index()
        {
            var AllTags =await _db.SpecialTags.ToListAsync();
            return View(AllTags);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTags specialTags)
        {
            if (!ModelState.IsValid)
            {
                return View(specialTags);
            }
            _db.SpecialTags.Add(specialTags);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Tag =await _db.SpecialTags.Where(m => m.Id.Equals(id)).FirstOrDefaultAsync();

            if (Tag == null)
            {
                return NotFound();
            }

            return View(Tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTags specialTags)
        {
            if (!ModelState.IsValid)
            {
                return NoContent();
            }

            _db.SpecialTags.Update(specialTags);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Tag = await _db.SpecialTags.Where(m => m.Id.Equals(id)).FirstOrDefaultAsync();

            if (Tag == null)
            {
                return NotFound();
            }

            return View(Tag);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Tag = await _db.SpecialTags.Where(m => m.Id.Equals(id)).FirstOrDefaultAsync();

            if (Tag == null)
            {
                return NotFound();
            }

            return View(Tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTags _specialTags)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != _specialTags.Id)
            {
                return NotFound();
            }

            var specialTag = _db.SpecialTags.Find(id);

            if (specialTag == null)
            {
                return NotFound();
            }

            _db.SpecialTags.Remove(specialTag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}