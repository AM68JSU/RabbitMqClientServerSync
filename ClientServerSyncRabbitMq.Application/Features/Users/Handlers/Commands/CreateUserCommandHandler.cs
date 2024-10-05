using AutoMapper;

using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using ClientServerSyncRabbitMq.Application.DTOs.User.Validators;
using ClientServerSyncRabbitMq.Application.Features.Users.Requests.Commands;
using ClientServerSyncRabbitMq.Application.Responses;
using ClientServerSyncRabbitMq.Domain.Entities;
using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Handlers.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository; 
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateUserDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateUserDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Failed.";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                var user = _mapper.Map<User>(request.CreateUserDto);
                user = await _userRepository.Add(user);

                response.Success = true;
                response.Message = "Creation Successful.";
                response.Id = user.Id;
            }

            return response;
        }
    }
}
