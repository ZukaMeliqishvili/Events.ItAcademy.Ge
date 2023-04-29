using Application._Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Events.It.Academy.Ge.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IEventService _eventService;

        public TicketController(IEventService eventService) => _eventService = eventService;

        protected virtual string GetUserId() => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        [Authorize]
        [HttpPost("Book/{id}")]
        public async Task BookTicket(int id, CancellationToken cancellationToken)
        {
            await _eventService.BookTicket(id, GetUserId(), cancellationToken);
        }
        [Authorize]
        [HttpPut("buy/{id}")]
        public async Task BuyTicket(int id, CancellationToken cancellationToken)
        {
            await _eventService.BuyTicket(id, GetUserId(), cancellationToken);
        }
    }
}
