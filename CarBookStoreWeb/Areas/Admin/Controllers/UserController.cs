using CarBookData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,Staff")]
    public class UserController : Controller
    {
        private const string entityName = "Üyeler";
        private readonly AppDbContext context;

        public UserController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var users = context.Users.OrderBy(q => q.UserName).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = context.Users.Find(id);
            user.Enabled = !user.Enabled;
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
            return Json(user.Enabled);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = context.Users.Find(id);
            context.Entry(user).State = EntityState.Deleted;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
