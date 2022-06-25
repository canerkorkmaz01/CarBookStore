using CarBookData;
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
        private const string entityName= "";
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
        public async Task <IActionResult> Create(Reservation reservation)
        {
            reservation.DateCreated = DateTime.Now;
            reservation.UserId = 0;
            context.Entry(reservation).State = EntityState.Added;

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} Rezervasyon işlemi başarıyla tamamlanmıştır";
                return RedirectToAction("Index","Home");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} Rezervasyon Ekleme işleminde Hata Oluştu";
                DropdownFill(reservation.CarId);
                return View(reservation);
            }
            
        }

        private void DropdownFill(int id)
        {
            ViewBag.Reservation = new SelectList(context.Cars.Where(q=>q.Id==id),"Id","CarName");
        }
    }
}
