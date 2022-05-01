using CarBookData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,Staff")]
    public class CarFeatureController : Controller
    {
        private readonly AppDbContext context;

        public CarFeatureController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    
        public IActionResult Create()
        {
            ViewBag.Feauture =  new SelectList(context.Cars.OrderBy(p => p.CarName).ToList(), "Id", "CarName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CarFeature carFeature)
        {

            return View();
        }

    }
}
