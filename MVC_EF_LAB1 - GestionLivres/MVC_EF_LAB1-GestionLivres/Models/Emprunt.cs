//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_EF_LAB1_GestionLivres.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Emprunt
    {
        internal object Database;
        internal object Adress;

        [Key]
        [DisplayName("Code d'emprunt")]
        public int id_emprunt { get; set; }

        [DisplayName("Code de membre")]
        public int id_membre { get; set; }

        [DisplayName("Code de livre")]
        public int id_livre { get; set; }

        [Required]
        [DisplayName("Date d'Emprunt")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> dt_pret { get; set; }

        [DisplayName("Date de Retour")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> dt_retour { get; set; }
    
        public virtual Livre Livre { get; set; }

        public virtual Membre Membre { get; set; }

    }
}
