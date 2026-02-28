using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuration de la chaîne de connexion
var connectionString = builder.Configuration.GetConnectionString("RestaurantManagerContextConnection")
    ?? throw new InvalidOperationException("La chaîne de connexion 'RestaurantManagerContextConnection' est introuvable.");

// 2. Ajout du Contexte de la base de données
builder.Services.AddDbContext<RestaurantManagerContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Configuration de ASP.NET Core Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>() // Activation des rôles
.AddEntityFrameworkStores<RestaurantManagerContext>();

// 4. Ajout des services MVC et Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// --- ETAPE SPECIALE : SEEDING (Création automatique des Rôles et des Utilisateurs) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // A. Création des rôles
    string[] roleNames = { "Gérant", "Serveur", "ChefCuisinier" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // B. Création du Gérant (Admin)
    var adminEmail = "admin@restaurant.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        if ((await userManager.CreateAsync(admin, "Admin123!")).Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Gérant");
        }
    }

    // C. Création du Chef Cuisinier
    var chefEmail = "chef@restaurant.com";
    if (await userManager.FindByEmailAsync(chefEmail) == null)
    {
        var chef = new IdentityUser { UserName = chefEmail, Email = chefEmail, EmailConfirmed = true };
        if ((await userManager.CreateAsync(chef, "Chef123!")).Succeeded)
        {
            await userManager.AddToRoleAsync(chef, "ChefCuisinier");
        }
    }

    // D. Création du Serveur
    var serveurEmail = "serveur@restaurant.com";
    if (await userManager.FindByEmailAsync(serveurEmail) == null)
    {
        var serveur = new IdentityUser { UserName = serveurEmail, Email = serveurEmail, EmailConfirmed = true };
        if ((await userManager.CreateAsync(serveur, "Serveur123!")).Succeeded)
        {
            await userManager.AddToRoleAsync(serveur, "Serveur");
        }
    }
}
// --- FIN DU SEEDING ---

// 5. Configuration du pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 6. Définition des Routes
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();