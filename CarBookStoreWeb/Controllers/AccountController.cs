using CarBookData;
using CarBookStoreWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarBookStoreWeb.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly AppDbContext context;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            AppDbContext context
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var newUser = new User
            {
                UserName=model.Email,
                Name=model.Name,
                Email=model.Email,
                Gender=model.Gender,
                EmailConfirmed=false
            };
            var result = await userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(model);
            }
            else
            {
                await userManager.AddClaimAsync(newUser, new Claim("FullName",newUser.Name));
                await userManager.AddToRoleAsync(newUser, "Staff");

                var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var url = Url.Action("Confirmation", "Account", new { id = newUser.Id, token }, Request.Scheme);

                //var body = string.Format(
                //    System.IO.File.ReadAllText(System.IO.Path.Combine(hostingEnvironment.WebRootPath, "Content", "template", "confirmation.html")),
                //    model.Name,
                //    url
                //    );

                //await mailService.SendAsync(
                //    mailTo: model.Email,
                //    subject: $"{configuration.GetValue<string>("Application:Name")} Üyelik E-Posta Doğrulama Mesajı",
                //    message: body,
                //    isHtml: true
                //    );

                  return View("RegisterSuccess");

                /*return View(url)*/;
            }
            
        }

        public async Task<IActionResult> Confirmation(int id, string token)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model =  new LoginViewModel { RememberMe = true };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {


            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
            var user = await userManager.FindByNameAsync(model.UserName);

            if (!user.Enabled)
            {
                ModelState.AddModelError("", "Yasaklı kullanıcı girişi");

                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl ?? "/");
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz kullanıcı girişi");
                    return View(model);

                }
                
            }

            //var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
            //if (result.Succeeded )
            //{
            //    var user = await userManager.FindByNameAsync(model.UserName);
            //    if (!user.Enabled)
            //    {
            //        ModelState.AddModelError("", "Yasaklı kullanıcı girişi");

            //        return RedirectToAction("Login", "Account");
            //    }
            //    return Redirect(model.ReturnUrl ?? "/");

            //}
            //else
            //{
            //    ModelState.AddModelError("", "Geçersiz kullanıcı girişi");
            //    return View(model);

            //}
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
