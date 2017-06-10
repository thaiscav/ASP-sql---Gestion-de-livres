using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_EF_LAB1_GestionLivres.Models
{
    public class ControleEmprunt
    {
            public static BibliothequeEntities db = new BibliothequeEntities();

            public static Dictionary<string, int> detailsEmprunts(int id_membre)
            {
                Dictionary<string, int> dic = new Dictionary<string, int>();

                int nbLivreEmp = db.Database.SqlQuery<int>("SELECT COUNT(id_emprunt) FROM Emprunt WHERE dt_retour IS NULL AND id_membre =" + id_membre).First();
                int nbLivreRetard = db.Database.SqlQuery<int>("SELECT COUNT(id_emprunt) FROM Emprunt WHERE DATEDIFF(DAY, dt_pret, GETDATE()) > 3 AND dt_retour IS NULL AND id_membre =" + id_membre).First();

                dic.Add("nbLivreEmp", nbLivreEmp);
                dic.Add("nbLivreRetard", nbLivreRetard);

                return (dic);
            }

        public static Dictionary<string, double> detailsRetard(int id_livre)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();

            //SELECT TOP 1 DATEDIFF(DAY, dt_pret+3, GETDATE()) FROM Emprunt WHERE id_livre = id_livre ORDER BY dt_pret DESC).First();

            int joursRetard = db.Database.SqlQuery<int>("SELECT TOP 1 DATEDIFF(DAY, (dt_pret + 3), GETDATE()) FROM Emprunt WHERE id_livre = {0} ORDER BY dt_pret DESC", id_livre).First();

            double fraiRetard = joursRetard * 1.5;

            dic.Add("joursRetard", joursRetard);
            dic.Add("fraiRetard", fraiRetard);

            return (dic);
        }

    }
}