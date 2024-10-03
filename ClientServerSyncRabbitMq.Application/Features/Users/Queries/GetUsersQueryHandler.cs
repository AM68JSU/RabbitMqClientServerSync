using AutoMapper;
using ClientServerSyncRabbitMq.Application.DTOs;
using ClientServerSyncRabbitMq.Domain.Interfaces;
using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // دریافت لیست کاربران از ریپوزیتوری
            var users = await _userRepository.GetAllUsersAsync();

            // استفاده از AutoMapper برای تبدیل لیست کاربران به لیست UserDto
            var userDtos = _mapper.Map<List<UserDto>>(users);

            return userDtos;
        }
    }
}
