using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using U5_W3_D1.Models;

namespace U5_W3_D1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UtentiController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Utenti
        public ActionResult Index()
        {
            return View(db.Utenti.ToList());
        }
        public ActionResult isAdmin(int id)
        {
            //change the user to admin
            Utenti utente = db.Utenti.Find(id);
            utente.isAdmin = !utente.isAdmin;
            db.Entry(utente).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Delete(int id)
        {
            Utenti utente = db.Utenti.Find(id);
            db.Utenti.Remove(utente);
            db.SaveChanges();
            TempData["Message"] = "Utente eliminato";
            return RedirectToAction("Index");
        }
    }
}
