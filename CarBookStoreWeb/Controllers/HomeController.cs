using CarBookData;
using CarBookStoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private  const string entityName = "";
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var model = context.Cars.OrderBy(q => q.CarName).ToList();
            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }
    }
}
