using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClientServerSyncRabbitMq.Domain.Entities;
using ClientServerSyncRabbitMq.Domain.Interfaces;

namespace ClientServerSyncRabbitMq.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ServerDbContext _context;

        public UserRepository(ServerDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserBySyncGuidAsync(Guid syncGuid)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.SyncGuid == syncGuid);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
