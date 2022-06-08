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
    public class ContactController : Controller
    {
        private const string entityname="Adres";
        private readonly AppDbContext context;

        public ContactController(AppDbContext context)
        {
            this.context = context;
        }


        public IActionResult Index()
        {
            var model = context.Contacts.OrderBy(q => q.Address).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            contact.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            contact.DateCreated = DateTime.Now;
            context.Entry(contact).State = EntityState.Added;

            try
            {
                context.SaveChanges();
                TempData["success"] = $"{entityname} Adres Ekleme İşlemi Başarıyla Tamamlanmıştır";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityname} Adres Ekleme İşlemi Hata Oluştu";
                return View(contact);
            }
            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contact = context.Contacts.Find(id);
            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            contact.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            contact.DateCreated = DateTime.Now;
            context.Entry(contact).State = EntityState.Modified;
            try
            {
                context.SaveChanges();
                TempData["success"] = $"{entityname} Adres Güncelleme Başarılı";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityname} Adres Güncellemede  Hata Oluştu";
                return View(contact);
            }
            
        }
    }
}
