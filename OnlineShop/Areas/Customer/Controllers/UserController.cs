using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var dd = _userManager.GetUserId(HttpContext.User);
            return View(_db.ApplicationUser.ToList());
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists
                var existingUser = await _userManager.FindByNameAsync(user.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Username already exists.");
                    return View(user);
                }

                // Proceed with user creation if the username is unique
                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (result.Succeeded)
                {
                    TempData["save"] = "User has been created successfully";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["save"] = "User has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);


        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();

            }
            _db.ApplicationUser.Remove(userInfo);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User has been delete successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }


        public async Task<IActionResult> Locout(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Locout(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();

            }
            userInfo.LockoutEnd = DateTime.Now.AddDays(3);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["locout"] = "User has been lockout successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        public async Task<IActionResult> Active(string id)
        {
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();

            }
            //userInfo.LockoutEnd = DateTime.Now.AddDays(-1);
            userInfo.LockoutEnd = null;
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["locout"] = "User has been active successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }



    }
}
