using System;
using System.Collections.Generic;

namespace OnlineShop.Models
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; } // Customer name property
        public string CustomerAddress { get; set; } // Customer address property
        public string CustomerPhone { get; set; } // Customer phone property
        public string CustomerEmail { get; set; } // Customer email property
        public DateTime OrderDate { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
