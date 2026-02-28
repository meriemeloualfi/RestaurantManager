using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public ReservationsController(RestaurantManagerContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations
                .OrderByDescending(r => r.DateReservation)
                .ToListAsync();
            return View(reservations);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.IdReservation == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // AJOUT : NumeroTable dans le Bind
        public async Task<IActionResult> Create([Bind("NomClient,NumeroTable,DateReservation,Heure,NbPersonnes,Statut,Remarques")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(reservation.Statut))
                {
                    reservation.Statut = "Confirmée";
                }

                _context.Add(reservation);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Réservation pour {reservation.NomClient} (Table {reservation.NumeroTable}) enregistrée !";
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // AJOUT : NumeroTable dans le Bind
        public async Task<IActionResult> Edit(int id, [Bind("IdReservation,NomClient,NumeroTable,DateReservation,Heure,NbPersonnes,Statut,Remarques")] Reservation reservation)
        {
            if (id != reservation.IdReservation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"La réservation de {reservation.NomClient} a été mise à jour.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.IdReservation))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.IdReservation == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "La réservation a été supprimée.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.IdReservation == id);
        }
    }
}