using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Stripe;
using OnlineShop.Models;

namespace OnlineShop.Models
{

    public enum MobileBanking
    {
        BKash,
        Roket,
        Nagad,
        UPay,
        Selfin
    }

    public enum PaymentMethods
    {
        CashOnDelivery,
        Card,
        MobileBanking
    }

    public enum PaymentCondition
    {
        Paid,
        Pending,
    
    }

    public enum OrderCondition
    {
        Delivered,
        Processing,
        OrderTaken
        // Add more order conditions as needed
    }

    public class OrderDetails
    {
        public int Id { get; set; }
        [Display(Name = "Order")]
        public int OrderId { get; set; }
        [Display(Name = "Product")]
        public int PorductId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [ForeignKey("PorductId")]
        public Products Product { get; set; }

        // Quantity of the product in this order detail
        public int Quantity { get; set; }

        // Price of the product in this order detail
        public decimal Price { get; set; }


        // Indicates if preservation is needed for organic products
        public bool RequiresPreservation { get; set; }
        public string StripeSessionId { get; set; }

        // Payment method used
        public PaymentMethods PaymentMethods { get; set; }

        // Payment condition
        public PaymentCondition PaymentCondition { get; set; }

        // Order condition
        public OrderCondition OrderCondition { get; set; }
    }
}


//PaymentMethods paymentMethod = PaymentMethods.MobileBanking;
//MobileBanking mobileBankingOption = MobileBanking.BKash;

