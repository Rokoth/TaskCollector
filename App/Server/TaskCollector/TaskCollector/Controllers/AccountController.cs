//Copyright 2021 Dmitriy Rokoth
//Licensed under the Apache License, Version 2.0
//
//ref2
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;
using TaskCollector.Service;

namespace TaskCollector.Controllers
{
    /// <summary>
    /// Controller for user login
    /// </summary>
    public class AccountController : CommonControllerBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public AccountController(IServiceProvider serviceProvider): base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // GET: AccountController/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: AccountController/Create
        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="userIdentity"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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
                        return ErrorRedirect("Неверный логин или пароль", null);                            
                    }                    
                    // установка аутентификационных куки
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(identity));
                }
                if(!string.IsNullOrEmpty(returnUrl)) 
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                return ErrorRedirect(ex.Message, ex.StackTrace);
            }
        }                

        // POST: AccountController/Create
        /// <summary>
        /// logout action
        /// </summary>
        /// <returns></returns>
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
                return ErrorRedirect(ex.Message, ex.StackTrace);
            }
        }
    }
}
