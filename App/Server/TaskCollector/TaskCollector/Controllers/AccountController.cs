using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    public class AccountController : Controller
    {
        private IServiceProvider _serviceProvider;
        public AccountController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        // GET: AccountController/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserIdentity userIdentity, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var source = new CancellationTokenSource(30000);
                    var dataService = _serviceProvider.GetRequiredService<IAuthService>();
                    var identity = await dataService.Auth(userIdentity, source.Token);
                    if (identity == null)
                    {                       
                        return RedirectToAction("Index", "Error", new { Message = "Неверный логин или пароль" });
                    }                    
                    // установка аутентификационных куки
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                }
                if(returnUrl!=null) return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }                

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    await HttpContext.SignOutAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }
    }
}
