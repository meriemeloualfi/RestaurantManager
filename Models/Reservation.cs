using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class Reservation
    {
        [Key]
        public int IdReservation { get; set; }

        [Required]
        public string NomClient { get; set; } = string.Empty;

        // AJOUT LOGIQUE : Le numéro de la table assignée
        [Required(ErrorMessage = "Veuillez assigner une table")]
        [Display(Name = "Table assignée")]
        public int NumeroTable { get; set; }

        public DateTime DateReservation { get; set; } = DateTime.Now;
        public string Heure { get; set; } = string.Empty;
        public int NbPersonnes { get; set; }
        public string Statut { get; set; } = "Confirmée";
        public string? Remarques { get; set; }
    }
}