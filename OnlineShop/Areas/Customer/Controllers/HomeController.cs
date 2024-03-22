
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Service;
using OnlineShop.Session;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly IMemoryCache _cache;

        public HomeController(ApplicationDbContext db, IEmailService emailService, IMemoryCache memoryCache)
        {
            _db = db;
            _emailService = emailService;
            _cache = memoryCache;
        }

        public async Task<IActionResult> Index(int? page)
        {
            // Check if the data is already cached
            if (!_cache.TryGetValue("products", out IEnumerable<Products> products))
            {
                // Data is not in cache, so retrieve it from the database
                products = await _db.Products
                    .Include(p => p.ProductTypes)
                    .Include(p => p.SpecialTag)
                    .ToListAsync();

                // Cache the data for 5 minutes
                _cache.Set("products", products, TimeSpan.FromMinutes(5));
            }

            // You can use the cached data here
            ViewData["productTypeSearchId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");

            var viewModel = new IndexPageViewModel
            {
                products = (List<Products>)products,
                Laptop = (List<Products>)GetCachedProductsByCategory("Laptop"),
                Phone = (List<Products>)GetCachedProductsByCategory("Mobile"),
                Furniture = (List<Products>)GetCachedProductsByCategory("Furniture"),
                Camera = (List<Products>)GetCachedProductsByCategory("Camera"),
                TvandMonitor = (List<Products>)GetCachedProductsByCategory("Tv and Monitor"),
                Car = (List<Products>)GetCachedProductsByCategory("Car"),
                bike = (List<Products>)GetCachedProductsByCategory("Bike"),
                Hardware = (List<Products>)GetCachedProductsByCategory("Hardware")
            };

            return View(viewModel);
        }

        // Helper method to retrieve cached products by category
        private IEnumerable<Products> GetCachedProductsByCategory(string category)
        {
            if (!_cache.TryGetValue(category, out IEnumerable<Products> cachedProducts))
            {
                // Data is not in cache, so retrieve it from the database
                cachedProducts = _db.Products
                    .Where(p => p.ProductTypes.ProductType == category)
                    .Include(p => p.ProductTypes)
                    .Include(p => p.SpecialTag)
                    .ToList();

                // Cache the data for 5 minutes
                _cache.Set(category, cachedProducts, TimeSpan.FromMinutes(5));
            }

            return cachedProducts;
        }



        //public IActionResult Index(int? page)
        //{

        //    ViewData["productTypeSearchId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");

        //    return View(_db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList().ToPagedList(page ?? 1, 12));
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
            var pagedProducts = products.ToPagedList(page ?? 1, 12);

            return View(pagedProducts);
        }

        // GET: Customer/Home/Products
        public IActionResult Products(int? page)
        {
            ViewData["productTypeSearchId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            // Check if products are already cached
            if (!_cache.TryGetValue("AllProducts", out IPagedList<Products> cachedProducts))
            {
                // Products not found in cache, retrieve them from the database
                var products = _db.Products.Include(p => p.ProductTypes).ToList().ToPagedList(page ?? 1, 12);

                // Cache the products for 5 minutes
                _cache.Set("AllProducts", products, TimeSpan.FromMinutes(5));

                return View(products);
            }

            // Products found in cache, return them
            return View(cachedProducts);
        }


        // POST: Filter products based on search criteria
        [HttpPost]
        public IActionResult Products(int? page, int productTypeId, string searchString, decimal? lowAmount, decimal? largeAmount, string sortOrder)
        {
            // Remove cached products to ensure fresh results are fetched
            _cache.Remove("AllProducts");

            ViewData["productTypeSearchId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");

            // Fetch minimum and maximum prices from the database
            var minPrice = _db.Products.Min(p => p.Price);
            var maxPrice = _db.Products.Max(p => p.Price);

            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;

            var productsQuery = _db.Products.Include(p => p.ProductTypes).AsQueryable();

            // Filter by product type
            if (productTypeId != 0)
            {
                productsQuery = productsQuery.Where(p => p.ProductTypes.Id == productTypeId);
            }

            // Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchString));
            }

            // Filter by price range
            if (lowAmount.HasValue && largeAmount.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= lowAmount && p.Price <= largeAmount);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "PriceLowToHigh":
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case "PriceHighToLow":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                default:
                    break;
            }

            // Retrieve and cache the filtered products
            var filteredProducts = productsQuery.ToPagedList(page ?? 1, 12);
            _cache.Set("AllProducts", filteredProducts, TimeSpan.FromMinutes(5));

            return View(filteredProducts);
        }



        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }



        public ActionResult Detail(int? id)
        {
            ViewData["productTypeSearchId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");

            if (id == null)
            {
                return NotFound();
            }

            var specificProduct = _db.Products
                .Include(p => p.ProductTypes)
                .Include(p => p.ImagesSmall)  // Include images
                .FirstOrDefault(c => c.Id == id);

            if (specificProduct == null)
            {
                return NotFound();
            }

            // Query related products ( products of the same category)
            var relatedProducts = _db.Products
                .Where(p => p.ProductTypeId == specificProduct.ProductTypeId && p.Id != specificProduct.Id)
                .Take(12) // Assuming  want to display 12 related products
                .ToList();

            var viewModel = new ProductDetailViewModelHome
            {
                SpecificProduct = specificProduct,
                RelatedProducts = relatedProducts
            };

            return View(viewModel);
        }





        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ActionName("Detail")]
        public ActionResult ProductDetail(int? id, int quantityInCart)
        {
            ViewData["productTypeSearchId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");

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

            ViewData["productTypeSearchId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");

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
