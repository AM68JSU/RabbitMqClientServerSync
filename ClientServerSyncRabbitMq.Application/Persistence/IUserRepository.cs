using System.Collections.Generic;
using System.Threading.Tasks;
using ClientServerSyncRabbitMq.Application.DTOs.User;
using ClientServerSyncRabbitMq.Domain.Entities;

namespace ClientServerSyncRabbitMq.Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericRepository<User>
    {
        //Task<User> GetUserWithDetails(int id);

        //Task<List<User>> GetUsersWithDetails();

        //Task<List<User>> GetUsersWithDetails(string userId);

        //Task<bool> UserExists(string userId);

        //Task AddUsers(IEnumerable<User> users);
        Task<User> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersFromClientAsync();

    }
}
