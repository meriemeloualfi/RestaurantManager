using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class PlatCommande
    {
        [Key]
        public int Id { get; set; }

        public int CommandeId { get; set; }
        public virtual required Commande Commande { get; set; }

        public int PlatId { get; set; }
        public virtual required Plat Plat { get; set; }

        public int Quantite { get; set; }

        // On stocke le prix au moment de la vente (au cas où le prix du menu change plus tard)
        public float PrixUnitaire { get; set; }
        public string? NoteSpecial { get; set; } // Pour "Sans oignons", "Bien cuit", etc.

    }
}