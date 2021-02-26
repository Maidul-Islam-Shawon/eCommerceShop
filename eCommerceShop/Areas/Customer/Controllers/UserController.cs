using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceShop.Data;
using eCommerceShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            this._userManager = userManager;
            this._db = db;
        }

        public async Task<IActionResult> Index()
        {
            var allUsersList =await _db.ApplicationUsers.ToListAsync();
            return View(allUsersList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(applicationUser, applicationUser.PasswordHash);

                if (result.Succeeded)
                {
                    TempData["Save"] = "User Created Successfully";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(m => m.Id.Equals(id));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = await _db.ApplicationUsers.FirstOrDefaultAsync(m => m.Id.Equals(user.Id));

            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;

            var userResult = await _userManager.UpdateAsync(userInfo);

            if (userResult.Succeeded)
            {
                TempData["Edit"] = "User Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in userResult.Errors)
            {
                ModelState.AddModelError(String.Empty, error.Description);
            }

        }
    }
}