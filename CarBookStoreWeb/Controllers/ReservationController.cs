using CarBookData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Controllers
{
    public class ReservationController : Controller
    {
        private const string EntityName= "";
        private readonly AppDbContext context;

        public ReservationController(AppDbContext context)
        {
            this.context = context;
        }



        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            DropdownFill(id);
            return View();
        }

        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
           
            return View();
        }

        private void DropdownFill(int id)
        {
            ViewBag.Reservation = new SelectList(context.Cars.Where(q=>q.Id==id),"Id","CarName");
        }
    }
}
