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

    [Fact]
    public async Task GetSalesAverageByGenre_ReturnsCorrectAverages()
    {
        // Arrange
        var bookRepo = new InMemoryBookRepository();
        var book1 = new Book { Title = "Livre A", Genre = "Fiction", SoldCopies = 100 };
        var book2 = new Book { Title = "Livre B", Genre = "Fiction", SoldCopies = 200 };
        var book3 = new Book { Title = "Livre C", Genre = "Non-Fiction", SoldCopies = 300 };

        await bookRepo.AddAsync(book1);
        await bookRepo.AddAsync(book2);
        await bookRepo.AddAsync(book3);


        var service = new BookAnalyticsService(bookRepo, new InMemoryAuthorRepository());
        // Act
        var result = await service.GetSalesAverageByGenre();
        var list = result.ToList();

        // Assert
        Assert.Equal(150, list.First(g => g.Genre == "Fiction").AverageSales);
        Assert.Equal(300, list.First(g => g.Genre == "Non-Fiction").AverageSales);
    }
    // test de la méthode GetBooksPublishedAfter2010
    [Fact]
    public async Task GetBooksPublishedAfter2010_returnsBooksAfter2010()
    {
        // 🧰 ARRANGE
        var mockRepo = new InMemoryBookRepository();
        var authorRepo = new InMemoryAuthorRepository();

        var fakeBooks = new List<Book>
        {
            new Book { Title = "Livre A", Genre = "Genre A", Published = new DateTime(2011,1,9)},
            new Book { Title = "Livre B", Genre = "Genre A", Published = new DateTime(2008,6,18)},
            new Book { Title = "Livre C", Genre = "Genre A", Published = new DateTime(2020,11,30)},
        };

        foreach (Book b in fakeBooks)
        {
            await mockRepo.AddAsync(b);
        }

        var service = new BookAnalyticsService(mockRepo, authorRepo);

        // 🧪 ACT
        var result = await service.GetBooksPublishedAfter2010();

        // ✅ ASSERT
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, b => b.Title == "Livre A");
        Assert.DoesNotContain(list, b => b.Title == "Livre B");
        Assert.Contains(list, b => b.Title == "Livre C");
    }

    [Fact]
    public async Task getAllBooksThaCostMoreThen20EurosTest()
    {
        // ARRANGE
        //ici j'appelle le repo
        var _mockBookRepo = new InMemoryBookRepository();
        var _mockAuthorRepo = new InMemoryAuthorRepository();
        // j'instancie quelques Books pour les tests unitaire - je vais pas utiliser les données de seed dans Program.cs
        var fakeBooks = new List<Book>
        {
            new Book { Title = "title 1", Genre = "Genre 1", Price = 15 },
            new Book { Title = "title 2", Genre = "Genre 2", Price = 25 },
            new Book { Title = "title 3", Genre = "Genre 1", Price = 110 },
        };

        foreach (Book b in fakeBooks)
        {
            await _mockBookRepo.AddAsync(b);
        }
        ;
        var service = new BookAnalyticsService(_mockBookRepo, _mockAuthorRepo);

        // ACT
        var result = await service.getAllBooksThaCostMoreThen20Euros();

        var list = result.ToList();

        // ASSERT
        Assert.Equal(2, list.Count);
        Assert.Contains("title 2", list);
        Console.WriteLine("test 1 passed ");
        Assert.Contains("title 3", list);
        Console.WriteLine("test 2 passed ");
        Assert.DoesNotContain("title 1", list);
    }

    [Fact]
    public async Task GetBooksOrderedByPublishDateTest()
    {
        // Arrange
        var _bookRepoMock = new InMemoryBookRepository();
        var _authorRepoMock = new InMemoryAuthorRepository();
        var fakeBooks = new List<Book>
        {
            new Book { Title = "Livre A", Genre = "Fiction", SoldCopies = 100, Published = new DateTime(2011,01,12), Price = 23 },
            new Book { Title = "Livre B", Genre = "Fiction", SoldCopies = 200, Published = new DateTime(2001,01,12), Price = 10 },
            new Book { Title = "Livre C", Genre = "Non-Fiction", SoldCopies = 300, Published = new DateTime(2015,11,30), Price = 15 }
        };
        // Act
        
        // Assert
    }
}
