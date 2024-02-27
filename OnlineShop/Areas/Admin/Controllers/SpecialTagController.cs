using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using System.Linq;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private ApplicationDbContext _db;

        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;   
        }


        public IActionResult Index()
        {
            var data = _db.SpecialTag.ToList();
            return View(data);
           
        }
    }
}
