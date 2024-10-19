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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        /*     public ActionResult ScheduleAppointment(string filter)
             {
                 // Set the filter type in ViewBag
                 ViewBag.FilterType = string.IsNullOrEmpty(filter) ? "Doctor" : filter;

                 IEnumerable<object> model;

                 // Fetch data based on filter
                 if (ViewBag.FilterType == "Doctor")
                 {
                     // Fetch the list of doctors from your database or service
                     // For example:
                     var doctors = _context.Doctors.ToList(); // Replace with your actual data source
                     model = doctors;
                 }
                 else
                 {
                     // Fetch the list of hospitals from your database or service
                     var hospitals = _context.Hospitals.ToList(); // Replace with your actual data source
                     model = hospitals;
                 }

                 // Pass the model to the view
                 return View(model);
             }

             */

        public ActionResult ScheduleAppointment(string filter)
        {
            // Set the filter type in ViewBag
            ViewBag.FilterType = string.IsNullOrEmpty(filter) ? "Doctor" : filter;

            IEnumerable<object> model;

            if (ViewBag.FilterType == "Doctor")
            {
                // Fetch the list of doctors along with their hospital data
                var doctors = _context.Doctors
                    .Include(d => d.Hospital) // This includes Hospital data based on HospitalId
                    .ToList();

                model = doctors;
            }
            else
            {
                // Fetch the list of hospitals
                var hospitals = _context.Hospitals.ToList();
                model = hospitals;
            }

            // Pass the model to the view
            return View(model);
        }


        public IActionResult ViewDoctors(int hospitalId)
        {
            // Fetch doctors associated with the given hospital ID
            var doctors = _context.Doctors.Where(d => d.HospitalId == hospitalId).ToList();

            // Optional: Fetch the hospital name for display
            ViewBag.HospitalName = _context.Hospitals.FirstOrDefault(h => h.Id == hospitalId)?.Name;

            return View(doctors);
        }


        [HttpPost]
        public IActionResult BookAppointment(int doctorId)
        {
            // Ensure the doctor exists
            var doctor = _context.Doctors.Find(doctorId);

            // Check if the doctor was found
            if (doctor == null)
            {
                // Optionally, handle the case where the doctor is not found
                return NotFound(); // or redirect to an error page
            }

            ViewBag.Doctor = doctor; // Pass the doctor object to the view
            return View();
        }


        [HttpPost]
        public IActionResult CreateAppointment(Appointment appointment)
        {
           
          

           
                var doctor = _context.Doctors.Find(appointment.DoctorId);
                if (doctor == null)
                {
                    ModelState.AddModelError("", "Selected doctor not found.");
                    return View("BookAppointment", appointment); 
                }

                
                int? patientId = HttpContext.Session.GetInt32("PatientId");
                if (patientId == null)
                {
                    ModelState.AddModelError("", "Patient not logged in.");
                    return View("BookAppointment", appointment);
                }
           
                appointment.PatientId = (int)patientId; 
                appointment.HospitalId = doctor.HospitalId; 
                appointment.Status = "Pending"; 
                appointment.RescheduleMessage = "Doctor needs to confirm this appointment."; 

                try
                {
                  
                    _context.Appointments.Add(appointment);
                    _context.SaveChanges();

                   
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
           
          
            
            return View("BookAppointment", appointment);
        }











    }
}
