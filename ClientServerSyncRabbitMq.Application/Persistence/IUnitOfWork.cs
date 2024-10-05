using System;
using System.Threading.Tasks;
namespace ClientServerSyncRabbitMq.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }

        Task Save();
    }
}
