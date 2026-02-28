using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManager.Controllers
{
    // Correction ici : On autorise le Gérant et l'Admin à accéder au contrôleur 
    // pour qu'ils puissent voir les factures.
    [Authorize(Roles = "Serveur,Gérant,Admin")]
    public class ServeurController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public ServeurController(RestaurantManagerContext context)
        {
            _context = context;
        }

        // --- DASHBOARD DU SERVEUR ---
        public async Task<IActionResult> Index()
        {
            var userEmail = User?.Identity?.Name;
            var employe = await _context.Employes.FirstOrDefaultAsync(e => e.Email == userEmail);

            // Correction : Vérifier que User n'est pas null avant d'appeler IsInRole
            if (employe == null && User != null && User.IsInRole("Gerant"))
                return RedirectToAction("Index", "Gerant");

            if (employe == null) return NotFound("Employé non trouvé");

            var today = DateTime.Today;

            var viewModel = new ServeurDashboardViewModel
            {
                MesTaches = await _context.Taches
                    .Where(t => t.EmployeId == employe.IdEmploye && t.Statut != "Terminé")
                    .ToListAsync(),

                ReservationsDuJour = await _context.Reservations
                    .Where(r => r.DateReservation.Date == today)
                    .OrderBy(r => r.DateReservation)
                    .ToListAsync(),

                NbCommandesPrises = await _context.Commandes
                    .Where(c => c.DateCommande.Date == today)
                    .CountAsync()
            };

            return View(viewModel);
        }

        // --- GESTION DES TACHES ---
        public async Task<IActionResult> MesTaches()
        {
            var userEmail = User?.Identity?.Name;
            var employe = await _context.Employes.FirstOrDefaultAsync(e => e.Email == userEmail);
            if (employe == null) return NotFound();

            var taches = await _context.Taches
                .Where(t => t.EmployeId == employe.IdEmploye)
                .OrderByDescending(t => t.DateDebut)
                .ToListAsync();

            return View(taches);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatut(int id, string nouveauStatut)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache != null)
            {
                tache.Statut = nouveauStatut;
                tache.EstTerminee = (nouveauStatut == "Terminé");
                if (tache.EstTerminee) tache.DateFin = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(MesTaches));
        }

        // --- PRISE DE COMMANDE ---
        public async Task<IActionResult> PrendreCommande()
        {
            var plats = await _context.Plats.ToListAsync();
            return View(plats);
        }

        [HttpPost]
        public async Task<IActionResult> ValiderCommande([FromBody] CommandeRequest request)
        {
            if (request == null || request.Articles == null || !request.Articles.Any())
                return Json(new { success = false, message = "Le ticket est vide." });

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nouvelleCommande = new Commande
                {
                    DateCommande = DateTime.Now,
                    TypeCommande = request.Type,
                    Statut = "En préparation",
                    NumeroTable = request.NumeroTable,
                    PlatsCommandes = new List<PlatCommande>()
                };

                foreach (var art in request.Articles)
                {
                    var plat = await _context.Plats.FindAsync(art.PlatId);
                    if (plat == null)
                        return Json(new { success = false, message = $"Plat avec l'ID {art.PlatId} introuvable." });

                    var platCommande = new PlatCommande
                    {
                        Commande = nouvelleCommande,
                        Plat = plat,
                        PlatId = art.PlatId,
                        Quantite = art.Qte,
                        PrixUnitaire = art.Prix,
                        NoteSpecial = art.Note ?? ""
                    };
                    nouvelleCommande.PlatsCommandes.Add(platCommande);
                }

                var vente = new Vente
                {
                    Commande = nouvelleCommande,
                    DateVente = DateTime.Now,
                    MontantBrut = request.MontantBrut,
                    Remise = request.Remise,
                    MontantTotal = request.MontantTotal
                };

                var facture = new Facture
                {
                    Vente = vente,
                    DateEmission = DateTime.Now,
                    Montant = request.MontantTotal
                };

                _context.Commandes.Add(nouvelleCommande);
                _context.Ventes.Add(vente);
                _context.Factures.Add(facture);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true, idFacture = facture.IdFacture });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var detailError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Json(new { success = false, message = "Erreur SQL : " + detailError });
            }
        }

        // --- AFFICHAGE FACTURE (CORRIGÉ) ---
        public async Task<IActionResult> VoirFacture(int id)
        {
            // On tente de trouver la facture par son propre ID
            var facture = await _context.Factures
                .Include(f => f.Vente)
                    .ThenInclude(v => v.Commande)
                        .ThenInclude(c => c.PlatsCommandes)
                            .ThenInclude(pc => pc.Plat)
                .FirstOrDefaultAsync(f => f.IdFacture == id);

            // Si non trouvé (par exemple si on a cliqué sur un ID de vente), on cherche par l'ID de vente
            if (facture == null)
            {
                facture = await _context.Factures
                    .Include(f => f.Vente)
                        .ThenInclude(v => v.Commande)
                            .ThenInclude(c => c.PlatsCommandes)
                                .ThenInclude(pc => pc.Plat)
                    .FirstOrDefaultAsync(f => f.Vente != null && f.Vente.IdVente == id);
            }

            if (facture == null) return NotFound("Désolé, la facture n'existe pas.");

            // Déterminer le nom à afficher sur la facture
            var userEmail = User?.Identity?.Name;
            var employe = await _context.Employes.FirstOrDefaultAsync(e => e.Email == userEmail);
            ViewBag.NomServeur = employe != null ? $"{employe.Nom}" : "Administrateur";

            return View(facture);
        }

        public async Task<IActionResult> MesVentes()
        {
            var ventes = await _context.Ventes
                .Include(v => v.Commande)
                .Include(v => v.Facture)
                .OrderByDescending(v => v.DateVente)
                .ToListAsync();

            return View(ventes);
        }

        public async Task<IActionResult> ListeReservations()
        {
            var reservations = await _context.Reservations
                .OrderBy(r => r.DateReservation)
                .ToListAsync();

            return View(reservations);
        }
    }

    public class CommandeRequest
    {
        public string Type { get; set; } = "Sur place";
        public int? NumeroTable { get; set; }
        public float MontantBrut { get; set; }
        public float Remise { get; set; }
        public float MontantTotal { get; set; }
        public List<ArticleRequest> Articles { get; set; } = new();
    }

    public class ArticleRequest
    {
        public int PlatId { get; set; }
        public string Nom { get; set; } = "";
        public float Prix { get; set; }
        public int Qte { get; set; }
        public string Note { get; set; } = "";
    }
}