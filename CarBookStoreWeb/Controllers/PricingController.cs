﻿using CarBookData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Controllers
{
    public class PricingController : Controller
    {
        private readonly AppDbContext context;

        public PricingController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var model = context.Cars.OrderBy(q=>q.CarName).ToList();
            return View(model);
        }
       
    }
}
