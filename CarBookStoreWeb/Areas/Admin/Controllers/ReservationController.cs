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

namespace CarBookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,Staff")]
    public class ReservationController : Controller
    {
        private const string entityName = "Rezervasyon";
        private readonly AppDbContext context;

        public ReservationController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
           
            var model = context.Reservations.OrderBy(q => q.NameSurname).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            DropdownFill();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            reservation.DateCreated = DateTime.Now;
            reservation.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(reservation).State = EntityState.Added;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} Ekleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} Ekleme işleminde Hata Oluştu";
                DropdownFill();
                return View(reservation);
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            DropdownFill();
            var model = context.Reservations.Find(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            reservation.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            reservation.DateCreated = DateTime.Now;
            context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} Güncelleme İşlemi Başarıyla Tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["error"] = $"{entityName} Güncelleme İşlemi Başarısız Oldu";
                return View(reservation);
            }
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var model = context.Reservations.Find(id);
            context.Entry(model).State = EntityState.Deleted;

            try
            {
                context.SaveChanges();
                TempData["success"] = $"{entityName} Silme İşlemi Başarıyla Tamamlanmıştır";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} Silme İşleminde Hata Oluştu";
                return View("Index");
            }
            return View(model);
        }

        public void DropdownFill()
        {
            ViewBag.Reservation = new SelectList(context.Cars.OrderBy(q => q.CarName),"Id","CarName");
        }
    }
}
