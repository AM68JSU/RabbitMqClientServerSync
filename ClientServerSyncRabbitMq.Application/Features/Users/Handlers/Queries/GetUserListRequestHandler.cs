using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using ClientServerSyncRabbitMq.Application.DTOs.User;
using ClientServerSyncRabbitMq.Application.Features.Users.Requests.Queries;
using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Handlers.Queries
{
    public class GetUserListRequestHandler : IRequestHandler<GetUserListRequest, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserListRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            var leaveTypes = await _userRepository.GetAll();
            return _mapper.Map<List<UserDto>>(leaveTypes);
        }

        Task<List<UserDto>> IRequestHandler<GetUserListRequest, List<UserDto>>.Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
