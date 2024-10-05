using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using ClientServerSyncRabbitMq.Application.DTOs.User;
using ClientServerSyncRabbitMq.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientServerSyncRabbitMq.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _dbContext;

        public UserRepository(DbContext dbContext) // سازنده با DbContext عمومی
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Set<User>().FindAsync(id);
            return user != null ? new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                SyncGuid = user.SyncGuid
            } : null; // می‌توانید مدیریت خطا یا خروجی دیگر را اینجا انجام دهید
        }

        public async Task<List<User>> GetAllUsersFromClientAsync()
        {
            return await _dbContext.Set<User>()
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    SyncGuid = u.SyncGuid
                }).ToListAsync();
        }

        public Task<User> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Add(User entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(User entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(User entity)
        {
            throw new NotImplementedException();
        }

        // سایر متدها...
    }

}
