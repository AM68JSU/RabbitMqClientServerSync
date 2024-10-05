using ClientServerSyncRabbitMq.Application.DTOs.User;

using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Requests.Commands
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public UserDto UserDto { get; set; }
    }
}
