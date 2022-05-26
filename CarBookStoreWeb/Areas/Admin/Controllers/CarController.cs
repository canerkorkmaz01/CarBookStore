using CarBookData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            Cars();
            ViewBag.CarFeatures = context.Features.OrderBy(p => p.Name).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car model)
        {
            if (model.PhotoFile != null)
            {
                try
                {
                    using (var image = Image.Load(model.PhotoFile.OpenReadStream()))
                    {
                        image.Mutate(p =>
                        {
                            p.Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Max,
                                Size = new Size(600, 600)
                            });
                            p.BackgroundColor(Color.White);
                            model.Photo = image.ToBase64String(JpegFormat.Instance);
                        });
                    }
                }
                catch (UnknownImageFormatException)
                {
                    ModelState.AddModelError("", "Yüklenen dosya bilinen bir görsel biçiminde değil!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Lütfen bir logo yükleyiniz!");
                return View(model);
            }

            if (model.PhotoFiles != null)
                foreach (var photoFile in model.PhotoFiles)
                {
                    try
                    {
                        using (var image = Image.Load(photoFile.OpenReadStream()))
                        {
                            image.Mutate(p =>
                            {
                                p.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = new Size(600, 600)
                                });
                                p.BackgroundColor(Color.White);
                                var photo = new CarPicture
                                {
                                    UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                                    DateCreated = DateTime.Now,
                                    Enabled = model.Enabled,
                                    Photo = image.ToBase64String(JpegFormat.Instance)
                                };
                                model.CarPictures.Add(photo);
                                context.Entry(photo).State = EntityState.Added;

                            });

                        }
                    }
                    catch (UnknownImageFormatException)
                    {

                    }
                }

            if (model.SelectedFeatures != null)
            {
                var features = await context.Features.ToListAsync();
                model.SelectedFeatures.ToList().ForEach(p => model.Features.Add(features.Single(q => q.Id == p)));
            }

            model.DateCreated = DateTime.Now;
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(model).State = EntityState.Added;
            ////model.Safe = Enum.GetName((Kasa)Convert.ToInt32(model.Safe));
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
        public async Task<IActionResult> Edit(int? id)
        {
            Cars();
            if (id == null)
            {
                return NotFound();
            }

            var model = await context.Cars.FindAsync(id);
            var features = model.Features.ToList();
            ViewBag.CarFeatures = context.Features.OrderBy(p => p.Name).ToList().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name, Selected = features.Any(q => q.Id == p.Id) });

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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await context.Cars.FindAsync(id);
            context.Entry(model).State = EntityState.Deleted;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} silme işlemi başarıyla tamamlanmıştır.";
            }
            catch (DbUpdateException e)
            {
                TempData["success"] = $"{e} silme işlemi Başarısız Olmuştur";
            }
            return RedirectToAction("Index");
        }

        private void Cars()
        {
            ViewBag.Feature = context.Features.ToList();
        }

        //private void Features(int? id)
        //{
        //    ViewBag.Features = context.Cars.Where(x=>x.CarFeatur)
        //}
    }
}
