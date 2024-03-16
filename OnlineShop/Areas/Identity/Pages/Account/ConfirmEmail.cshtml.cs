using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace OnlineShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string uid, string code)
        {
            if (uid == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(uid);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{uid}'.");
            }


            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(code))
            {
                code = code.Replace(' ', '+');
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    // Set EmailConfirmed to true
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
            }

            return Page();
        }
    }
}
