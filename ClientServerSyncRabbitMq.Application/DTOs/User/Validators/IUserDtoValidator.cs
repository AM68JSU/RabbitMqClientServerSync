using ClientServerSyncRabbitMq.Application.DTOs.User;
using FluentValidation;

namespace ClientServerSyncRabbitMq.Application.DTOs.User.Validators
{
    public class IUserDtoValidator : AbstractValidator<IUserDto>
    {
        public IUserDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

        }
    }
}
