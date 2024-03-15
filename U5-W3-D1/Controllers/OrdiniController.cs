using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using U5_W3_D1.Models;

namespace U5_W3_D1.Controllers
{
    public class OrdiniController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ordini
        public ActionResult Index()
        {
            var ordini = db.Ordini.Include(o => o.Utenti).OrderByDescending(o => o.DataOrdine); ;
            return View(ordini.ToList());
        }

        public ActionResult isEvaso(int id)
        {
            Ordini ordine = db.Ordini.Find(id);
            ordine.isEvaso = true;
            db.Entry(ordine).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Ordini/Details/5
        public ActionResult isntEvaso(int id)
        {
            Ordini ordine = db.Ordini.Find(id);
            ordine.isEvaso = false;
            db.Entry(ordine).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult Details(int? id)
        {
            var Dettagli = db.Dettagli.Include(o => o.Ordini)
                .Include(o => o.Prodotti)
                .Where(o => o.idOrdini_FK == id);

            return PartialView("_Details", Dettagli.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult OrdiniUtente()
        {
            var userId = db.Utenti.FirstOrDefault(u => u.Email == User.Identity.Name).idUtente;
            var ordini = db.Ordini.Include(o => o.Utenti)
                .Include(o => o.Dettagli)
                .Where(o => o.idUtente_FK == userId)
                .OrderByDescending(o => o.DataOrdine);
            return View(ordini.ToList());
        }


        // GET: Ordini/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUtente_FK = new SelectList(db.Utenti, "idUtente", "Nome", ordini.idUtente_FK);
            return View(ordini);
        }

        // POST: Ordini/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idOrdine,Indirizzo,idUtente_FK,Totale,isEvaso,Note,DataOrdine")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordini).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idUtente_FK = new SelectList(db.Utenti, "idUtente", "Nome", ordini.idUtente_FK);
            return View(ordini);
        }

        // GET: Ordini/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // POST: Ordini/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordini ordini = db.Ordini.Find(id);
            db.Ordini.Remove(ordini);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Dettagli(int id)
        {
            var dettagli = db.Dettagli.Where(d => d.idOrdini_FK == id).Include(d => d.Prodotti);
            return View(dettagli.ToList());
        }



        public async Task<ActionResult> GetNumeroOrdini()
        {
            // Conta il numero di ordini nel database
            int totale = await db.Ordini.Where(o => o.isEvaso == true).CountAsync();

            // Restituisce il conteggio come risposta JSON
            return Json(totale, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> IncassatoGiorno(DateTime data)
        {

            // Calcola l'incasso per il giorno specificato
            decimal incasso = await db.Ordini
                .Where(o => o.DataOrdine.Year == data.Year && o.DataOrdine.Month == data.Month && o.DataOrdine.Day == data.Day)
                .SumAsync(o => o.Totale);

            // Restituisce l'incasso come risposta JSON
            return Json(incasso, JsonRequestBehavior.AllowGet);
        }




    }
}
