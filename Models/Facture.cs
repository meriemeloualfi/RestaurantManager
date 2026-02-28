
using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class Facture
    {
        [Key]
        public int IdFacture { get; set; }
        public DateTime DateEmission { get; set; }
        public float Montant { get; set; }

        public int VenteId { get; set; }
        // On ajoute ? pour dire que cette propriété peut être nulle au début
        public virtual Vente? Vente { get; set; }
    }
}