using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models // Vérifie bien que c'est Manager et pas App
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }

        // On ajoute = string.Empty; pour éviter les avertissements "non-nullable"
        public string Nom { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Relations : On utilise ? pour dire que ça peut être vide au début
        // Et on initialise avec une nouvelle liste par défaut
        public virtual ICollection<Commande>? Commandes { get; set; } = new List<Commande>();
        public virtual ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
    }
}