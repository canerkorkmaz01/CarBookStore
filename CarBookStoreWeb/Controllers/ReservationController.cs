using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Controllers
{
    public class ReservationController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reservation()
        {
            return View();
        }
    }
}
