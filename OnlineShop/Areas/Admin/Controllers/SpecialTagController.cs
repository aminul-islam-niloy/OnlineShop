using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Data.Migrations;
using OnlineShop.Models;
using System.Linq;
using System.Threading.Tasks;


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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTag.Add(specialTag);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(specialTag);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTag.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }

            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Models.SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _db.Update(specialTag);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(specialTag);
        }

        //GET Details Action Method

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialTag = _db.SpecialTag.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        //POST Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Models.SpecialTag specialTag)
        {
            return RedirectToAction(nameof(Index));

        }



        //GET Delete Action Method

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialTag = _db.SpecialTag.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, Models.SpecialTag specialTag)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != specialTag.Id)
            {
                return NotFound();
            }

            var specialTags = _db.SpecialTag.Find(id);
            if (specialTags == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(specialTags);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(specialTag);
        }



    }
}
