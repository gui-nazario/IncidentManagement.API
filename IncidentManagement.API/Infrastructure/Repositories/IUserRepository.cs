using IncidentManagement.API.Domain.Entities;

namespace IncidentManagement.API.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task UpdateAsync(User user);
    Task AddAsync(User user);
}