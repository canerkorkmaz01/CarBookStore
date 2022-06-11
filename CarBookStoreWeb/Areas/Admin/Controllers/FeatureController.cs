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
    public class FeatureController : Controller
    {
        private const string entityName = "Araç Özelliği";
        private readonly AppDbContext context;

        public FeatureController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var feature = context.Features.OrderBy(x => x.Name).ToList();
            return View(feature);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Feature carFeature)
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
           
            if (id == null)
            {
                NotFound();
            }
            var features = await context.Features.FindAsync(id);
            return View(features);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Feature carFeature)
        {
            carFeature.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(carFeature).State = EntityState.Modified;
            try
            {
                TempData["success"] = $"{entityName}Güncelleme İşlemi Başarıyla Tamamlanmıştır ";
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {

                TempData["error"] = $"{entityName} Güncelleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(carFeature);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await context.Features.FindAsync(id);

            context.Entry(model).State = EntityState.Deleted;

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $" {entityName}Silme İşlemi Başarıyla Gerçekleştirilmiştir";

            }
            catch (DbUpdateException e)
            {
                TempData["success"] = $"{e} Silme işlemi Başarısız Olmuştur";
            }
            return RedirectToAction("Index");
        }
        //public void DropdownFill()
        //{
        //    ViewBag.Feauture = new SelectList(context.Cars.OrderBy(p => p.CarName).ToList(), "Id", "CarName");
        //}
    }
}
