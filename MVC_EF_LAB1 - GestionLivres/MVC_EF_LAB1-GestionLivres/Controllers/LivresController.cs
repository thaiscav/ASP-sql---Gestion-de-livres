using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_EF_LAB1_GestionLivres.Models;

namespace MVC_EF_LAB1_GestionLivres.Controllers
{
    public class LivresController : Controller
    {
        private BibliothequeEntities db = new BibliothequeEntities();

        // GET: Livres
        [HttpGet]
        public ActionResult Index(string titreAuteur, string isbn, string categorie)
        {

            var livres = (from c in db.Livres
                          select c).Distinct();

            ViewBag.TitreAuteur = (from c in db.Livres
                                   select c).Distinct();

            /*ViewBag.Auteur = (from c in db.Livres
                              select c.auteur).Distinct();*/

            ViewBag.Isbn = (from c in db.Livres
                            select c.ISBN).Distinct();

            ViewBag.Categorie = (from c in db.Livres
                                 select c.categorie);


                if ((!String.IsNullOrWhiteSpace(titreAuteur)) || (!String.IsNullOrWhiteSpace(isbn)) || (!String.IsNullOrWhiteSpace(categorie)))
                {
                    livres = livres.Where(l => l.nom.ToUpper().Contains(titreAuteur.ToUpper())
                       || l.auteur.ToUpper().Contains(titreAuteur)
                       || l.ISBN.ToUpper().Contains(isbn.ToUpper())
                       || l.categorie.ToUpper().Contains(categorie));
                }

            return View(livres.ToList());
        }

        // GET: Livres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livre livre = db.Livres.Find(id);
            if (livre == null)
            {
                return HttpNotFound();
            }
            return View(livre);
        }

        public ActionResult List(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Emprunt emprunt = db.Emprunts.Find(id);

            var emprunts = db.Emprunt.Include(e => e.Livre).Include(e => e.Membre);

            emprunts = from c in db.Emprunt
                       orderby c.id_membre
                       where c.id_livre == id
                       select c;

            var titre = (from c in db.Livres
                          where c.id_livre == id
                          select c.nom).FirstOrDefault();


            ViewBag.livre = titre;

            return View(emprunts.ToList());
        }

        // GET: Livres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Livres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_livre,ISBN,nom,auteur,annee,editeur,categorie")] Livre livre)
        {
            if (ModelState.IsValid)
            {
                db.Livres.Add(livre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(livre);
        }

        // GET: Livres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livre livre = db.Livres.Find(id);
            if (livre == null)
            {
                return HttpNotFound();
            }
            return View(livre);
        }

        // POST: Livres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_livre,ISBN,nom,auteur,annee,editeur,categorie")] Livre livre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(livre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(livre);
        }

        // GET: Livres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livre livre = db.Livres.Find(id);
            if (livre == null)
            {
                return HttpNotFound();
            }
            return View(livre);
        }

        // POST: Livres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Livre livre = db.Livres.Find(id);
            db.Livres.Remove(livre);
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
    }
}
