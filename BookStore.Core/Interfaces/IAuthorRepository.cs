using BookStore.Core.Models;

namespace BookStore.Core.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task AddAsync(Author author);
    Task<Author?> GetByIdAsync(Guid id);
}
