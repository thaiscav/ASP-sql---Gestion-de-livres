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
    public class MembresController : Controller
    {
        private BibliothequeEntities db = new BibliothequeEntities();


        // GET: Membres
        [HttpGet]
        public ActionResult Index(int? id_membre, string nom, string tel, string ville, string sexe)
        {
            var membres = db.Membres.Include(m => m.Adress);

            //FILTRES

            ViewBag.id_membre = (from c in db.Membres
                                 select c.id_membre).Distinct();

            ViewBag.nom = (from c in db.Membres
                          select c).Distinct();

            ViewBag.ville = (from c in db.Membres
                          select c.Adress.ville).Distinct();

            ViewBag.Tel = (from c in db.Membres
                          select c.tel).Distinct();

            ViewBag.Sexe = (from c in db.Membres
                           select c.sex);

            if (id_membre != null)
            {
                membres = from c in db.Membres
                          orderby c.id_membre
                          where c.id_membre == id_membre
                          select c;
            }
            else if ((!String.IsNullOrWhiteSpace(nom)) || (!String.IsNullOrWhiteSpace(tel)) || (!String.IsNullOrWhiteSpace(ville)) || (!String.IsNullOrWhiteSpace(sexe)))
            {
                membres = membres.Where(m => m.nom.ToUpper().Contains(nom.ToUpper())
                   || m.prenom.ToUpper().Contains(nom.ToUpper()) 
                   || m.tel.Contains(tel)
                   || m.Adress.ville.Contains(ville)
                   || m.sex == sexe
                   || m.courriel.Contains(nom));
            }

            // OPTION de Recherche exacte
            /*
                         membres = from c in db.Membres
                                    orderby c.nom
                                    where c.prenom == prenom || prenom.Equals(null) || prenom.Equals("")
                                    where c.nom == nom || nom.Equals(null) || nom.Equals("")
                                    where c.tel == tel || tel.Equals(null) || tel.Equals("")
                                    select c;
            */

            return View(membres.ToList());
        }

        // GET: Membres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membre membre = db.Membres.Find(id);
            if (membre == null)
            {
                return HttpNotFound();
            }
            return View(membre);
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
                       orderby c.id_livre
                       where c.id_membre == id
                       select c;

            var prenom = (from c in db.Membres
                          where c.id_membre == id
                          select c.prenom).FirstOrDefault();

            var nom = (from c in db.Membres
                          where c.id_membre == id
                          select c.nom).FirstOrDefault();

            ViewBag.membre = prenom + " " + nom;

            return View(emprunts.ToList());
        }

        // GET: Membres/Create
        public ActionResult Create()
        {
            ViewBag.id_adress = new SelectList(db.Adresses, "id_adress", "id_adress");
            return View();
        }

        // POST: Membres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_adress,numéro_civique,rue,ville,code_postal")] Adress adress,[Bind(Include = "id_membre,nom,prenom,id_adress,tel,courriel,sex,dt_naissance")] Membre membre)
        {
            if (ModelState.IsValid)
            {
                db.Adresses.Add(adress);
                db.Membres.Add(membre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_adress = new SelectList(db.Adresses, "id_adress", "id_adress", membre.id_adress);
            return View(membre);
        }

        // GET: Membres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membre membre = db.Membres.Find(id);
            if (membre == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_adress = new SelectList(db.Adresses, "id_adress", "id_adress", membre.id_adress);
            return View(membre);
        }

        // POST: Membres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_membre,nom,prenom,id_adress,tel,courriel,sex,dt_naissance")] Membre membre, [Bind(Include = "id_adress,numéro_civique,rue,ville,code_postal")] Adress adress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adress).State = EntityState.Modified;
                db.Entry(membre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_adress = new SelectList(db.Adresses, "id_adress", "id_adress", membre.id_adress);
            return View(membre);
        }

        // GET: Membres/Delete/5
        public ActionResult Delete(int? id_m, int? id_a)
        {
            if (id_m == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membre membre = db.Membres.Find(id_m);
            if (membre == null)
            {
                return HttpNotFound();
            }
            return View(membre);
        }

        // POST: Membres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id_m, int id_a)
        {
            Membre membre = db.Membres.Find(id_m);
            db.Membres.Remove(membre);
            Adress adresses = db.Adresses.Find(id_a);
            db.Adresses.Remove(adresses);
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
