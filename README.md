# Restaurant Manager

A web application for managing a restaurant's daily operations: dishes, ingredients and staff,
with secure user authentication. Built with ASP.NET Core MVC and Entity Framework Core.

## Features

- **Dish management**: create, view, edit and delete dishes, with images
- **Ingredient management**: track ingredients, with images
- **Employee management**: manage staff records, with photos
- **Authentication and authorization** with ASP.NET Core Identity (register, login, logout)
- Image upload and storage for dishes, ingredients and employees
- SQL Server database with Entity Framework Core migrations
- [Add or remove features: orders, stock alerts, roles (Admin / Staff), search, etc.]

## Tech Stack

| Category | Tools |
|----------|-------|
| Framework | ASP.NET Core MVC (.NET 10) |
| Language | C# |
| ORM | Entity Framework Core 10 |
| Database | SQL Server |
| Authentication | ASP.NET Core Identity (with Identity UI) |
| Front-end | Razor Views, HTML, CSS, JavaScript, [Bootstrap if used] |

## Project Structure

```
RestaurantManager/
├── Areas/Identity/     # Identity pages (login, register)
├── Controllers/        # MVC controllers
├── Models/             # Entities and view models
├── Views/              # Razor views
├── Migrations/         # EF Core migrations
├── wwwroot/
│   └── images/
│       ├── plats/        # Dish images
│       ├── ingredients/  # Ingredient images
│       └── employes/     # Employee photos
├── appsettings.json
├── Program.cs
└── RestaurantManager.sln
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express or full edition)
- Visual Studio 2022 or later, or VS Code

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/meriemeloualfi/RestaurantManager.git
   cd RestaurantManager
   ```

2. Configure the database connection in `appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RestaurantManager;Trusted_Connection=True;"
   }
   ```
   (Use the connection string name that matches your `Program.cs`.)

3. Apply the migrations to create the database
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   ```
   In Visual Studio, you can also run `Update-Database` in the Package Manager Console.

4. Run the application
   ```bash
   dotnet run
   ```
   Then open the URL shown in the terminal (for example `https://localhost:7xxx`).

## Usage

1. Register a new account, or log in.
2. Use the navigation menu to manage dishes, ingredients and employees.
3. Upload images when creating or editing an item.

## Screenshots

![Home](screenshots/home.png)
![Dishes](screenshots/dishes.png)

## Future Improvements

- Order and table management
- Stock tracking with low-stock alerts
- Role-based access (Admin, Manager, Staff)
- Dashboard with sales statistics

## Author

**Meriem El Oualfi**
GitHub: [@meriemeloualfi](https://github.com/meriemeloualfi)s
