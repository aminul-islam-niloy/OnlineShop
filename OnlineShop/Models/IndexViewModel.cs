using System.ComponentModel.DataAnnotations;
using System;

namespace OnlineShop.Models
{
    public class IndexViewModel
    {
       
        public string FirstName { get; set; }
    
        public string LastName { get; set; }

        public int? UsernameChangeLimit { get; set; } = 10;
        public byte[] ProfilePicture { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
