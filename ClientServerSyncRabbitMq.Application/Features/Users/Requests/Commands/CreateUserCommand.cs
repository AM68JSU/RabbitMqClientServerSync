using ClientServerSyncRabbitMq.Application.DTOs.User;
using ClientServerSyncRabbitMq.Application.Responses;

using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Requests.Commands
{
    public class CreateUserCommand : IRequest<BaseCommandResponse>
    {
        public CreateUserDto CreateUserDto { get; set; }
    }
}
