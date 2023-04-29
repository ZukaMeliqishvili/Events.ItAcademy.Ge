using Application._Event.Models.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Events.It.Academy.Ge.Api.Infrastructure.Swagger.SwaggerExamples
{
    public class EventResponseExample : IExamplesProvider<EventResponseModel>
    {

        EventResponseModel IExamplesProvider<EventResponseModel>.GetExamples()
        {
            return new EventResponseModel
            {
                Id = 12,
                Title = "Live Concert",
                Description = "Come and enjoy Best live concert in Georgia",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(2).AddHours(5),
                NumberOfTickets = 20000,
                TicketPrice = 120,
                IsConfirmed = true,
                ImageUrl = "https://www.rockadrome.com/store/images/detailed/22/deep-purple-made-in-japan-lp-b-600.jpg",
                UserId = new Guid().ToString(),
                ConfirmedAt = DateTime.Now,
                DaysForUpdate = 10
            };
        }
    }
}
