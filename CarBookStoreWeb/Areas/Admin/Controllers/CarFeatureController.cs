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
    public class CarFeatureController : Controller
    {
        private const string entityName = "Araç Özelliği";
        private readonly AppDbContext context;

        public CarFeatureController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task <IActionResult> Index()
        {
            var model = await context.CarFeatures.ToListAsync();
            DropdownFill();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            DropdownFill();
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(CarFeature carFeature)
        {
            carFeature.DateCreated = DateTime.Now;
            carFeature.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(carFeature).State = EntityState.Added;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} Ekleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} Eklem işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(carFeature);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            DropdownFill();
            if (id == null)
            {
                NotFound();
            }
            var features = await context.CarFeatures.FindAsync(id);
            return View(features);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CarFeature carFeature)
        {
            carFeature.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(carFeature).State = EntityState.Modified;
            try
            {
                TempData["success"] = $"{entityName}Ekleme İşlemi Başarıyla Tamamlanmıştır ";
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {

                TempData["error"] = $"{entityName} Ekleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(carFeature);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await context.CarFeatures.FindAsync(id);

            context.Entry(model).State = EntityState.Deleted;

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $" {entityName}Silme İşlemi Başarıyla Gerçekleştirilmiştir";
               
            }
            catch (DbUpdateException e)
            {
                TempData["success"] = $"{e} silme işlemi Başarısız Olmuştur";
            }
            return RedirectToAction("Index");
        }
        public void DropdownFill()
        {
            ViewBag.Feauture = new SelectList(context.Cars.OrderBy(p => p.CarName).ToList(), "Id", "CarName");
        }

    }
}
