using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using BookStore.Core.Services;
using BookStore.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container. (enregistrer les services dans le conteneur des dépendances DI)
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddScoped<IAuthorRepository, InMemoryAuthorRepository>();
builder.Services.AddScoped<BookAnalyticsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();

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
app.UseAuthorization();
app.MapControllers();

app.Run();

