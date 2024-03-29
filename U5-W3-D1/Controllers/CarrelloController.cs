﻿using System;
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
    public class CarrelloController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Carrello
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<Prodotti>;
            if (cart == null || !cart.Any())
            {
                TempData["CartMessage"] = "Il carrello è vuoto";
                return RedirectToAction("Index", "Prodotti");
            }
            return View(cart);
        }

        // GET: Carrello/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // POST: Carrello/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProdotto,Nome,Foto,Foto2,Foto3,Prezzo,Consegna,Ingredienti")] Prodotti prodotti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prodotti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prodotti);
        }

        // GET: Carrello/Delete/5
            public ActionResult Delete(int? id)
            {
                var cart = Session["cart"] as List<Prodotti>;
                if (cart != null)
                {
                    var productToRemove = cart.FirstOrDefault(p => p.idProdotto == id);
                    if (productToRemove != null)
                    {
                        if (productToRemove.quantita > 1)
                        {
                            productToRemove.quantita--;
                        }
                        else
                        {
                            cart.Remove(productToRemove);
                        }
                    }
                }

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


        // POST: Carrello/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prodotti prodotti = db.Prodotti.Find(id);
            db.Prodotti.Remove(prodotti);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Ordina(string note, string indirizzo)
        {
            ModelDbContext db = new ModelDbContext();
            var userId = db.Utenti.FirstOrDefault(u => u.Email == User.Identity.Name).idUtente;

            var cart = Session["cart"] as List<Prodotti>;
            if (cart != null && cart.Any()) // Check if the cart is not empty
            {
                // Create a new order
                Ordini newOrder = new Ordini();
                newOrder.DataOrdine = DateTime.Now;
                newOrder.isEvaso = false;
                newOrder.idUtente_FK = userId;
                newOrder.Indirizzo = indirizzo;
                newOrder.Totale = cart.Sum(p => p.Prezzo);
                newOrder.Note = note;

                // Save the order to the database
                db.Ordini.Add(newOrder);
                db.SaveChanges();
                TempData["Message"] = "Ordine effettuato con successo";

                foreach (var product in cart)
                {
                    Dettagli newDetail = new Dettagli();
                    newDetail.idOrdini_FK = newOrder.idOrdine;
                    newDetail.idProdotto_FK = product.idProdotto;
                    newDetail.Quantità = Convert.ToInt32(product.quantita);

                    // Save the order detail to the database
                    db.Dettagli.Add(newDetail);
                    db.SaveChanges();
                   
                }

                // Clear the cart
                cart.Clear();
            }

            return RedirectToAction("Index", "Prodotti");
        }
    }
}
