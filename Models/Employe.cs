using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RestaurantManager.Models
{
    public class Employe
    {
        internal readonly object Prenom;

        [Key]
        public int IdEmploye { get; set; }

        public string Nom { get; set; } = string.Empty;
        public string Poste { get; set; } = string.Empty;
        public string Horaire { get; set; } = string.Empty;
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = "passwd123!";

        // Relations
        public int? RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

        public virtual ICollection<Tache> Taches { get; set; } = new List<Tache>();
    }
}