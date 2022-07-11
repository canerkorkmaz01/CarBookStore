using CarBookData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

       
        public IActionResult Car()
        {
            var cars = context.Cars.OrderBy(q => q.CarName).ToList();
            return View(cars);
        }

        public IActionResult CarDetails(int id)
        {
            var model =  context.Cars.Find(id);
            var features = model.Features.ToList();
            ViewBag.CarFeatures = context.Features.OrderBy(p => p.Name).ToList().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name, Selected = features.Any(q => q.Id == p.Id) });
            return View(model);
        }
    }
}
