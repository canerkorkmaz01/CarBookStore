using CarBookData;
using Microsoft.AspNetCore.Http;
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
   
    public class ReservationController : Controller
    {
        private const string entityName= "Rezervasyon";
        private readonly AppDbContext context;
   
        public ReservationController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Create(int? reservationid)
        {
            DropdownFill(reservationid);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            reservation.DateCreated = DateTime.Now;
            reservation.Enabled = true;
            reservation.UserId = 3;
            context.Entry(reservation).State = EntityState.Added;

            try
            {
                await context.SaveChangesAsync();
                return View();
            }
            catch (DbUpdateException)
            {
                //DropdownFill(reservation.Id);
                return View(reservation);
            }
        }

        private void DropdownFill(int? id)
        {
            //int ıd = Convert.ToInt32(this.RouteData.Values["Reservationid"]);
            ViewBag.Reservation = new SelectList(context.Cars.Where(q => q.Id == id), "Id", "CarName");
            ViewBag.GearType = new SelectList(context.Cars.Where(q => q.Id == id), "Id", "GearType");
            ViewBag.FuelType = new SelectList(context.Cars.Where(q => q.Id == id), "Id", "FuelType");
            ViewBag.Photo = context.Cars.Find(id).Photo;
            
        }
    }
}
