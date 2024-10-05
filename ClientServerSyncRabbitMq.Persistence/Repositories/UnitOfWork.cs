using System;
using System.Threading.Tasks;

using ClientServerSyncRabbitMq.Application.Constants;
using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ClientServerSyncRabbitMq.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext; // استفاده از DbContext عمومی
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUserRepository _userRepository;

        public UnitOfWork(DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_dbContext);
        
        public async Task Save()
        {
         //   var username = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
