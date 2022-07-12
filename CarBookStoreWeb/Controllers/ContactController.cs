using CarBookData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext context;

        public ContactController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var model = context.Contacts.SingleOrDefault(q => q.Enabled);
            return View(model);
        }
    }
}
