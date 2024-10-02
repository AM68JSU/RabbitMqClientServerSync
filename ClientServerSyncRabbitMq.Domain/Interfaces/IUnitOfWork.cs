namespace ClientServerSyncRabbitMq.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        Task SaveChangesAsync();
    }
}
