using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Foreign key to Products table
        [ForeignKey("ProductId")]
        public Products Product { get; set; }
        public string ImagePath { get; set; }
    }
}
