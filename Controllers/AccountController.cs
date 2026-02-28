using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class AccountController : Controller
{
    public IActionResult RedirectUser()
    {
        if (User.IsInRole("Gérant"))
        {
            return RedirectToAction("Index", "Gerant"); // On créera ce dashboard après
        }
        else if (User.IsInRole("Serveur"))
        {
            return RedirectToAction("Index", "Serveur");
        }
        else if (User.IsInRole("ChefCuisinier"))
        {
            return RedirectToAction("Index", "Chef");
        }

        return RedirectToAction("Index", "Home");
    }
}