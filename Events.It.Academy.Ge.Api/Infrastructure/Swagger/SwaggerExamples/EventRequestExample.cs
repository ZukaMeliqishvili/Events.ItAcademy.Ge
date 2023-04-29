using Application._Event.Models.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Events.It.Academy.Ge.Api.Infrastructure.Swagger.SwaggerExamples
{
    public class EventRequestExample : IExamplesProvider<EventRequestModel>
    {
        EventRequestModel IExamplesProvider<EventRequestModel>.GetExamples()
        {
            return new EventRequestModel
            {
                Title = "Gia suramelashvili",
                Description = "See the brightest man in Philarmonia concert hall",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(3),
                NumberOfTickets = 5000,
                TicketPrice = 20,
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRuJEK5wZWp9Tx9VY-0OELN88WJTWWBq6rSsg&usqp=CAU"
            };
        }
    }
}
