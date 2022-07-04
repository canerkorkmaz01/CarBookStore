using CarBookData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Components
{
    [ViewComponent(Name = "Footer")]
    public class FooterViewComponents : ViewComponent
    {
        private readonly AppDbContext context;

        public FooterViewComponents(AppDbContext context)
        {
            this.context = context;
        }


        public IViewComponentResult Invoke()
        {
            var model = context.Contacts.SingleOrDefault(q => q.Enabled);
            return View(model);
        }
    }
}
