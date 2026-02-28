using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;
using System.IO; // INDISPENSABLE pour gérer les fichiers
using Microsoft.AspNetCore.Hosting; // INDISPENSABLE pour IWebHostEnvironment

namespace RestaurantManager.Controllers
{
    public class PlatsController : Controller
    {
        private readonly RestaurantManagerContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // Pour gérer le chemin des images

        public PlatsController(RestaurantManagerContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Plats
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plats.ToListAsync());
        }

        // GET: Plats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var plat = await _context.Plats.FirstOrDefaultAsync(m => m.IdPlat == id);
            if (plat == null) return NotFound();

            return View(plat);
        }

        // GET: Plats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlat,Nom,Prix,Description")] Plat plat, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Gestion de l'upload de l'image
                if (imageFile != null && imageFile.Length > 0)
                {
                    // 1. Créer un nom de fichier unique (Guid) pour éviter d'écraser des fichiers de même nom
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                    // 2. Définir le chemin de stockage : wwwroot/images/plats
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string path = Path.Combine(wwwRootPath, "images", "plats");

                    // Créer le dossier s'il n'existe pas
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    // 3. Enregistrer physiquement le fichier
                    string fullPath = Path.Combine(path, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // 4. Enregistrer le nom du fichier dans l'objet plat
                    plat.ImagePath = fileName;
                }

                _context.Add(plat);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Le plat '{plat.Nom}' a été ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }
            return View(plat);
        }

        // GET: Plats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plat = await _context.Plats.FindAsync(id);
            if (plat == null) return NotFound();

            return View(plat);
        }

        // POST: Plats/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPlat,Nom,Prix,Description,ImagePath")] Plat plat, IFormFile? imageFile)
        {
            if (id != plat.IdPlat) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // GESTION DE L'IMAGE
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // 1. Créer un nom de fichier unique
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                        // 2. Chemin vers wwwroot/images/plats
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/plats", fileName);

                        // 3. Enregistrer le fichier physiquement
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // 4. Mettre à jour le nom de l'image dans l'objet plat
                        plat.ImagePath = fileName;
                    }

                    _context.Update(plat);
                    await _context.SaveChangesAsync();

                    // MESSAGE POUR LE TOAST
                    TempData["Success"] = "Le plat '" + plat.Nom + "' a été modifié avec succès !";

                    return RedirectToAction(nameof(Index)); // On redirige vers le catalogue
                }
                catch (DbUpdateConcurrencyException) { /* ... */ }
            }
            return View(plat);
        }

        // GET: Plats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var plat = await _context.Plats.FirstOrDefaultAsync(m => m.IdPlat == id);
            if (plat == null) return NotFound();

            return View(plat);
        }

        // POST: Plats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plat = await _context.Plats.FindAsync(id);
            if (plat != null)
            {
                // Supprimer physiquement l'image du serveur avant de supprimer le plat
                if (!string.IsNullOrEmpty(plat.ImagePath))
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "plats", plat.ImagePath);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }

                _context.Plats.Remove(plat);
            }

            await _context.SaveChangesAsync();
            TempData["DeleteSuccess"] = "Le plat a été définitivement supprimé.";
            return RedirectToAction(nameof(Index));
        }

        private bool PlatExists(int id)
        {
            return _context.Plats.Any(e => e.IdPlat == id);
        }
    }
}