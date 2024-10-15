using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DocLink.Models;



/*

namespace DocLink.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Route for the Index action
        [HttpGet("")] // Matches /home or /home/index
        [HttpGet("index")] // Matches /home/index
        public IActionResult Index()
        {
            return View();
        }

        // Route for the Privacy action
        [HttpGet("privacy")] // Matches /home/privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Route for the Error action
        [HttpGet("error")] // Matches /home/error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}



*/