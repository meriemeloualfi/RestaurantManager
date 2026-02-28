using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
    public class Tache
    {
        [Key]
        public int IdTache { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime DateDebut { get; set; } = DateTime.Now;
        public DateTime DateFin { get; set; }

        public int EmployeId { get; set; }
        public bool EstTerminee { get; set; } = false;

        // CHANGEMENT ICI : On ajoute le ? pour le rendre nullable
        // Cela permet aux anciennes tâches de ne pas avoir d'utilisateur
        public string? UserId { get; set; }

        public virtual IdentityUser? User { get; set; }

        public virtual Employe? Employe { get; set; }

        public string? Statut { get; set; }

        [NotMapped]
        public object? Id { get; set; }
    }
}