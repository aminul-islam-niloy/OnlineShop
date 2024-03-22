using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Payment;
using OnlineShop.Session;
using Stripe.Checkout;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
  
    public class OrderController : Controller


    {

        private readonly StripeSettings _stripeSettings;

        public string SessionId { get; set; }
    
        private ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;

        public OrderController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IOptions<StripeSettings> stripeSettings)
        {
            _db = db;
            _userManager = userManager;
            _stripeSettings = stripeSettings.Value;
        }


        //GET Checkout actioin method

        public IActionResult Checkout()
        {
            return View();
        }

        //POST Checkout action method

        [Authorize(Roles = "Customer")]
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
                    OrderDetails orderDetails = new OrderDetails
                    {
                        PorductId = product.Id,
                        Price = product.Price,  // Assuming product.Price represents the unit price
                        Quantity = product.QuantityInCart
                    };
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
            return RedirectToAction("PaymentPage", new { orderId = anOrder.Id });
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }

        //Success?orderId={orderId}"

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

      


        [Authorize(Roles = "Admin")]
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
                    od.Order.User.UserName,
               
                    // Include username
                    // Include email
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
                    UserName = g.Key.UserName,
                    PaymentMethods = g.First().PaymentMethods,


                    Products = g.Select(od => new ProductViewModel
                    {
                        ProductId = od.PorductId,
                        ProductName = od.Product.Name,
                        Price = od.Product.Price,
                        Image = od.Product.Image,
                        ProductColor = od.Product.ProductColor,
                        Quantity = od.Quantity // Get quantity from OrderDetails
                    }).ToList(),

                    // Calculate the total price by summing the prices of all products in the order
                    TotalPrice = g.Sum(od => od.Product.Price * od.Quantity)
                }).ToList();

            return View(ordersWithProductsAndUsers);
        }




        [Authorize(Roles = "Admin")]
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



        [Authorize(Roles = "Customer")]
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

            // Map the data to view model
            var viewModel = userOrdersWithProducts.Select(order => new UserOrdersViewModel
            {
                UserName = user.UserName, 
                UserPhone = user.PhoneNumber, 
                OrderNo = order.OrderNo,
                OrderDate = order.OrderDate,
                TotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.Price),
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailsViewModel
                {
                    ProductId = od.PorductId,
                    ProductName = od.Product.Name,
                    ProductImage = od.Product.Image,
                    ProductColor = od.Product.ProductColor,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    PaymentMethods = od.PaymentMethods,
                    
                }).ToList()
            }).ToList();

            return View(viewModel);
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserOrder(string id)
        {
           
            int orderId;
            if (!int.TryParse(id, out orderId))
            {
                // Handle invalid id format
                return BadRequest();
            }

            var order = await _db.Orders.FirstOrDefaultAsync(o => o.OrderNo == id);
            if (order == null)
            {
                return NotFound();
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(UserOrders));
        }

    
        //stripe payment method

        //public IActionResult CreatePayment(string amount)
        //{

        //    var currency = "usd"; // Currency code
        //    var successUrl = "https://localhost:44343/Customer/Order";
        //    var cancelUrl = "https://localhost:44343/Customer/Order/Checkout";
        //    StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        //    var options = new SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string>
        //        {
        //            "card"
        //        },
        //        LineItems = new List<SessionLineItemOptions>
        //        {
        //            new SessionLineItemOptions
        //            {
        //                PriceData = new SessionLineItemPriceDataOptions
        //                {
        //                    Currency = currency,
        //                    UnitAmount = Convert.ToInt32(amount) * 100,  // Amount in smallest currency unit (e.g., cents)
        //                    ProductData = new SessionLineItemPriceDataProductDataOptions
        //                    {
        //                        Name = "Product Name",
        //                        Description = "Product Description"
        //                    }
        //                },
        //                Quantity = 1
        //            }
        //        },
        //        Mode = "payment",
        //        SuccessUrl = successUrl,
        //        CancelUrl = cancelUrl
        //    };

        //    var service = new SessionService();
        //    var session = service.Create(options);
        //    SessionId = session.Id;

        //    return Redirect(session.Url);
        //}

        //public async Task<IActionResult> success()
        //{

        //    return View("Index");
        //}

        //public IActionResult cancel()
        //{
        //    return View();
        //}


        [HttpPost]
        public IActionResult CreatePayment(int orderId)
        {
            // Retrieve the order from the database
            var order = _db.Orders.Include(o => o.OrderDetails)
                                  .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var totalAmount = order.OrderDetails.Sum(od => od.Price * od.Quantity);

            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            // Create the Stripe session
            var sessionService = new SessionService();
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(totalAmount), 
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Order Payment"
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = "https://localhost:44343/Customer/Order/UserOrders",
                CancelUrl = "https://localhost:44343/Customer/Order/PaymentPage"
            };


            var session = sessionService.Create(options);

            // Store the session ID in the order details
            order.OrderDetails.ForEach(od =>
            {
                od.StripeSessionId = session.Id;
                od.PaymentMethods = PaymentMethods.Card; 
            });

 
            _db.SaveChanges();


            return Redirect(session.Url);
        }


        //  PaymentPage

        public IActionResult PaymentPage(int orderId)
        {
            // Pass orderId to the view
            ViewBag.OrderId = orderId;
            return View();
        }




    }

}


