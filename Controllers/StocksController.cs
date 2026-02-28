using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Controllers
{
    public class StocksController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public StocksController(RestaurantManagerContext context)
        {
            _context = context;
        }

        // C'est ici que le lien "Stocks" de la sidebar va pointer
        public async Task<IActionResult> Index()
        {
            // On récupère tous les ingrédients pour voir l'état global du stock
            var ingredients = await _context.Ingredients.ToListAsync();
            return View(ingredients);
        }

        // GET: Stocks/Create (Pour ajouter un nouvel ingrédient au stock)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdIngredient,Nom,QuantiteDisponible,Unite,SeuilAlerte")] Ingredient ingredient, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // GESTION DE L'IMAGE
                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ingredients");

                    // Créer le dossier s'il n'existe pas
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    ingredient.ImagePath = fileName;
                }

                // Lier au stock par défaut
                var stock = await _context.Stocks.FirstOrDefaultAsync();
                if (stock != null) ingredient.StockId = stock.IdStock;

                _context.Add(ingredient);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ingrédient ajouté !";
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        // Ajoute ici les méthodes Edit et Delete plus tard...
        [HttpPost]
        public async Task<IActionResult> AjusterStock(int id, float quantite)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                // Cette ligne permet d'ajouter ou de soustraire
                ingredient.QuantiteDisponible += quantite;

                if (ingredient.QuantiteDisponible < 0) ingredient.QuantiteDisponible = 0;

                await _context.SaveChangesAsync();
                TempData["Success"] = $"Le stock de {ingredient.Nom} est maintenant de {ingredient.QuantiteDisponible} {ingredient.Unite}";
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // On cherche l'ingrédient par son ID
            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(m => m.IdIngredient == id);

            if (ingredient == null) return NotFound();

            // On envoie l'ingrédient à la vue
            return View(ingredient);
        }
        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return NotFound();

            return View(ingredient);
        }

        // POST: Stocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient ingredient, IFormFile? imageFile)
        {
            if (id != ingredient.IdIngredient) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Gestion de l'image si un nouveau fichier est téléchargé
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ingredients", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        ingredient.ImagePath = fileName; // On met à jour le chemin
                    }
                    else
                    {
                        // Conserver l'ancienne image si aucune nouvelle n'est choisie
                        _context.Entry(ingredient).Property(x => x.ImagePath).IsModified = false;
                    }

                    _context.Update(ingredient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.IdIngredient)) return NotFound();
                    else throw;
                }
            }
            return View(ingredient);
        }

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.IdIngredient == id);
        }
        // GET: Stocks/Delete/5
        // Affiche la page de confirmation de suppression
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(m => m.IdIngredient == id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // POST: Stocks/Delete/5
        // Exécute réellement la suppression après confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient != null)
            {
                // 1. Supprimer l'image physiquement du serveur si elle existe
                if (!string.IsNullOrEmpty(ingredient.ImagePath))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ingredients", ingredient.ImagePath);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // 2. Supprimer l'entrée de la base de données
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
            }

            // 3. Retourner à la liste principale
            return RedirectToAction(nameof(Index));
        }
    }
}