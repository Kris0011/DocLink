using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocLink.Models;
using System.Linq;
using DocLink.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using DocLink.ViewModel;
using Microsoft.AspNetCore.Authorization;



namespace DocLink.Controllers
{
    public class HospitalController : Controller
    {
        private readonly DocLinkDbContext _context;
        
        private readonly IHospitalRepository _hospitalRepository;


        public HospitalController(DocLinkDbContext context , IHospitalRepository hospitalRepository)
        {

            _context = context;
            _hospitalRepository = hospitalRepository;
        }


               [HttpPost]
        public IActionResult AddDoctor(Doctor doctor)
        {
           
            string hospitalEmail = HttpContext.Session.GetString("HospitalEmail");

            if (!string.IsNullOrEmpty(hospitalEmail))
            {
              
                var hospital = _context.Hospitals.FirstOrDefault(h => h.Email == hospitalEmail);

                if (hospital != null)
                {
                    
                    doctor.HospitalId = hospital.Id;

                    if (ModelState.IsValid)
                    {
                       
                        _context.Doctors.Add(doctor);
                        _context.SaveChanges();

                       
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    
                    ModelState.AddModelError(string.Empty, "Hospital not found.");
                }
            }
            else
            {

                ModelState.AddModelError(string.Empty, "No hospital email in session.");
            }


            return View("Index");
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



        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.GetInt32("PatientId") != null)
            {
                return RedirectToAction("Dashboard", "Patient");
            }
            if(HttpContext.Session.GetInt32("HospitalEmail") !=null)
            {
                return RedirectToAction("Index", "Hospital");


            }
            if(HttpContext.Session.GetInt32("DoctorId") != null)
            {
                return RedirectToAction("DashBoard", "Doctor");
            }

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

                 var claims = new List<Claim>
         {
             new Claim(ClaimTypes.Name, hospital.Email),
             new Claim("HospitalId", hospital.Id.ToString())
         };

                 var claimsIdentity = new ClaimsIdentity(claims,"HospitalCookies");
                 await HttpContext.SignInAsync("HospitalCookies", new ClaimsPrincipal(claimsIdentity));

                 return RedirectToAction("Index");
             }

             ViewBag.ErrorMessage = "Invalid login credentials.";
             return View();
         }
       

      



        public async Task<IActionResult> Logout()
        {
            // Sign out the user from the authentication scheme
            await HttpContext.SignOutAsync("HospitalCookies");

            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the Login page
            return RedirectToAction("Login");
        }


        [Authorize(AuthenticationSchemes = "HospitalCookies")]
        public async Task<IActionResult> Index()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login");
            }
        
           
            var hospitalEmail = HttpContext.Session.GetString("HospitalEmail");
        
           
            var hospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.Email == hospitalEmail);
           
        
           
            return View();
        }

        [Authorize(AuthenticationSchemes = "PatientCookies")]
        [HttpGet("Hospital/{HopitalId}/Doctors")]
        public IActionResult Doctors(int HopitalId)
        {
            IEnumerable<Doctor> doctors = _hospitalRepository.GetDoctorByHospitalId(HopitalId);
            Console.WriteLine("Inside Doctors");

            for (int i = 0; i < doctors.Count(); i++)
            {
                Doctor doctor = doctors.ElementAt(i);
                Console.WriteLine(doctor.FullName);  
            }
            DoctorsViewModel doctorsVM = new DoctorsViewModel();
            doctorsVM._doctors = doctors;
            doctorsVM._hospital = _hospitalRepository.GetHospitalById(HopitalId);
            return View(doctorsVM);
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
