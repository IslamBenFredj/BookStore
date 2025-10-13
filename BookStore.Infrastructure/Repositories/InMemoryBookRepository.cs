using BookStore.Core.Models;
public class InMemoryBookRepository : IBookRepository
{
    private readonly List<Book> _books = new();
    public Task AddAsync(Book book)
    {
        _books.Add(book);
        return Task.CompletedTask;
    }
    public Task<IEnumerable<Book>> GetAllAsync() => Task.FromResult(_books.AsEnumerable());

    public Task<Book?> GetByIdAsync(Guid id) => Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

    public Task RemoveAsync(Guid id)
    {
        var b = _books.FirstOrDefault(x => x.Id == id);
        if (b != null) _books.Remove(b);
        return Task.CompletedTask;
    }
    public Task UpdateAsync(Book book)
    {
        var idx = _books.FindIndex(x => x.Id == book.Id);
        if (idx >= 0) _books[idx] = book;
        return Task.CompletedTask;
    }

}
