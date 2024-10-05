using System.Collections.Generic;

using ClientServerSyncRabbitMq.Application.DTOs.User;

using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Requests.Queries
{
    public class GetUserListRequest : IRequest<List<UserDto>>
    {
    }
}
