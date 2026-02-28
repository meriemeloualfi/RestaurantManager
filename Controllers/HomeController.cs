using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.Models;

namespace RestaurantManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // On vérifie d'abord si l'utilisateur est connecté
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // 1. Redirection pour le GÉRANT
                if (User.IsInRole("Gérant"))
                {
                    return RedirectToAction("Index", "Gerant");
                }

                // 2. Redirection pour le CHEF CUISINIER
                if (User.IsInRole("ChefCuisinier"))
                {
                    return RedirectToAction("Index", "Cuisinier");
                }

                // 3. Redirection pour le SERVEUR
                if (User.IsInRole("Serveur"))
                {
                    return RedirectToAction("Index", "Serveur"); // Assure-toi d'avoir un ServeurController
                }
            }

            // Si l'utilisateur n'est pas connecté ou n'a pas de rôle spécifique, 
            // il voit la page d'accueil par défaut (le plat rond / landing page)
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}