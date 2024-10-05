using System.Threading;
using System.Threading.Tasks;


using AutoMapper;
using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using ClientServerSyncRabbitMq.Application.DTOs.User;
using ClientServerSyncRabbitMq.Application.Features.Users.Requests.Queries;
using MediatR;



namespace ClientServerSyncRabbitMq.Application.Features.Users.Handlers.Queries
{
    public class GetUserDetailRequestHandler : IRequestHandler<GetUserDetailRequest, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserDetailRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserDetailRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.Id);
            return _mapper.Map<UserDto>(user);
        }
    }
}
