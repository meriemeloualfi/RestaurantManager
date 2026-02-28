using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        // On initialise avec string.Empty pour éviter les alertes jaunes
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;

        // Relations : On utilise ? et on crée une nouvelle liste par défaut
        public virtual ICollection<Employe>? Employes { get; set; } = new List<Employe>();
        public virtual ICollection<Commande>? Commandes { get; set; } = new List<Commande>();
    }
}