using CarBookData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,Staff")]
    public class CarPictureController : Controller
    {
        private readonly string entityname ="Araç Resim Ekleme";
        private readonly AppDbContext context;

        public CarPictureController(AppDbContext context)
        {
            this.context = context; 
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
