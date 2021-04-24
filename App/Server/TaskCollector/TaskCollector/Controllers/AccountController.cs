using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskCollector.Contract.Model;

namespace TaskCollector.Controllers
{
    public class AccountController : Controller
    {
       
        // GET: AccountController/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserIdentity userIdentity)
        {
            try
            {
                return Ok();
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }                

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            try
            {
                return Ok();
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Error", new { Message = ex.Message });
            }
        }
    }
}
