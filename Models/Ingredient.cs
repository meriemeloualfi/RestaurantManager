using System.ComponentModel.DataAnnotations; // <--- TRÈS IMPORTANT

namespace RestaurantManager.Models
{
    public class Ingredient
    {
        [Key] // <--- AJOUTE CECI pour dire à EF que c'est la clé primaire
        public int IdIngredient { get; set; }

        public string Nom { get; set; } = string.Empty;
        public float QuantiteDisponible { get; set; }
        public string Unite { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public float SeuilAlerte { get; set; }

        public int? StockId { get; set; }
        public virtual Stock? Stock { get; set; }
        public int Quantite { get; internal set; }
    }
}