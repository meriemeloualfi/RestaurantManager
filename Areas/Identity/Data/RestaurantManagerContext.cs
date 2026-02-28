using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models; // Très important pour voir tes classes

namespace RestaurantManager.Data;

public class RestaurantManagerContext : IdentityDbContext<IdentityUser>
{
    public RestaurantManagerContext(DbContextOptions<RestaurantManagerContext> options)
        : base(options)
    {
    }

    // On ajoute ici toutes les tables de ton diagramme
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Employe> Employes { get; set; }
    public DbSet<Tache> Taches { get; set; }
    public DbSet<Commande> Commandes { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Vente> Ventes { get; set; }
    public DbSet<Facture> Factures { get; set; }
    public DbSet<Plat> Plats { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<PlatCommande> PlatsCommandes { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Ici on pourrait ajouter des configurations spéciales si besoin
    }
}