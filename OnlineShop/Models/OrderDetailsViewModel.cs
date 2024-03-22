using System;
using System.Collections.Generic;

namespace OnlineShop.Models
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public string  OrderNo { get; set; }
        public string CustomerName { get; set; } // Customer name property
        public string CustomerAddress { get; set; } // Customer address property
        public string CustomerPhone { get; set; } // Customer phone property
        public string CustomerEmail { get; set; } // Customer email property
        public DateTime OrderDate { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public string UserId { get; set; } // Include UserId property
        public string UserName { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductColor { get; set; }
        public int Quantity { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
