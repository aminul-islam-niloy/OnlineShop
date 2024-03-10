using System.Collections.Generic;

namespace OnlineShop.Models
{
    public class UserOrdersViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Order> Orders { get; set; }
    }
}
