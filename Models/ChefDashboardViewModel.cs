using RestaurantManager.Models; // Pour accéder à Ingredient et Tache

namespace RestaurantManager.Models
{
    public class ChefDashboardViewModel
    {
        public required IEnumerable<Ingredient> IngredientsAlerte { get; set; }
        public required IEnumerable<Tache> MesTaches { get; set; }
        public int NbCommandesEnAttente { get; set; }
    }
}