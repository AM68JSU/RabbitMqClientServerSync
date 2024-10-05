using ClientServerSyncRabbitMq.Application.DTOs.Common;

namespace ClientServerSyncRabbitMq.Application.DTOs.User
{
    public class UserDto : BaseDto, IUserDto
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsSynced { get; set; } = false;

        public DateTime? LastSyncedAt { get; set; }

        public Guid SyncGuid { get; set; } = Guid.NewGuid(); 
    }
}
