// Application/DTOs/CreateUserDto.cs

namespace ClientServerSyncRabbitMq.Application.DTOs.User
{
    public class CreateUserDto: IUserDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsSynced { get; set; } = false;

        public DateTime? LastSyncedAt { get; set; }

        public Guid SyncGuid { get; set; } = Guid.NewGuid();
    }
}
