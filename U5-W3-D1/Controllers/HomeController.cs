using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using U5_W3_D1.Models;

namespace U5_W3_D1.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }


        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string psw)
        {
            using (var context = new ModelDbContext())
            {
                var user = context.Utenti.FirstOrDefault(u => u.Email == email && u.Psw == psw);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        

        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
            public ActionResult Register(Utenti utente)
            {
                using (var context = new ModelDbContext())
                {
                    context.Utenti.Add(utente);
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
    }

}
