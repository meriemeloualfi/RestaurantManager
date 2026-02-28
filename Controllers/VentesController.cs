using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManager.Controllers
{
    public class VentesController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public VentesController(RestaurantManagerContext context)
        {
            _context = context;
        }

        // 1. Liste des ventes
        public async Task<IActionResult> Index()
        {
            var ventes = await _context.Ventes
                .Include(v => v.Commande)
                .OrderByDescending(v => v.DateVente)
                .ToListAsync();
            return View(ventes);
        }

        // 2. Formulaire de création (GET)
        public IActionResult Create()
        {
            // On charge la liste des IDs de commandes pour aider l'utilisateur
            // On affiche l'ID, mais on pourrait afficher "Commande #105 - Client X"
            ViewBag.CommandeId = new SelectList(_context.Commandes, "IdCommande", "IdCommande");
            return View();
        }

        // 3. Action de création (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MontantBrut,Remise,CommandeId")] Vente vente)
        {
            // --- ÉTAPE 1 : Vérifier si la commande existe en base de données ---
            var commandeExiste = await _context.Commandes.AnyAsync(c => c.IdCommande == vente.CommandeId);

            if (!commandeExiste)
            {
                ModelState.AddModelError("CommandeId", $"La commande n°{vente.CommandeId} n'existe pas.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Calcul automatique du total
                    vente.MontantTotal = (vente.MontantBrut) - (vente.Remise);
                    vente.DateVente = DateTime.Now;

                    // Sauvegarde de la vente
                    _context.Add(vente);
                    await _context.SaveChangesAsync();

                    // Génération de la facture liée
                    var facture = new Facture
                    {
                        VenteId = vente.IdVente,
                        DateEmission = DateTime.Now,
                        Montant = vente.MontantTotal
                    };

                    _context.Add(facture);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erreur lors de l'enregistrement : " + ex.Message);
                }
            }

            // Si on arrive ici, c'est qu'il y a eu un problème : on recharge la liste pour la vue
            ViewBag.CommandeId = new SelectList(_context.Commandes, "IdCommande", "IdCommande", vente.CommandeId);
            return View(vente);
        }

        // 4. Transformer une commande en vente (Paiement direct)
        public async Task<IActionResult> FinaliserDepuisCommande(int commandeId)
        {
            var commande = await _context.Commandes
                .Include(c => c.PlatsCommandes)
                .FirstOrDefaultAsync(c => c.IdCommande == commandeId);

            if (commande == null) return NotFound();

            // Calcul du total basé sur les plats (Conversion float/decimal selon votre modèle)
            float total = commande.PlatsCommandes.Sum(pc => pc.PrixUnitaire * pc.Quantite);

            var vente = new Vente
            {
                CommandeId = commandeId,
                MontantBrut = total,
                Remise = 0,
                MontantTotal = total
            };

            // On renvoie vers la vue Create pré-remplie
            ViewBag.CommandeId = new SelectList(_context.Commandes, "IdCommande", "IdCommande", vente.CommandeId);
            return View("Create", vente);
        }

        // 5. Affichage de la facture
        public async Task<IActionResult> Facture(int id)
        {
            var facture = await _context.Factures
                .Include(f => f.Vente)
                    .ThenInclude(v => v.Commande)
                .FirstOrDefaultAsync(f => f.IdFacture == id);

            if (facture == null) return NotFound();

            return View(facture);
        }
    }
}