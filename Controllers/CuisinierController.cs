using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;
using System.Security.Claims;

namespace RestaurantManager.Controllers
{
    [Authorize(Roles = "ChefCuisinier")]
    public class CuisinierController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public CuisinierController(RestaurantManagerContext context)
        {
            _context = context;
        }

        // --- 1. DASHBOARD DU CUISINIER ---
        public async Task<IActionResult> Index()
        {
            var userEmail = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login", "Account");

            var employe = await _context.Employes.FirstOrDefaultAsync(e => e.Email == userEmail);
            if (employe == null) return NotFound();

            var viewModel = new ChefDashboardViewModel
            {
                IngredientsAlerte = await _context.Ingredients
                    .Where(i => i.QuantiteDisponible <= i.SeuilAlerte)
                    .ToListAsync(),

                // Sur le dashboard, on ne montre que ce qui n'est pas encore fini
                MesTaches = await _context.Taches
                    .Where(t => t.EmployeId == employe.IdEmploye && t.Statut != "Terminé")
                    .OrderByDescending(t => t.DateDebut)
                    .ToListAsync(),

                NbCommandesEnAttente = 12 // À lier à votre table Commandes plus tard
            };

            return View(viewModel);
        }

        // --- 2. GESTION DU STOCK ---

        public async Task<IActionResult> Stock()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            return View(ingredients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConsommerIngredient(int id, float quantiteUtilisee)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return NotFound();

            if (ingredient.QuantiteDisponible >= quantiteUtilisee)
            {
                ingredient.QuantiteDisponible -= quantiteUtilisee;
                _context.Update(ingredient);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Stock mis à jour pour {ingredient.Nom}.";
            }
            else
            {
                TempData["Error"] = $"Stock insuffisant pour {ingredient.Nom} !";
            }

            return RedirectToAction(nameof(Stock));
        }

        // --- 3. GESTION DES TÂCHES (PAGE DÉDIÉE) ---

        public async Task<IActionResult> Taches()
        {
            var userEmail = User?.Identity?.Name;
            var employe = await _context.Employes.FirstOrDefaultAsync(e => e.Email == userEmail);
            if (employe == null) return NotFound();

            // Ici on récupère TOUTES les tâches (même terminées) pour les stats de la page
            var toutesLesTaches = await _context.Taches
                .Where(t => t.EmployeId == employe.IdEmploye)
                .OrderByDescending(t => t.DateDebut)
                .ToListAsync();

            var viewModel = new ChefDashboardViewModel
            {
                MesTaches = toutesLesTaches,
                // On peut remplir le reste si nécessaire pour la vue
                IngredientsAlerte = Enumerable.Empty<Ingredient>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MettreEnCours(int id)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache == null) return NotFound();

            tache.Statut = "En cours";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Taches)); // Redirige vers la liste des tâches
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TerminerTache(int id)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache == null) return NotFound();

            tache.Statut = "Terminé";
            tache.DateFin = DateTime.Now;
            // Si votre modèle a une propriété booléenne :
            // tache.EstTerminee = true; 

            _context.Update(tache);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tâche terminée avec succès !";
            return RedirectToAction(nameof(Taches));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(m => m.IdIngredient == id);

            if (ingredient == null) return NotFound();

            return View(ingredient);
        }
    }
}