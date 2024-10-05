namespace ClientServerSyncRabbitMq.Application.DTOs.User
{
    public interface IUserDto
    {
         string Name { get; set; }

         string Email { get; set; }

         bool IsSynced { get; set; }

         DateTime? LastSyncedAt { get; set; }

         Guid SyncGuid { get; set; } 

    }
}
