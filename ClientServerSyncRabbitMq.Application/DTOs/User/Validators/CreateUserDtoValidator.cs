using FluentValidation;

namespace ClientServerSyncRabbitMq.Application.DTOs.User.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            Include(new IUserDtoValidator());
        }
    }
}
