using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
    public class Commande
    {
        [Key]
        public int IdCommande { get; set; }

        public DateTime DateCommande { get; set; } = DateTime.Now;

        [Required]
        public string Statut { get; set; } = "En attente";

        [Required]
        public string TypeCommande { get; set; } = "Sur place";

        public int? ClientId { get; set; }

        public int? NumeroTable { get; set; }

        // --- NOUVEAU : Lien avec le Serveur (Employé) ---
        public int? EmployeId { get; set; }

        [ForeignKey("EmployeId")]
        public virtual Employe? Employe { get; set; }
        // -----------------------------------------------

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        public virtual Vente? Vente { get; set; }

        public virtual ICollection<PlatCommande> PlatsCommandes { get; set; } = new List<PlatCommande>();

        [NotMapped] // Cela dit à Entity Framework de ne pas chercher cette colonne en SQL
        public float Total { get; set; }
    }
}