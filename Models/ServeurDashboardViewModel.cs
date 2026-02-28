namespace RestaurantManager.Models
{
    public class ServeurDashboardViewModel
    {
        public List<Tache> MesTaches { get; set; } = new();
        public List<Reservation> ReservationsDuJour { get; set; } = new();
        public int NbCommandesPrises { get; set; }
    }
}