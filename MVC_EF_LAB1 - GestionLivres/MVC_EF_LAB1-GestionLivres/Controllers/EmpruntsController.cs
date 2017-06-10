using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_EF_LAB1_GestionLivres.Models;
using MVC_EF_LAB1_GestionLivres.ViewModel;

namespace MVC_EF_LAB1_GestionLivres.Controllers
{
    public class EmpruntsController : Controller
    {
        private BibliothequeEntities db = new BibliothequeEntities();
/*
        // GET: Emprunts
        public ActionResult Index()
        {
            var emprunts = db.Emprunt.Include(e => e.Livre).Include(e => e.Membre);

            return View(emprunts.ToList());
        }
*/
        // GET: Emprunts
        [HttpGet]
        public ActionResult Index(string prenomNom, string titre, string ISBN)
        {
            var emprunts = db.Emprunt.Include(m => m.Membre).Include(m => m.Livre);

            //FILTRES

            ViewBag.prenomNom = (from c in db.Emprunt
                                 select c.Membre).Distinct();

            ViewBag.titre = (from c in db.Emprunt
                             select c.Livre.nom).Distinct();

            ViewBag.ISBN = (from c in db.Emprunt
                            select c.Livre.ISBN).Distinct();

            if ((!String.IsNullOrWhiteSpace(prenomNom)) || (!String.IsNullOrWhiteSpace(titre)) || (!String.IsNullOrWhiteSpace(ISBN)))
            {
                emprunts = emprunts.Where(e => e.Membre.nom.ToUpper().Contains(prenomNom.ToUpper())
                   || e.Membre.prenom.ToUpper().Contains(prenomNom.ToUpper())
                   || e.Livre.nom.Contains(titre)
                   || e.Livre.ISBN.Contains(ISBN));
            }
            else
            {
                emprunts = from c in db.Emprunt
                          orderby c.dt_pret
                          select c;
            }

            return View(emprunts);
        }


        // GET: Emprunts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emprunt emprunt = db.Emprunt.Find(id);
            if (emprunt == null)
            {
                return HttpNotFound();
            }
            return View(emprunt);
        }

        public ActionResult DetailEmprunt()
        {
            //Quantidade de livros por Membro

            //select * from Emprunt group by id_membres
            var resultado = db.Emprunt.GroupBy(x => x.id_membre).Select(grupo => new { Chave = grupo.Key, Valores = grupo.ToList() }).ToList();

            var data = from membre in db.Emprunt
                       group membre by membre.id_membre into qntdEmprunt

                       select new DetailEmprunt()
                       {
                           id_membre = qntdEmprunt.Key,
                           nom_membre = qntdEmprunt.ToString(),
                           //prenom_membre = qntdEmprunt.ToString(),
                           total_emprunt = qntdEmprunt.Count()
                           //nb_livreNonRetourne = qntdEmprunt.Count()
                       };

            return View(data);
        }

        // GET: Emprunts/Create
        public ActionResult Create()
        {
            //Controle de Membre
            string membre = "SELECT * FROM Membres WHERE id_membre IN (SELECT id_membre FROM Emprunt GROUP BY id_membre HAVING COUNT(id_membre) <= 3) OR id_membre NOT IN (SELECT id_membre FROM Emprunt)";
            var membresOK = db.Database.SqlQuery<Membre>(membre).ToList();

            //Controle de Livres
            string livres = "SELECT * FROM Livres WHERE id_livre NOT IN(SELECT id_livre FROM Emprunt WHERE dt_retour IS NULL)";
            var livresOK = db.Database.SqlQuery<Livre>(livres).ToList();

            ViewBag.id_livre = new SelectList(livresOK, "id_livre", "nom");
            ViewBag.id_membre = new SelectList(membresOK, "id_membre", "prenom");

            return View();
        }

        // POST: Emprunts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_emprunt,id_membre,prenom,id_livre,nom,dt_pret,dt_retour")] Emprunt emprunt)
        {
            if (ModelState.IsValid)
            {
                db.Emprunt.Add(emprunt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_livre = new SelectList(db.Livres, "id_livre", "nom", emprunt.Livre.nom);
            ViewBag.id_membre = new SelectList(db.Membres, "id_membre", "prenom", emprunt.Membre.prenom);

            return View(emprunt);
        }

        // GET: Emprunts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emprunt emprunt = db.Emprunt.Find(id);
            if (emprunt == null)
            {
                return HttpNotFound();
            }

            //Controle de Membre
            string membre = "SELECT * FROM Emprunt e INNER JOIN Membres l ON e.id_membre = l.id_membre where id_emprunt=" + id;
            var membreOK = db.Database.SqlQuery<Membre>(membre).ToList();

            //Controle de Livres
            string livre = "SELECT * FROM Emprunt e INNER JOIN Livres l ON e.id_livre = l.id_livre where id_emprunt=" + id;
            var livreOK = db.Database.SqlQuery<Livre>(livre).ToList();

            ViewBag.id_livre = new SelectList(livreOK, "id_livre", "nom");
            ViewBag.id_membre = new SelectList(membreOK, "id_membre", "prenom");

            return View(emprunt);
        }

        // POST: Emprunts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_emprunt,id_membre,id_livre,dt_pret,dt_retour")] Emprunt emprunt)
        {

            if (emprunt.dt_retour >= emprunt.dt_pret)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(emprunt).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            if (emprunt.dt_retour < emprunt.dt_pret)
            {
                return RedirectToAction("Edit");
            }

            return View(emprunt);
        }

        // GET: Emprunts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emprunt emprunt = db.Emprunt.Find(id);
            if (emprunt == null)
            {
                return HttpNotFound();
            }
            return View(emprunt);
        }

        // POST: Emprunts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emprunt emprunt = db.Emprunt.Find(id);
            db.Emprunt.Remove(emprunt);
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
