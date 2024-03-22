using System.Collections.Generic;
using System;

namespace OnlineShop.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public PaymentMethods   PaymentMethods { get; set; }

        public List<ProductViewModel> Products { get; set; }
    }
}
