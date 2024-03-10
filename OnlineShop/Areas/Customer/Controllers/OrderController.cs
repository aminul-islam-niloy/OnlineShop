using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;

        public OrderController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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
            // Retrieve the user ID of the current authenticated user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Associate the order with the user ID
            anOrder.UserId = userId;
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
            return View();
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

        //public async Task<IActionResult> UserOrders()
        //{
        //    // Retrieve the user ID of the current authenticated user
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    // Retrieve user details
        //    var user = await _userManager.FindByIdAsync(userId);

        //    // Retrieve orders associated with the user's ID
        //    var userOrders = await _db.Orders
        //        .Include(o => o.User)
        //        .Where(o => o.UserId == userId)
        //        .ToListAsync();

        //    // Create a view model to hold user information and orders
        //    var viewModel = new UserOrdersViewModel
        //    {
        //        User = (ApplicationUser)user,
        //        Orders = userOrders
        //    };

        //    return View(viewModel);
        //}

        public async Task<IActionResult> UserOrders()
        {
            // Retrieve the user ID of the current authenticated user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve user details
            var user = await _userManager.FindByIdAsync(userId);

            // Retrieve orders associated with the user's ID including order details and products
            var userOrdersWithProducts = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            // Create a view model to hold user information and orders
            var viewModel = new UserOrdersViewModel
            {
                User = (ApplicationUser)user,
                Orders = userOrdersWithProducts
            };

            return View(viewModel);
        }



        public IActionResult Index()
        {
            var ordersWithProductsAndUsers = _db.OrderDetails
            .Include(od => od.Product)
            .Include(od => od.Order)
            .ThenInclude(o => o.User) // Include user details
            .Where(od => od.Order != null && od.Product != null)
            .GroupBy(od => new
            {
                od.OrderId,
                od.Order.OrderNo,
                od.Order.Name,
                od.Order.Address,
                od.Order.Email,
                od.Order.PhoneNo,
                od.Order.OrderDate,
                od.Order.UserId, // Include user ID in grouping
                od.Order.User

            })
            .Select(g => new OrderDetailsViewModel
            {
                OrderId = g.Key.OrderId,
                OrderNo = g.Key.OrderNo,
                CustomerName = g.Key.Name,
                CustomerAddress = g.Key.Address,
                CustomerPhone = g.Key.PhoneNo,
                CustomerEmail = g.Key.Email,
                OrderDate = g.Key.OrderDate,
                UserId = g.Key.UserId, // Add user ID to the view model
                UserName = g.Key.Name,

                Products = g.Select(od => new ProductViewModel
                {
                    ProductId = od.PorductId,
                    ProductName = od.Product.Name,
                    Price = od.Product.Price,
                    Image = od.Product.Image,
                    ProductColor = od.Product.ProductColor,
                    Quantity = od.Product.Quantity
                }).ToList()
            }).ToList();

            return View(ordersWithProductsAndUsers);


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _db.OrderDetails.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _db.OrderDetails.Remove(order);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index"); // Redirect to the index page after deletion
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserOrder(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(UserOrders));
        }


    }

}
