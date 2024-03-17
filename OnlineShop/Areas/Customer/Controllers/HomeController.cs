﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Service;
using OnlineShop.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
  
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IEmailService _emailService;

        public HomeController(ApplicationDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }


        //public async Task<ViewResult> Index(int? page)
        //{

            //UserEmailOptions options = new UserEmailOptions
            //{
            //    ToEmails = new List<string>() { "heyalex4@gmail.com" },
            //    PlaceHolders = new List<KeyValuePair<string, string>>()
            //    {
            //        new KeyValuePair<string, string>("{{UserName}}", "Alex")
            //    }
            //};

            //await _emailService.SendTestEmail(options);
        //}

           public IActionResult Index(int? page)
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList().ToPagedList(page ?? 1, 8));
        }


        ////POST Index action method
        //[HttpPost]
        //public IActionResult Index( string searchString,int?page)
        //{
        //    var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList().ToPagedList(page ?? 1, 8); 

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        // Filter products based on search string
        //        products = products.Where(p => p.Name.Contains(searchString)).ToList().ToList().ToPagedList(page ?? 1, 8);
        //    }

        //    return View(products);
        //}

        [HttpPost]
        public IActionResult Index(string searchString, int? page)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Filter products based on search string
                products = products.Where(p => p.Name.Contains(searchString)).ToList();
            }

            // Convert the filtered products to a paged list using the current page number
            var pagedProducts = products.ToPagedList(page ?? 1, 8);

            return View(pagedProducts);
        }



        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }




        //GET product detail acation method

        public ActionResult Detail(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ActionName("Detail")]
        public ActionResult ProductDetail(int? id, int quantityInCart)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            // Check if the requested quantity can be added to the cart
            if (product.Quantity >= quantityInCart)
            {
                // Update the database quantity
                product.Quantity -= quantityInCart;
                _db.SaveChanges(); // Save changes to the database

                // Update the session cart
                List<Products> products = HttpContext.Session.Get<List<Products>>("products");
                if (products == null)
                {
                    products = new List<Products>();
                }

                // Find if the product is already in the cart
                var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    existingProduct.QuantityInCart += quantityInCart;
                }
                else
                {
                    // Add the product to the cart
                    product.QuantityInCart = quantityInCart;
                    products.Add(product);
                }

                // Update the session
                HttpContext.Session.Set("products", products);
            }
            else
            {
                return NotFound();
               
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Customer")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var productToRemove = products.FirstOrDefault(p => p.Id == id);
                if (productToRemove != null)
                {
                    // Update the database quantity
                    var productInDb = _db.Products.FirstOrDefault(p => p.Id == id);
                    if (productInDb != null)
                    {
                        productInDb.Quantity += productToRemove.QuantityInCart;
                    }

                    // Remove the product from the cart
                    products.Remove(productToRemove);

                    // Update the session
                    HttpContext.Session.Set("products", products);
                    _db.SaveChanges(); // Save changes to the database
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]


        [Authorize(Roles = "Customer")]
        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }


  


        //GET product Cart action method

            [Authorize(Roles = "Customer")]
        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }




        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Order(int id)
        {
            var order = _db.Orders.Include(o => o.OrderDetails)
                                   .ThenInclude(od => od.Product)
                                   .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }






    }
}
