using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocLink.Models;
using System.Linq;

namespace DocLink.Controllers
{
    public class HospitalController : Controller
    {
        private readonly DocLinkDbContext _context;

        public HospitalController(DocLinkDbContext context)
        {
            _context = context;
        }

      
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetString("HospitalEmail") != null;
        }

        public IActionResult Signup()
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Hospital");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup([Bind("Name, Address, Phone, Email, Password")] Hospital hospital)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hospital);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hospital = await _context.Hospitals
                .FirstOrDefaultAsync(h => h.Email == email && h.Password == password);

            if (hospital != null)
            {
                HttpContext.Session.SetString("HospitalEmail", hospital.Email);
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Invalid login credentials.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Index()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            return View(await _context.Hospitals.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return View(hospital);
        }

        private bool HospitalExists(int id)
        {
            return _context.Hospitals.Any(e => e.Id == id);
        }
    }
}
