using Practica.Domain.Entities;

namespace Practica.Application.Interfaces
{
    internal interface IUserService
    {
        Task AddUserAsync(User user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task RemoveUserAsync(int id);
        Task UpdateUserByIdAsync(int id, User user);
    }
}
