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
        public IActionResult Create(Car model)
        {
            model.DateCreated = DateTime.Now;
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            model.Enabled = true;
            model.Safe = Enum.GetName((Kasa)Convert.ToInt32(model.Safe));
            model.FuelType = Enum.GetName((Yakıt)Convert.ToInt32(model.FuelType));
            model.GearType = Enum.GetName((Vites)Convert.ToInt32(model.GearType));
            model.Licence = Enum.GetName((Ehliyet)Convert.ToInt32(model.Licence));
            context.Entry(model).State = EntityState.Added;

            try
            {
                context.SaveChanges();
                TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} ekleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(model);
            }
            

        }

    }
}
