using System.Text;
using BookStore.Api.Data;
using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using BookStore.Core.Services;
using BookStore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container. (enregistrer les services dans le conteneur des dépendances DI)
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddScoped<IAuthorRepository, InMemoryAuthorRepository>();
builder.Services.AddScoped<BookAnalyticsService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//**************** Configurer Identity + JWT [Début] sans appsettings.json ****************//

// // 1) DbContext InMemory
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("BookStoreAuth"));

// // 2) Identity
// builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//     .AddEntityFrameworkStores<AppDbContext>()
//     .AddDefaultTokenProviders();

// // 3) JWT Config
// var jwtKey = "this_is_a_super_super_secret_key_12345"; // ⚠️ à stocker dans appsettings.json plus tard
// var key = Encoding.ASCII.GetBytes(jwtKey);

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(key),
//         ValidateIssuer = false,
//         ValidateAudience = false
//     };
// });
//**************** Configurer Identity + JWT [Fin] sans appsettings.json ****************//


var app = builder.Build();


/**************** config de JWT avec appsettings.json DEBUT ****************/
// 1) Ajouter Identity + roles

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// 2) Configuration du JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtConfig["Key"];
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"]
    };
});

// 3) Créer automatiquement les rôles au démarrage
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // 🔥 Création automatique des rôles Admin / User
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

/**************** config de JWT avec appsettings.json FIN ****************/


// 🌱 SEEDING DES DONNÉES
var bookRepo = app.Services.GetRequiredService<IBookRepository>();

bookRepo.AddAsync(new Book { Title = "Livre A", Genre = "Fiction", SoldCopies = 100, Published = new DateTime(2011,01,12), Price = 23 }).Wait();
bookRepo.AddAsync(new Book { Title = "Livre B", Genre = "Fiction", SoldCopies = 200, Published = new DateTime(2001,01,12), Price = 10 }).Wait();
bookRepo.AddAsync(new Book { Title = "Livre C", Genre = "Non-Fiction", SoldCopies = 300, Published = new DateTime(2015,11,30), Price = 15 }).Wait();


Console.WriteLine("📚 Données initiales seedées avec succès !");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

