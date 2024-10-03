// Application/DTOs/CreateUserDto.cs
namespace ClientServerSyncRabbitMq.Application.DTOs
{
    public class CreateUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Guid SyncGuid { get; set; } = Guid.NewGuid();
    }
}
