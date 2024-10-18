using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DocLink.Models;
using DocLink.Repository;
using DocLink.ViewModel;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;


namespace DocLink.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHospitalRepository _hospitalRepository;

        public HomeController(ILogger<HomeController> logger , IHospitalRepository hospitalRepository)
        {
            _logger = logger;
            _hospitalRepository = hospitalRepository;
        }

       
        [HttpGet("")] 
        [HttpGet("index")] 
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

     
        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet("home/hospitals")]
        public IActionResult Hospitals()
        {
            IEnumerable<Hospital> hospitals = _hospitalRepository.GetHospitals();
            return View(hospitals);
        }
    }
}


