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
        public IActionResult OrderConfirmation()
        {
            return View();
        }



        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

        //public IActionResult Index()
        //{
        //    // Retrieve all orders from the database
        //    var orders = _db.Orders.ToList();
        //    return View(orders);
        //}

        //public IActionResult OrdersWithProducts()
        //{
        //    var ordersWithProducts = _db.OrderDetails
        //        .Include(od => od.Product)
        //        .Where(od => od.Order != null && od.Product != null)
        //        .GroupBy(od => new { od.OrderId, od.Order.OrderNo })
        //        .Select(g => new OrderDetailsViewModel
        //        {
        //            OrderId = g.Key.OrderId,
        //            OrderNo = g.Key.OrderNo,
        //            Products = g.Select(od => new ProductViewModel
        //            {
        //                ProductId = od.PorductId,
        //                ProductName = od.Product.Name,
        //                Price = od.Product.Price,
        //                Image = od.Product.Image,
        //                ProductColor = od.Product.ProductColor,
        //                Quantity = od.Product.Quantity
        //            }).ToList()
        //        })
        //        .ToList();

        //    return View(ordersWithProducts);
        //}


        public IActionResult Index()
        {
            var ordersWithProducts = _db.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order) // Include customer details
                .Where(od => od.Order != null && od.Product != null)
                .GroupBy(od => new { od.OrderId, od.Order.OrderNo, od.Order.Name, od.Order.Address,od.Order.Email,od.Order.PhoneNo,od.Order.OrderDate }) // Include customer details in the grouping
                .Select(g => new OrderDetailsViewModel
                {
                    OrderId = g.Key.OrderId,
                    OrderNo = g.Key.OrderNo,
                    CustomerName = g.Key.Name, // Customer name from grouping
                    CustomerAddress = g.Key.Address, // Customer address from grouping
                    CustomerPhone = g.Key.PhoneNo, // Customer phone from grouping
                    CustomerEmail = g.Key.Email, // Customer email from grouping
                    OrderDate = g.Key.OrderDate,
                    
                    Products = g.Select(od => new ProductViewModel
                    {
                        ProductId = od.PorductId,
                        ProductName = od.Product.Name,
                        Price = od.Product.Price,
                        Image = od.Product.Image,
                        ProductColor = od.Product.ProductColor,
                        Quantity = od.Product.Quantity
                    }).ToList()
                })
                .ToList();

            return View(ordersWithProducts);
        }

        //delete order

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _db.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            // Return a JSON response indicating success
            return Json(new { success = true });
        }


    }

}
