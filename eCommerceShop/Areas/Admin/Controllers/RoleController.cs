using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceShop.Areas.Admin.Models;
using eCommerceShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eCommerceShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            this._roleManager = roleManager;
            this._db = db;
            this._userManager = userManager;
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            ViewBag.id = role.Id;
            ViewBag.name = role.Name;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            role.Name = name;

            var isExist = await _roleManager.RoleExistsAsync(name);

            if (isExist)
            {
                ViewBag.message = "This Role is already Exist";
                ViewBag.name = name;
                return View();
            }

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                TempData["Edit"] = "Role Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            
            if (role == null)
            {
                return NotFound();
            }


            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                TempData["Delete"] = "Role Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
               
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Assign()
        {
            var userList = await _db.ApplicationUsers.ToListAsync();
            var roleList = await _roleManager.Roles.ToListAsync();

            ViewData["UserId"] = new SelectList(userList, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(roleList, "Name", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserViewModel roleUser)
        {
            var user =await _db.ApplicationUsers.FirstOrDefaultAsync(m => m.Id.Equals(roleUser.UserId));
            //var role = _roleManager.FindByIdAsync(roleUser.RoleId);

            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);

            //var checkRoleUserExist = await _userManager.IsInRoleAsync(user, roleUser.RoleId);

            //if (!checkRoleUserExist)
            //{
            //    ViewBag.message = "This User is already assigned with this Role";
            //    return View();
            //}

            if (role.Succeeded)
            {
                TempData["Save"] = "User Role Assigned Successfully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.message = "This User is already assigned with this Role";
            var userList = await _db.ApplicationUsers.ToListAsync();
            var roleList = await _roleManager.Roles.ToListAsync();

            ViewData["UserId"] = new SelectList(userList, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(roleList, "Name", "Name");

            return View();
        }
    }
}