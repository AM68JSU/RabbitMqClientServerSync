using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ClientServerSyncRabbitMq.Domain.Interfaces;
using ClientServerSyncRabbitMq.Application.DTOs;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    IsSynced = user.IsSynced
                });
            }

            return userDtos;
        }
    }
}
