using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}