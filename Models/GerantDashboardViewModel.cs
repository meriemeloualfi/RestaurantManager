namespace RestaurantManager.Models
{
    public class GerantDashboardViewModel
    {
        public decimal VentesDuJour { get; set; }
        public int NbCommandesActives { get; set; }
        public required List<Ingredient> AlertesStock { get; set; }
    }
}