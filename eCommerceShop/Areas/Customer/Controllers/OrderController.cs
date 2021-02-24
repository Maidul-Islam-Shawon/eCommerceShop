using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceShop.Data;
using eCommerceShop.Models;
using eCommerceShop.Utility;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");

            if (products != null)
            {
                foreach(var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    anOrder.OrderDetails.Add(orderDetails);

                }
            }
            anOrder.OrderNo = GetOrderNumber();
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();

            HttpContext.Session.Set("products", new List<Products>());

            return View();
        }


        public string GetOrderNumber()
        {
            int rowCount = _db.Orders.ToList().Count()+1;
            return rowCount.ToString("000");

        }
    }
}