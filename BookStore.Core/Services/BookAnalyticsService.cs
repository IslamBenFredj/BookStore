using BookStore.Core.DTOs;
using BookStore.Core.Interfaces;

namespace BookStore.Core.Services;

public class BookAnalyticsService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public BookAnalyticsService(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    // Récupère les Top 3 auteurs par nombre de livres vendus
    public async Task<IEnumerable<AuthorSalesDto>> GetTopAuthorsBySalesAsync(int topN = 3)
    {
        var books = await _bookRepository.GetAllAsync();
        var authors = await _authorRepository.GetAllAsync();

        var query =
            from b in books
            group b by b.AuthorId into g
            join a in authors on g.Key equals a.Id
            orderby g.Sum(x => x.SoldCopies) descending
            select new AuthorSalesDto
            {
                AuthorName = a.FullName,
                TotalSales = g.Sum(x => x.SoldCopies)
            };

        return query.Take(topN);
    }
    // calculer la moyenne des ventes (SoldCopies) par genre de livre (Genre) 
    public async Task<IEnumerable<GenreSalesDto>> GetSalesAverageByGenre()
    {
        // 1- récupérer tous les livres
        var books = await _bookRepository.GetAllAsync();

        Console.WriteLine($"Nombre de livres trouvés : {books.Count()}");

        // 2- grouper les livres par genre
        var query = from b in books
                        // 3- pour chaque groupe, calculer la moyenne des ventes
                    group b by b.Genre into g
                    select new GenreSalesDto
                    {
                        Genre = g.Key,
                        AverageSales = g.Average(x => x.SoldCopies)
                    };
        
        // 4- retourner une liste de GenreSalesDto
        return query;
    }
}
