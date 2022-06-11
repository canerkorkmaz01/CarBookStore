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
    [Authorize(Roles = "Administrators")]
    public class PricingController : Controller
    {
        private const string entityName = "Araç Fiyatlandırma";
        private readonly AppDbContext context;

        public PricingController(
            AppDbContext context
            )
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var model = context.Pricings.OrderBy(q=>q.Cars).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            DropdownFill();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pricing pricing)
        {
            pricing.DateCreated = DateTime.Now;
            pricing.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(pricing).State = EntityState.Added;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} Ekleme İşlemi Başarıyla Tamamlanmıştır";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["success"] = $"{entityName} Ekleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(pricing);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult>Edit(int id)
        {
            DropdownFill();
            var model = await context.Pricings.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pricing pricing)
        {
            DropdownFill();
            pricing.DateCreated = DateTime.Now;
            pricing.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(pricing).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} Güncelleme İşlemi Başarıyla Tamamlanmıştır";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["success"] = $"{entityName} Güncelleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(pricing);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await context.Pricings.FindAsync(id);
            context.Entry(model).State = EntityState.Deleted;

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} Silme İşlemi başarıyla Tamamlanmıştır";
            }
            catch (DbUpdateException)
            {
                TempData["success"] = "isimli kayıt, bir ya da daha fazla kayıt ile ilişkili olduuğundan silme işlemi yapılamıyor!";
                
            }
            return RedirectToAction("Index");
        }

        private void DropdownFill()
        {
            ViewBag.Pricing = new SelectList(context.Cars.OrderBy(p => p.CarName).ToList(), "Id", "CarName");
        }

    }
}
