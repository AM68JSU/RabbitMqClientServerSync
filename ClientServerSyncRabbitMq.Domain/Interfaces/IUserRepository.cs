using System.Collections.Generic;
using System.Threading.Tasks;
using ClientServerSyncRabbitMq.Domain.Entities;

namespace ClientServerSyncRabbitMq.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserBySyncGuidAsync(Guid syncGuid);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task SaveChangesAsync();
    }
}
