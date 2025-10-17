using BookStore.Core.Interfaces;
using BookStore.Core.Models;

namespace BookStore.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult(_users.AsEnumerable());

    public Task<User?> GetByIdAsync(Guid id) =>
        Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}
