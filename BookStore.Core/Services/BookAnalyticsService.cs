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

    // Récupère les Top 3 auteurs par nombre de livres vendus,
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
}
