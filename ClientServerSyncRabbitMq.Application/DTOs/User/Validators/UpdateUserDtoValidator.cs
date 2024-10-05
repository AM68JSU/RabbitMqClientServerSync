using FluentValidation;

namespace ClientServerSyncRabbitMq.Application.DTOs.User.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UserDto>
    {
        public UpdateUserDtoValidator()
        {
            Include(new IUserDtoValidator());

            RuleFor(p => p.Id)
                .NotNull()
                .WithMessage("{PropertyName} must be present.");
        }
    }
}
