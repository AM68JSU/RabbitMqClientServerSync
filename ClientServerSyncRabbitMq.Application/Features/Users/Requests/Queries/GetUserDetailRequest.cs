using ClientServerSyncRabbitMq.Application.DTOs.User;

using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Requests.Queries
{
    public class GetUserDetailRequest : IRequest<UserDto>
    {
        public int Id { get; set; }
    }
}
