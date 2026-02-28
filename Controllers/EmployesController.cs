using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Controllers
{
    public class EmployesController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public EmployesController(RestaurantManagerContext context)
        {
            _context = context;
        }

        // GET: Employes
        public async Task<IActionResult> Index()
        {
            // On charge les restos et les tâches pour pouvoir afficher des compteurs (ex: 2/5 finies) dans la liste
            var employes = await _context.Employes
                .Include(e => e.Restaurant)
                .Include(e => e.Taches)
                .ToListAsync();
            return View(employes);
        }

        // GET: Employes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // MODIF : On trie les tâches directement lors du chargement (les plus récentes en premier)
            var employe = await _context.Employes
                .Include(e => e.Restaurant)
                .Include(e => e.Taches.OrderByDescending(t => t.DateDebut))
                .FirstOrDefaultAsync(m => m.IdEmploye == id);

            if (employe == null) return NotFound();

            return View(employe);
        }

        // GET: Employes/Create
        public IActionResult Create()
        {
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Nom"); // Utiliser "Nom" pour l'affichage
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Poste,Horaire,RestaurantId,Email")] Employe employe, IFormFile? imageFile)
        {
            // Sécurité : Mot de passe par défaut pour le premier accès
            employe.Password = "passwd123!";
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/employes");
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    employe.ImagePath = fileName;
                }

                _context.Add(employe);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"L'employé {employe.Nom} a été créé. Identifiant : {employe.Email}";
                return RedirectToAction(nameof(Index));
            }

            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Nom", employe.RestaurantId);
            return View(employe);
        }

        // GET: Employes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employe = await _context.Employes.FindAsync(id);
            if (employe == null) return NotFound();

            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Nom", employe.RestaurantId);
            return View(employe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmploye,Nom,Poste,Horaire,RestaurantId,ImagePath,Email")] Employe employe, IFormFile? imageFile)
        {
            if (id != employe.IdEmploye) return NotFound();

            // Récupérer l'employé actuel pour préserver le Password et l'ImagePath
            var employeDb = await _context.Employes.AsNoTracking().FirstOrDefaultAsync(e => e.IdEmploye == id);
            if (employeDb == null) return NotFound();

            // Retirer le mot de passe de la validation car il n'est pas dans le formulaire
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/employes");
                        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                        // Supprimer l'ancienne image
                        if (!string.IsNullOrEmpty(employeDb.ImagePath))
                        {
                            string oldPath = Path.Combine(folderPath, employeDb.ImagePath);
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string filePath = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        employe.ImagePath = fileName;
                    }
                    else
                    {
                        // Garder l'ancienne image si aucune nouvelle n'est choisie
                        employe.ImagePath = employeDb.ImagePath;
                    }

                    // RÉ-ASSIGNER LE MOT DE PASSE (Très important)
                    employe.Password = employeDb.Password;

                    _context.Update(employe);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Modifications enregistrées pour {employe.Nom}.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeExists(employe.IdEmploye)) return NotFound();
                    else throw;
                }
            }

            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Nom", employe.RestaurantId);
            return View(employe);
        }

        // GET: Employes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employe = await _context.Employes
                .Include(e => e.Restaurant)
                .FirstOrDefaultAsync(m => m.IdEmploye == id);

            if (employe == null) return NotFound();

            return View(employe);
        }

        // POST: Employes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employe = await _context.Employes.FindAsync(id);
            if (employe != null)
            {
                // MODIF : Supprimer physiquement la photo du serveur avant de supprimer l'employé
                if (!string.IsNullOrEmpty(employe.ImagePath))
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/employes", employe.ImagePath);
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                }

                _context.Employes.Remove(employe);
                await _context.SaveChangesAsync();
                TempData["DeleteSuccess"] = $"L'employé {employe.Nom} a été supprimé définitivement.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmployeExists(int id)
        {
            return _context.Employes.Any(e => e.IdEmploye == id);
        }
    }
}