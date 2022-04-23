using CarBookData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Controllers
{
    
    public class CarController : Controller
    {
     

       

        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult Cars()
        {
            return View();
        }
    }
}
