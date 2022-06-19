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
        private const string entityName="";
        private readonly AppDbContext context;

        public CarController(AppDbContext context)
        {
            this.context = context; 
        }
     
        public IActionResult Index()
        {
            var carsList = context.Cars.OrderBy(q => q.CarName).ToList();
            return View(carsList);
        }

       
        public IActionResult Cars()
        {
            return View();
        }
    }
}
