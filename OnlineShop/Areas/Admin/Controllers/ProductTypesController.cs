using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        //dbset property access and it called when we try to modify this Page
        private ApplicationDbContext _db;
        public ProductTypesController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            var data = _db.ProductTypes.ToList();
            return View(data);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task< IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        
            return View(productTypes);
        }
    }
}
