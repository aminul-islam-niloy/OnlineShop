using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int? UsernameChangeLimit { get; set; } = 10;
        public byte[] ProfilePicture { get; set; }

        public DateTime? DateOfBirth { get; set; } // Add date of birth property
        public string Address { get; set; } // Add address property

    }
}
