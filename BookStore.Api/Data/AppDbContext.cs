using BookStore.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Exemple de configuration minimale :
        builder.Entity<Book>()
               .HasKey(b => b.Id);

        builder.Entity<Author>()
               .HasKey(a => a.Id);

        // builder.Entity<Book>()
        //        .HasOne<Author>()            // si je veux relation Author -> Books
        //        .WithMany()
        //        .HasForeignKey(b => b.AuthorId)
        //        .OnDelete(DeleteBehavior.Cascade);
    }
}
