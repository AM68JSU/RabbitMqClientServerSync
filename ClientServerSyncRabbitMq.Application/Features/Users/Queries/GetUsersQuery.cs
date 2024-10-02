using System.Collections.Generic;
using MediatR;
using ClientServerSyncRabbitMq.Application.DTOs;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
    }
}
