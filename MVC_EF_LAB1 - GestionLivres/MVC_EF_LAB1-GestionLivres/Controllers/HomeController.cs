using MVC_EF_LAB1_GestionLivres.Models;
using MVC_EF_LAB1_GestionLivres.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_EF_LAB1_GestionLivres.Controllers
{
    public class HomeController : Controller
    {
        private BibliothequeEntities db = new BibliothequeEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailEmprunt()
        {
            //Quantidade de livros por Membro

            //select * from Emprunt group by id_membres
            var resultado = db.Emprunt.GroupBy(x => x.id_membre).Select(grupo => new { Chave = grupo.Key, Valores = grupo.ToList()}).ToList();

            var data = from membre in db.Emprunt
                       group membre by membre.id_membre into qntdEmprunt

                       select new DetailEmprunt()
                       {
                           id_membre = qntdEmprunt.Key,
                           //nom_membre = qntdEmprunt,
                           //prenom_membre = qntdEmprunt.ToString(),
                           total_emprunt = qntdEmprunt.Count()
                           //nb_livreNonRetourne = qntdEmprunt.Count()
                       };
   
            return View(data);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}