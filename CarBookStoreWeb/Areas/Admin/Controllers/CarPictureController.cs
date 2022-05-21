using CarBookData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


namespace CarBookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,Staff")]
    public class CarPictureController : Controller
    {
        private readonly string entityName ="Araç ";
        private readonly AppDbContext context;

        public CarPictureController(AppDbContext context)
        {
            this.context = context; 
        }

        public IActionResult Index()
        {
            DropdownFill();
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            DropdownFill();
            return  View();
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
                    DropdownFill();
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
                                    Enabled = true,
                                    DateCreated = DateTime.Now,
                                    Photo = image.ToBase64String(JpegFormat.Instance)
                                };
                                context.Entry(photo).State = EntityState.Added;
                                model.CarPictures.Add(photo);
                            });
                        }
                    }
                    catch (UnknownImageFormatException)
                    {

                    }
                }

            model.DateCreated = DateTime.Now;
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(model).State = EntityState.Added;
     
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                TempData["error"] = $"{entityName} ekleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                DropdownFill();
                return View(model);
            }
        }
        public void DropdownFill()
        {
            ViewBag.CarPicture = new SelectList(context.Cars.OrderBy(C => C.CarName).ToList(),"Id","CarName");
        }
    }       
}
     