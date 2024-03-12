using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Session;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
  
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }



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
        //   [Authorize(Roles = "Customer")]
        //POST product detail acation method
        //[HttpPost]
        //[ActionName("Detail")]
        //public ActionResult ProductDetail(int? id,int quantityInCart)
        //{
        //    List<Products> products = new List<Products>();
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products == null)
        //    {
        //        products = new List<Products>();
        //    }

        //    if (product.Quantity >= product.QuantityInCart + quantityInCart)
        //    {
        //        // Increase the quantity in the cart
        //        product.QuantityInCart += quantityInCart;
        //        // Decrease the available quantity
        //        product.Quantity -= quantityInCart;
        //        _db.SaveChanges(); // Save changes to the database

        //    }

        //    products.Add(product);
        //    HttpContext.Session.Set("products", products);
        //    return RedirectToAction(nameof(Index));
        //}

        ////  [Authorize(Roles = "Customer")]
        ////GET Remove action methdo


        ////   [ActionName("Remove")]

        //[HttpPost]
        //public IActionResult RemoveToCart(int? id)
        //{
        //    List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products != null)
        //    {
        //        var productToRemove = products.FirstOrDefault(p => p.Id == id);
        //        if (productToRemove != null)
        //        {
        //            // Increase the available quantity of the product
        //            productToRemove.Quantity += productToRemove.QuantityInCart;

        //            // Update the product quantity in the database
        //            _db.Entry(productToRemove).State = EntityState.Modified;

        //            // Remove the product from the cart
        //            products.Remove(productToRemove);

        //            // Update the session cart
        //            HttpContext.Session.Set("products", products);

        //            _db.SaveChanges(); // Save changes to the database
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}



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
                // Handle the case where requested quantity exceeds available quantity
                // You can redirect with a message indicating insufficient stock
            }

            return RedirectToAction(nameof(Index));
        }

        
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

        [HttpPost]

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


        //[HttpPost]
        //[Authorize(Roles = "Customer")]
        //public IActionResult Remove(int? id)
        //{
        //    List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products != null)
        //    {
        //        var product = products.FirstOrDefault(c => c.Id == id);
        //        if (product != null)
        //        {
        //            products.Remove(product);
        //            HttpContext.Session.Set("products", products);
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        //   [HttpPost]
        //public IActionResult Remove(int? id)
        //{
        //    List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        //    if (products != null)
        //    {
        //        var productToRemove = products.FirstOrDefault(c => c.Id == id);
        //        if (productToRemove != null)
        //        {
        //            // Increase the product quantity in the total products
        //            var totalProduct = _db.Products.FirstOrDefault(p => p.Id == id);
        //            if (totalProduct != null)
        //            {
        //                totalProduct.Quantity += productToRemove.QuantityInCart;
        //                _db.SaveChanges(); // Save changes to the database
        //            }

        //            products.Remove(productToRemove);
        //            HttpContext.Session.Set("products", products);
        //            _db.SaveChanges(); // Save changes to the database
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}



        //GET product Cart action method

        //    [Authorize(Roles = "Customer")]
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
