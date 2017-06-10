using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MVC_EF_LAB1_GestionLivres.Models;

namespace MVC_EF_LAB1_GestionLivres.ViewModel
{
    public class DetailEmprunt
    {
        [key]
        public int id_membre { get; set; } //Emprunt

        public string prenom_membre { get; set; } //Emprunt

        public string nom_membre { get; set; }//Emprunt

        public int total_emprunt { get; set; } // group by id_membre COUNT

        public int nb_livreNonRetourne { get; set; } // dt_retour = null

       public virtual Emprunt Emprunt { get; set; }
       public virtual ICollection<Emprunt> Emprunts { get; set; }

    }
}