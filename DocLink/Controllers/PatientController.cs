using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocLink.Models;
using DocLink.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DocLink.Controllers
{
    public class PatientController : Controller
    {
        private readonly DocLinkDbContext _context;

        public PatientController(DocLinkDbContext context)
        {
            _context = context;
        }

        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("PatientId") != null;
        }
        public IActionResult Dashboard()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            return View();  
        }

      
        public async Task<IActionResult> PreviousAppointments()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            var patientId = HttpContext.Session.GetInt32("PatientId");
            var appointments = await _context.Appointments
                                             .Include(a => a.Doctor)
                                             .Include(a => a.Hospital)
                                             .Where(a => a.PatientId == patientId)
                                             .ToListAsync();

            return View(appointments);  
        }

      
        public IActionResult ScheduleAppointment()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            ViewBag.Hospitals = new SelectList(_context.Hospitals, "Id", "Name");
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "FullName");
            return View(); 
        }


        [Authorize(AuthenticationSchemes = "PatientCookies")]
        public async Task<IActionResult> Index()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }
            return View(await _context.Patients.ToListAsync());
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

            var patient = await _context.Patients.FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Dashboard", "Patient");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Email == model.Email && p.Password == model.Password);

                if (patient != null)
                {
                   
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, patient.Email),
                new Claim("PatientId", patient.Id.ToString())  
            };

                    var claimsIdentity = new ClaimsIdentity(claims, "PatientCookies");
                    await HttpContext.SignInAsync("PatientCookies", new ClaimsPrincipal(claimsIdentity));


                    
                  

                    HttpContext.Session.SetInt32("PatientId", patient.Id);  

                    
                    return RedirectToAction("Dashboard", "Patient");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }




        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("PatientCookies");

          
            HttpContext.Session.Clear();

          
            return RedirectToAction("Login");
        }





        public IActionResult Signup()
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Dashboard", "Patient");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(Patient patient)
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Dashboard", "Patient");
            }

            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(patient);
        }

        public IActionResult Create()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Password,Address,PhoneNumber,Gender,Age")] Patient patient)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
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

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Password,Address,PhoneNumber,Gender,Age")] Patient patient)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }



      /*  public IActionResult ScheduleAppointments()
        {
            var model = new ScheduleAppointmentViewModel();
            ViewBag.Hospitals = new SelectList(_context.Hospitals, "Id", "Name");
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "FullName");

            return View(model);
        }

        [HttpPost]
        public IActionResult SearchAppointments(ScheduleAppointmentViewModel model)
        {
            // Initialize DoctorList to prevent null reference
            model.DoctorList = new List<DoctorViewModel>();

            if (ModelState.IsValid)
            {
                // Build the query for doctors based on the hospital and name
                var doctors = _context.Doctors.AsQueryable();

                if (model.HospitalId != 0) // Assuming 0 means 'All Hospitals'
                {
                    doctors = doctors.Where(d => d.HospitalId == model.HospitalId);
                }

                // Filtering by DoctorName if it has a value
                if (!string.IsNullOrEmpty(model.DoctorName))
                {
                    doctors = doctors.Where(d => d.FullName.Contains(model.DoctorName));
                }

                // Populate the DoctorList with the filtered results
                model.DoctorList = doctors.Select(d => new DoctorViewModel
                {
                    Id = d.Id,
                    Name = d.FullName,
                    HospitalName = d.Hospital.Name // Ensure this navigation property exists
                }).ToList();
            }

          
            ViewBag.Hospitals = new SelectList(_context.Hospitals, "Id", "Name", model.HospitalId);
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "FullName", model.DoctorId);

          
            return View("ScheduleAppointments", model);
        }

        */


    }
}
