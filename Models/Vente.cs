using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class Vente
    {
        [Key]
        public int IdVente { get; set; }
        public DateTime DateVente { get; set; } = DateTime.Now;

        public float MontantBrut { get; set; } // Prix total avant remise
        public float Remise { get; set; }      // Montant de la réduction (ex: 10.00 DH)
        public float MontantTotal { get; set; } // Montant final payé (Brut - Remise)

        public int CommandeId { get; set; }
        public virtual Commande? Commande { get; set; }
        public virtual Facture? Facture { get; set; }
    }
}