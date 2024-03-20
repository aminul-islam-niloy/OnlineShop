using System.Collections.Generic;

namespace OnlineShop.Models
{
    public class ProductDetailViewModelHome
    {
        public Products SpecificProduct { get; set; }
        public List<Products> RelatedProducts { get; set; }
    }
}
