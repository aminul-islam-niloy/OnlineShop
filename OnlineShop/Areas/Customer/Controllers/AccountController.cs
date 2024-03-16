using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {

            var user = await _userManager.FindByIdAsync(uid);
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    // Set EmailConfirmed to true
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    ViewBag.IsSuccess = true;
                }
            }

            return View();


           // return RedirectToAction("SuccessRegistration", "Account"); // or any other 
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        //don't run it
        //public async  Task <IActionResult>SetAllEmailsConfirmedAsync()
        //{
        //    // Retrieve all users from the database
        //    var users = await _userManager.Users.ToListAsync();

        //    // Iterate through each user and set EmailConfirmed to true
        //    foreach (var user in users)
        //    {
        //        user.EmailConfirmed = true;
        //        await _userManager.UpdateAsync(user);
        //    }
        //    return View();
        //}
    }
}

