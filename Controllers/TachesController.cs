using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Nécessaire pour Include et FirstOrDefaultAsync
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Controllers
{
    public class TachesController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public TachesController(RestaurantManagerContext context)
        {
            _context = context;
        }

        // --- CREATE ---
        public IActionResult Create(int employeId)
        {
            ViewBag.EmployeId = employeId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,DateFin,EmployeId")] Tache tache)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tache);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Employes", new { id = tache.EmployeId });
            }
            return View(tache);
        }

        // --- EDIT (Nouvelle page pour modifier) ---
        // GET: Taches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tache = await _context.Taches.FindAsync(id);
            if (tache == null) return NotFound();

            return View(tache);
        }

        // POST: Taches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTache,Description,DateFin,EstTerminee,EmployeId")] Tache tache)
        {
            if (id != tache.IdTache) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tache);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Taches.Any(e => e.IdTache == id)) return NotFound();
                    else throw;
                }
                // Redirige vers la fiche de l'employé
                return RedirectToAction("Details", "Employes", new { id = tache.EmployeId });
            }
            return View(tache);
        }

        // --- DELETE (Nouvelle page de confirmation) ---
        // GET: Taches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // On inclut l'employé pour afficher son nom sur la page de confirmation si besoin
            var tache = await _context.Taches
                .Include(t => t.Employe)
                .FirstOrDefaultAsync(m => m.IdTache == id);

            if (tache == null) return NotFound();

            return View(tache);
        }

        // POST: Taches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache != null)
            {
                int empId = tache.EmployeId;
                _context.Taches.Remove(tache);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Employes", new { id = empId });
            }
            return RedirectToAction("Index", "Employes");
        }

        // --- TOGGLE COMPLETE ---
        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache == null) return NotFound();

            tache.EstTerminee = !tache.EstTerminee;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Employes", new { id = tache.EmployeId });
        }
    }
}