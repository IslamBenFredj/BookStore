using Xunit;
using BookStore.Core.Services;
using BookStore.Infrastructure.Repositories;
using BookStore.Core.Models;

namespace BookStore.Tests;

public class BookAnalyticsServiceTests
{
    [Fact]
    public async Task GetTopAuthorsBySalesAsync_ReturnsAuthorsOrderedBySales()
    {
        // Arrange
        var bookRepo = new InMemoryBookRepository();
        var authorRepo = new InMemoryAuthorRepository();

        var author1 = new Author { FirstName = "Alice", LastName = "Martin" };
        var author2 = new Author { FirstName = "Bob", LastName = "Durand" };
        var author3 = new Author { FirstName = "Charlie", LastName = "Dupont" };

        await authorRepo.AddAsync(author1);
        await authorRepo.AddAsync(author2);
        await authorRepo.AddAsync(author3);

        await bookRepo.AddAsync(new Book { Title = "Livre A", AuthorId = author1.Id, SoldCopies = 50 });
        await bookRepo.AddAsync(new Book { Title = "Livre B", AuthorId = author2.Id, SoldCopies = 200 });
        await bookRepo.AddAsync(new Book { Title = "Livre C", AuthorId = author3.Id, SoldCopies = 100 });

        var service = new BookAnalyticsService(bookRepo, authorRepo);

        // Act
        var result = await service.GetTopAuthorsBySalesAsync(3);

        // Assert
        var list = result.ToList();
        Assert.Equal("Bob Durand", list[0].AuthorName);
        Assert.Equal(200, list[0].TotalSales);
    }
}
