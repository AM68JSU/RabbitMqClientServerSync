using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ClientServerSyncRabbitMq.Domain.Entities;
using ClientServerSyncRabbitMq.Domain.Interfaces;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Commands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public AddUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var userEntity = new User
            {
                Name = request.User.Name,
                Email = request.User.Email
            };

            await _userRepository.AddUserAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            return Unit.Value;
        }

        Task IRequestHandler<AddUserCommand>.Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
