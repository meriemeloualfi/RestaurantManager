using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data; // Remplacez par votre namespace Data
using RestaurantManager.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManager.Controllers
{
    public class GerantController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public GerantController(RestaurantManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Calcul des ventes RÉELLES à partir de la table Vente
            // On additionne le 'MontantTotal' pour toutes les ventes d'aujourd'hui
            var ventesDuJour = await _context.Ventes
                .Where(v => v.DateVente.Date == DateTime.Today)
                .SumAsync(v => (float?)v.MontantTotal) ?? 0f;

            // 2. Nombre de commandes ACTIVES (qui n'ont pas encore de vente ou statut non terminé)
            // On compte les commandes dont le statut est "En cours", "Prête" ou "Servie"
            var nbCommandesActives = await _context.Commandes
                .CountAsync(c => c.Statut != "Terminé" && c.Statut != "Annulé");

            // 3. Alertes Stock (Ingrédients sous le seuil)
            var alertesStock = await _context.Ingredients
                .Where(i => i.QuantiteDisponible <= i.SeuilAlerte)
                .ToListAsync();

            var viewModel = new GerantDashboardViewModel
            {
                VentesDuJour = (decimal)ventesDuJour, // On cast en decimal pour l'affichage
                NbCommandesActives = nbCommandesActives,
                AlertesStock = alertesStock
            };

            return View(viewModel);
        }
    }
}