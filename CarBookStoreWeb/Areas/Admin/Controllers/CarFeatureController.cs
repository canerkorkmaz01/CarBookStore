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
    
        public IActionResult Create()
        {
            DropdownFill();
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(CarFeature carFeature)
        {
            carFeature.DateCreated = DateTime.Now;
            carFeature.Enabled = true;
            carFeature.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(carFeature).State = EntityState.Added;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                TempData["error"] = $"{entityName} güncelleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(carFeature);
            }
            
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            DropdownFill();
            var features = await context.CarFeatures.FindAsync(id);
            return View(features);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(CarFeature carFeature)
        {
            return View();
        }


        public void DropdownFill()
        {
            ViewBag.Feauture = new SelectList(context.Cars.OrderBy(p => p.CarName).ToList(), "Id", "CarName");
        }

    }
}
