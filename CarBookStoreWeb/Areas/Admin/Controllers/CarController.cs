using CarBookData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class CarController : Controller
    {
        private const string entityName = "Araç Ekleme";
        private readonly AppDbContext context;

        public CarController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var carAll = context.Cars;
            return View(carAll);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(Car model)
        {
            model.DateCreated  = DateTime.Now;
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //model.Safe = Enum.GetName((Kasa)Convert.ToInt32(model.Safe));
           
            context.Entry(model).State = EntityState.Added;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} ekleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(model);
            }
        }

        [HttpGet]
        public async Task <IActionResult> Edit(int id)
        {
            var model = await context.Cars.FindAsync(id);
           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Car model)
        {
            var original = await context.Cars.FindAsync(model.Id);
           
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            context.Entry(original).CurrentValues.SetValues(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} Güncelleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} güncelleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Cars.FindAsync(id);
            context.Entry(model).State = EntityState.Deleted;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} silme işlemi başarıyla tamamlanmıştır.";
            }
            catch (DbUpdateException)
            {
            }
            return RedirectToAction("Index");
        }
    }
}
