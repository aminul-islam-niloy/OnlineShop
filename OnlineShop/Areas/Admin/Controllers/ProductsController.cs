﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
     [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {

        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductsController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).ToList());
        }

        //POST Index action method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            }

            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    // Filter products based on search string
            //    products = products.Where(p => p.Name.Contains(searchString)).ToList();
            //}

            //if (string.IsNullOrEmpty(searchString))
            //{
            //    // Filter products based on search string
            //    products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            //}


            return View(products);
        }


        //Get Create method
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Products product, List<IFormFile> ImagesSmall)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "This product already exists";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "Name");
                    return View(product);
                }

                if (ImagesSmall != null && ImagesSmall.Count > 0)
                {
                    product.ImagesSmall = new List<ProductImage>(); // Initialize the collection

                    foreach (var image in ImagesSmall)
                    {
                        try
                        {
                            var imagePath = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));

                            // Ensure the directory exists
                            Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                            using (var fileStream = new FileStream(imagePath, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }

                            // Add the image path to the product's images collection
                            product.ImagesSmall.Add(new ProductImage { ImagePath = "Images/" + image.FileName });
                        }
                        catch (Exception ex)
                        {
                            // Log any exceptions that occur
                            // This can help diagnose the issue further
                            Console.WriteLine($"Error copying image: {ex.Message}");
                        }
                    }
                }
                else
                {
                    product.ImagesSmall = new List<ProductImage>(); // Ensure collection is initialized
                }

                // Check if there are any images in ImagesSmall collection
                if (product.ImagesSmall != null && product.ImagesSmall.Any())
                {
                    // Set the Image property to the path of the first image
                    product.Image = product.ImagesSmall.First().ImagePath;
                }

                // Add the product to the database
                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                TempData["save"] = "Product has been added";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }



        //GET Edit Action Method

        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                .FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Products product, List<IFormFile> ImagesSmall)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Clear existing images to avoid conflicts
                    product.ImagesSmall = new List<ProductImage>();

                    if (ImagesSmall != null && ImagesSmall.Count > 0)
                    {
                        foreach (var image in ImagesSmall)
                        {
                            try
                            {
                                // Construct the image path
                                var imagePath = Path.Combine(_he.WebRootPath, "Images", Path.GetFileName(image.FileName));

                                // Ensure the directory exists
                                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                                // Copy the image to the destination path
                                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }

                                // Add the image to the product's collection
                                product.ImagesSmall.Add(new ProductImage { ImagePath = "Images/" + image.FileName });
                            }
                            catch (Exception ex)
                            {
                                // Log any exceptions that occur while copying images
                                Console.WriteLine($"Error copying image: {ex.Message}");
                            }
                        }
                    }

                    // Check if there are any images in ImagesSmall collection
                    if (product.ImagesSmall != null && product.ImagesSmall.Any())
                    {
                        // Set the Image property to the path of the first image
                        product.Image = product.ImagesSmall.First().ImagePath;
                    }

                    // Update other product details as needed
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();
                    TempData["save"] = "Product has been updated";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log any unexpected exceptions
                    Console.WriteLine($"Error editing product: {ex.Message}");
                    TempData["error"] = "An unexpected error occurred while editing the product.";
                }
            }

            // If ModelState is not valid, return the view with the product object
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "Name");
            return View(product);
        }







        //GET Details Action Method
        public ActionResult Details(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products
                .Include(p => p.ProductTypes)
                .Include(p => p.SpecialTag)
                .Include(p => p.ImagesSmall) // Include the collection of images
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }



        //GET Delete Action Method

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.SpecialTag).Include(c => c.ProductTypes).Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST Delete Action Method

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["save"] = "Product has been Deleted";
            return RedirectToAction(nameof(Index));
        }



    }
}
