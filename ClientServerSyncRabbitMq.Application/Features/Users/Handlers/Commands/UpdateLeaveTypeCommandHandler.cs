

using AutoMapper;
using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using ClientServerSyncRabbitMq.Application.DTOs.User.Validators;
using ClientServerSyncRabbitMq.Application.Exceptions;
using ClientServerSyncRabbitMq.Application.Features.Users.Requests.Commands;
using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Handlers.Commands
{
    public class UpdateUsersCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUsersCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateUserDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UserDto, cancellationToken);

            if (validationResult.IsValid == false)
            {
                throw new ValidationException(validationResult);
            }

            var user = await _userRepository.Get(request.UserDto.Id);

            if (user == null)
            {
                throw new NotFoundException(nameof(Users), request.UserDto.Id);
            }

            _mapper.Map(request.UserDto, user);

            await _userRepository.Update(user);

            return Unit.Value;
        }
    }
}
