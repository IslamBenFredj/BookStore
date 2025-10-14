using BookStore.Core.Models;
using BookStore.Core.Interfaces;

namespace BookStore.Infrastructure.Repositories;

public class InMemoryAuthorRepository : IAuthorRepository
{
    private readonly List<Author> _authors = new();

    public Task<IEnumerable<Author>> GetAllAsync() => Task.FromResult(_authors.AsEnumerable());

    public Task AddAsync(Author author)
    {
        _authors.Add(author);
        return Task.CompletedTask;
    }

    public Task<Author?> GetByIdAsync(Guid id)
        => Task.FromResult(_authors.FirstOrDefault(a => a.Id == id));
}
