
using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class Stock
    {
        [Key]
        public int IdStock { get; set; }

        // On initialise avec la date actuelle par défaut
        public DateTime DateMaj { get; set; } = DateTime.Now;

        // On ajoute le ? et on initialise la liste
        public virtual ICollection<Ingredient>? Ingredients { get; set; } = new List<Ingredient>();
    }
}