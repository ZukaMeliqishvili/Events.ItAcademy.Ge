using Application.Localization;
using Application.User.Models.Request;
using FluentValidation;

namespace Events.It.Academy.Ge.Api.Infrastructure.FluentValidations
{
    public class UserRegisterValidator:AbstractValidator<UserRegisterModel>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(20)
                .Matches("^[A-Za-z0-9]*$")
                .WithErrorCode(ErrorMessages.InvalidUsername);
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
                .WithMessage(ErrorMessages.InvalidPassword);
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(25)
                .Matches("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$")
                .WithMessage(ErrorMessages.InvalidEmail);
        }
    }
}
