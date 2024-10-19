using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DocLink;
using DocLink.Models;
using DocLink.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DocLink.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DocLinkDbContext _context;
        
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("DoctorId") != null;
        }

        public DoctorController(DocLinkDbContext context)
        {
            _context = context;
        }

     
        [Authorize(AuthenticationSchemes = "DoctorCookies")]
        public async Task<IActionResult> Index()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            return View();

        }
        
        [HttpGet]
        public IActionResult Login()
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Dashboard", "Doctor");
            }
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorEmail == model.Email && d.password == model.Password);
            if (doctor != null)
            {
                HttpContext.Session.SetString("DoctorEmail", doctor.DoctorEmail);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, doctor.DoctorEmail) ,
                    new Claim("DoctorId", doctor.Id.ToString())
                };
                
                var claimsIdentity = new ClaimsIdentity(claims, "DoctorCookies");
                await HttpContext.SignInAsync("DoctorCookies", new ClaimsPrincipal(claimsIdentity));
                
                HttpContext.Session.SetInt32("DoctorId", doctor.Id);  
                return RedirectToAction("Dashboard", "Doctor");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = "DoctorCookies")]
        public  IActionResult Dashboard()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }
            var doctorId = HttpContext.Session.GetInt32("DoctorId");
            var doctor = _context.Doctors.Find(doctorId);
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.Patient != null)
                .ToList();

            
            var viewModel = new DoctorDashboardViewModel(doctor, appointments);
            
            return View(viewModel);
        }
        
        
        
        

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        public IActionResult Create()
        {
            ViewBag.Hospitals = new SelectList(_context.Hospitals, "Id", "Name");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Specialization,Hospital")] Doctor doctor, int hospitalId)
        {
            if (ModelState.IsValid)
            {
                doctor.Hospital = await _context.Hospitals.FindAsync(hospitalId);

                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Hospitals = new SelectList(_context.Hospitals, "Id", "Name");
            return View(doctor);
        }




        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Specialization")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
