// Application/DTOs/UserDto.cs
namespace ClientServerSyncRabbitMq.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsSynced { get; set; } = false;

        public DateTime? LastSyncedAt { get; set; }

        public Guid SyncGuid { get; set; } = Guid.NewGuid();
    }
}
