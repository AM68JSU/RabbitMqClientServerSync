using System;
using MediatR;
using ClientServerSyncRabbitMq.Application.DTOs;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Commands
{
    public class AddUserCommand : IRequest
    {
        public UserDto User { get; set; }
    }
}
