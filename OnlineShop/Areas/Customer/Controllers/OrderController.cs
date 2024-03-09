using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }


        //GET Checkout actioin method

        public IActionResult Checkout()
        {
            return View();
        }

        //POST Checkout action method

        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> Checkout(Order anOrder)
        //{
        //    List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products != null)
        //    {
        //        foreach (var product in products)
        //        {
        //            OrderDetails orderDetails = new OrderDetails();
        //            orderDetails.PorductId = product.Id;
        //            anOrder.OrderDetails.Add(orderDetails);
        //        }
        //    }

        //    anOrder.OrderNo = GetOrderNo();
        //    _db.Orders.Add(anOrder);
        //    await _db.SaveChangesAsync();
        //    HttpContext.Session.Set("products", new List<Products>());
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Checkout(Order anOrder)
        //{
        //    // Set the order date to the current date and time
        //    anOrder.OrderDate = DateTime.Now;

        //    List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products != null)
        //    {
        //        foreach (var product in products)
        //        {
        //            OrderDetails orderDetails = new OrderDetails();
        //            orderDetails.PorductId = product.Id;
        //            anOrder.OrderDetails.Add(orderDetails);
        //        }
        //    }

        //    // Set the order number
        //    anOrder.OrderNo = GetOrderNo();

        //    // Add the order to the database context
        //    _db.Orders.Add(anOrder);

        //    // Save changes to the database
        //    await _db.SaveChangesAsync();

        //    // Clear the session data
        //    HttpContext.Session.Set("products", new List<Products>());

        //    // Redirect the user or provide feedback
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            // Set the order date to the current date and time
            anOrder.OrderDate = DateTime.Now;

            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.PorductId = product.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }

            // Set the order number
            anOrder.OrderNo = GetOrderNo();

            // Add the order to the database context
            _db.Orders.Add(anOrder);

            // Save changes to the database
            await _db.SaveChangesAsync();

            // Clear the session data
            HttpContext.Session.Set("products", new List<Products>());

            // Redirect the user to the order confirmation page
            return  View();
        }



        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

      

     




    }

}
