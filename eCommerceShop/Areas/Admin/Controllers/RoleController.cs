using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            ViewBag.Roles = roles;

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (name == null)
            {
                return NoContent();
            }

            var isExist = await _roleManager.RoleExistsAsync(name);

            if (!isExist)
            {
                IdentityRole role = new IdentityRole();
                role.Name = name;

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["Save"] = "Product Data Saved Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.message = "This Role is already Exist";
            ViewBag.name = name;
            return View();
        }
    }
}