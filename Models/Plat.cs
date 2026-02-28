
using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class Plat
    {
        [Key]
        public int IdPlat { get; set; }
        public string Nom { get; set; } = string.Empty;
        public float Prix { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; } // Contiendra le nom de l'image (ex: pizza.jpg)
        public virtual ICollection<Commande>? Commandes { get; set; }
        public virtual ICollection<Ingredient>? Ingredients { get; set; }
    }
}