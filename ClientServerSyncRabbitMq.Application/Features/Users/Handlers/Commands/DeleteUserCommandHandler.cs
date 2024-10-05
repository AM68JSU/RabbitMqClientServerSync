using System.Threading;
using System.Threading.Tasks;
using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using ClientServerSyncRabbitMq.Application.Exceptions;
using ClientServerSyncRabbitMq.Application.Features.Users.Requests.Commands;
using ClientServerSyncRabbitMq.Domain.Entities;
using MediatR;

namespace ClientServerSyncRabbitMq.Application.Features.Users.Handlers.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.Id);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            await _userRepository.Delete(user);

            return Unit.Value;
        }

        Task IRequestHandler<DeleteUserCommand>.Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
