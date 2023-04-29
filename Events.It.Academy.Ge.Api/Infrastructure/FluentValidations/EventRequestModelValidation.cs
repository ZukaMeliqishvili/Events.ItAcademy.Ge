using Application._Event.Models.Request;
using Application.Localization;
using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Events.It.Academy.Ge.Api.Infrastructure.FluentValidations
{
    public class EventRequestModelValidation: AbstractValidator<EventRequestModel>
    {
        public EventRequestModelValidation()
        {
            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .WithMessage(ErrorMessages.EmptyTitle)
                .MinimumLength(5)
                .MaximumLength(50)
                .WithMessage(ErrorMessages.TitleLength);
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage(ErrorMessages.EmptyDescription)
                .MinimumLength(10)
                .MaximumLength(200)
                .WithMessage(ErrorMessages.DescriptionLength);
            RuleFor(x => x.StartDate)
                .NotNull()
                .WithMessage(ErrorMessages.EmptyStartDate)
                .Must(x => x > DateTime.Now)
                .WithMessage(ErrorMessages.InvalidDate);
            RuleFor(x=> x.EndDate)
                .NotNull()
                .WithMessage(ErrorMessages.EmptyEndDate)
                .Must(x => x > DateTime.Now)
                .WithMessage(ErrorMessages.InvalidDate);
            RuleFor(x => x.NumberOfTickets)
                .Must(x => x > 0)
                .WithMessage(ErrorMessages.NumberOfTickets);
            RuleFor(x => x.TicketPrice)
                .Must(x => x >= 1)
                .WithMessage(ErrorMessages.Price);

        }
    }
}
