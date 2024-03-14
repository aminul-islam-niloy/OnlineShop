using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class UserProfileController : Controller
    {
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;
        public UserProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }


        public async Task<IActionResult> Details(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var user = await _db.ApplicationUser.FirstOrDefaultAsync(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the current user is authorized to view the details
            if (currentUser.Id != user.Id)
            {
                return Forbid(); 
            }

            return View(user);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != id)
            {
           
                return Forbid();
            }

            var user = await _db.ApplicationUser.FirstOrDefaultAsync(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user, IFormFile profilePicture)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != user.Id)
            {
             
                return Forbid();
            }


            var userInfo = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            userInfo.PhoneNumber = user.PhoneNumber;
            userInfo.Address = user.Address;
            userInfo.DateOfBirth = user.DateOfBirth;

            // Handle profile picture upload
            if (profilePicture != null && profilePicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    userInfo.ProfilePicture = memoryStream.ToArray();
                }
            }


            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["save"] = "User has been updated successfully";
                // Redirect to the dashboard with userId
                return RedirectToAction("Deshboard", new { id = user.Id });
            }
            return View(userInfo);
         
        }

        public async Task<IActionResult> Delete(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != id)
            {
               
                return Forbid();
            }

            var user = await _db.ApplicationUser.FirstOrDefaultAsync(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != user.Id)
            {
               
                return Forbid();
            }

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

        public async Task<IActionResult> Lockout(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var user = await _db.ApplicationUser.FirstOrDefaultAsync(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Lockout(ApplicationUser user)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var userInfo = await _db.ApplicationUser.FirstOrDefaultAsync(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.LockoutEnd = DateTime.Now.AddDays(1);
            await _userManager.UpdateAsync(userInfo);
          


            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["lockout"] = "User has been locked out successfully";
                // Redirect to the dashboard with userId
                return RedirectToAction("Deshboard", new { id = user.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Active(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var user = await _db.ApplicationUser.FirstOrDefaultAsync(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }



            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var userInfo = await _db.ApplicationUser.FirstOrDefaultAsync(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.LockoutEnd = null;
            await _userManager.UpdateAsync(userInfo);
           
            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["lockout"] = "User has been Active successfully";
                // Redirect to the dashboard with userId
                return RedirectToAction("Deshboard", new { id = user.Id });
            }


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deshboard(string id) 
        {

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var user = await _db.ApplicationUser.FirstOrDefaultAsync(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

           
            if (currentUser.Id != user.Id)
            {
                return Forbid();  
            }

            return View(user);
        }



    }
}
