using Application._Event.Models.Request;
using Application.Localization;
using FluentValidation;

namespace Events.It.Academy.Ge.Api.Infrastructure.FluentValidations
{
    public class EventUpdateRequestModelValidator:AbstractValidator<EventUpdateRequestModel>
    {
        public EventUpdateRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .WithMessage(ErrorMessages.EmptyTitle)
                .MinimumLength(5)
                .MaximumLength(30)
                .WithMessage(ErrorMessages.TitleLength);
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage(ErrorMessages.EmptyDescription)
                .MinimumLength(10)
                .MaximumLength(200)
                .WithMessage(ErrorMessages.DescriptionLength);
        }
    }
}
